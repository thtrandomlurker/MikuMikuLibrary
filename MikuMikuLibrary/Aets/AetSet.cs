using MikuMikuLibrary.IO;
using MikuMikuLibrary.IO.Common;
using MikuMikuLibrary.IO.Sections;

namespace MikuMikuLibrary.Aets;

public class AetSet : BinaryFile
{
    public override BinaryFileFlags Flags =>
        BinaryFileFlags.Load | BinaryFileFlags.Save | BinaryFileFlags.HasSectionFormat;

    public override Encoding Encoding
    {
        get
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("shift-jis");
        }
    }

    public List<Scene> Scenes { get; }

    public override void Read(EndianBinaryReader reader, ISection section = null)
    {
        while (reader.Position < reader.Length)
        {
            long offset = reader.ReadOffset();

            if (offset == 0)
                return;

            reader.ReadAtOffset(offset, () =>
            {
                var scene = new Scene();
                scene.Read(reader);

                Scenes.Add(scene);
            });
        }
    }

    public override void Write(EndianBinaryWriter writer, ISection section = null)
    {
        foreach (var scene in Scenes)
            writer.WriteOffset(16, AlignmentMode.Left, () => scene.Write(writer));

        writer.WriteNulls(writer.AddressSpace.GetByteSize());
    }

    public AetSet()
    {
        Scenes = new List<Scene>();
    }
}

public class MiraiAetSet : AetSet
{

    public override void Read(EndianBinaryReader reader, ISection section = null)
    {
        int sceneCountOffset = reader.ReadInt32();
        int scenePointersOffset = reader.ReadInt32();
        int sceneNamesOffset = reader.ReadInt32();
        int sceneElementInfoOffset = reader.ReadInt32();
        Console.WriteLine($"Header Info:\n\tSceneCountPosition: {sceneCountOffset}\n\tScenePointersPosition: {scenePointersOffset}\n\tSceneNamePointersPosition: {sceneNamesOffset}\n\tSceneElementInfoPosition: {sceneElementInfoOffset}");

        reader.Seek(sceneCountOffset, SeekOrigin.Begin);

        int sceneCount = reader.ReadInt32();
        Console.WriteLine($"Number of Scenes: {sceneCount}");
        Scenes.Capacity = sceneCount;

        reader.Seek(scenePointersOffset, SeekOrigin.Begin);

        for (int i = 0; i < sceneCount; i++)
        {
            reader.ReadOffset(() =>
            {
                var scene = new MiraiScene();
                scene.Read(reader);
                Scenes.Add(scene);
            });
        }

        reader.Seek(sceneNamesOffset, SeekOrigin.Begin);

        for (int i = 0; i < sceneCount; i++)
        {
            string name = reader.ReadStringOffset(StringBinaryFormat.NullTerminated);
            Scenes[i].Name = name;
            Console.WriteLine($"Scene {i} name: {name}");
        }
        reader.Seek(sceneElementInfoOffset, SeekOrigin.Begin);

        for (int i = 0; i < sceneCount; i++)
        {
            List<string> videoNames = new List<string>();

            int sourceNamesOffset = reader.ReadInt32();
            int unkOffset = reader.ReadInt32();

            if (Scenes[i] is MiraiScene mScene)
            {
                foreach (var video in mScene.Videos)
                {
                    if (video is MiraiVideo mVideo)
                    {
                        foreach (var source in mVideo.Sources)
                        {
                            if (source is MiraiVideoSource mSource)
                            {
                                int nameOffsetPosition = sourceNamesOffset + (mSource.NameIndex * 4);

                                reader.ReadAtOffset(nameOffsetPosition, () =>
                                {
                                    mSource.Name = reader.ReadStringOffset(StringBinaryFormat.NullTerminated);
                                });
                            }

                        }
                    }
                }
            }
        }
    }

    public override void Write(EndianBinaryWriter writer, ISection section = null)
    {
        throw new NotImplementedException();
    }

    public AetSet GetClassicAetSet()
    {
        AetSet classicSet = new AetSet();
        foreach (var scene in Scenes)
        {
            if (scene is MiraiScene mScene)
            {
                classicSet.Scenes.Add(mScene.GetClassicScene());
            }
        }
        return classicSet;
    }

    public MiraiAetSet() : base()
    {

    }
}