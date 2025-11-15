using MikuMikuLibrary.IBLs;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.IO.Common;
using MikuMikuLibrary.IO.Sections;
using MikuMikuLibrary.Textures;

namespace MikuMikuLibrary.Databases;

public class ToonCurveInfo
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public List<ColorPoint> DiffuseCurvePoints { get; }
    public List<ColorPoint> SpecularCurvePoints { get; }
    public List<ColorPoint> FresnelCurvePoints { get; }

    public void Read(EndianBinaryReader reader)
    {
        Id = reader.ReadUInt32();
        Name = reader.ReadStringOffset(StringBinaryFormat.NullTerminated);
        int diffusePointCount = reader.ReadInt32();
        reader.ReadOffset(() =>
        {
            DiffuseCurvePoints.Capacity = diffusePointCount;
            for (int j = 0; j < diffusePointCount; j++)
            {
                DiffuseCurvePoints.Add(new ColorPoint()
                {
                    Color = reader.ReadVector3(),
                    Offset = reader.ReadInt32()
                });
            }
        });
        int specularPointCount = reader.ReadInt32();
        reader.ReadOffset(() =>
        {
            SpecularCurvePoints.Capacity = specularPointCount;
            for (int j = 0; j < specularPointCount; j++)
            {
                SpecularCurvePoints.Add(new ColorPoint()
                {
                    Color = reader.ReadVector3(),
                    Offset = reader.ReadInt32()
                });
            }
        });
        int fresnelPointCount = reader.ReadInt32();
        reader.ReadOffset(() =>
        {
            FresnelCurvePoints.Capacity = fresnelPointCount;
            for (int j = 0; j < fresnelPointCount; j++)
            {
                FresnelCurvePoints.Add(new ColorPoint()
                {
                    Color = reader.ReadVector3(),
                    Offset = reader.ReadInt32()
                });
            }
        });
    }

    public void Write(EndianBinaryWriter writer)
    {
        writer.Write(Id);
        writer.WriteStringOffset(Name);
        writer.Write(DiffuseCurvePoints.Count);
        writer.WriteOffsetIf(DiffuseCurvePoints.Count > 0, () => {
            foreach (var point in DiffuseCurvePoints)
            {
                writer.Write(point.Color);
                writer.Write(point.Offset);
            }
        });
        writer.Write(SpecularCurvePoints.Count);
        writer.WriteOffsetIf(SpecularCurvePoints.Count > 0, () => {
            foreach (var point in SpecularCurvePoints)
            {
                writer.Write(point.Color);
                writer.Write(point.Offset);
            }
        });
        writer.Write(FresnelCurvePoints.Count);
        writer.WriteOffsetIf(FresnelCurvePoints.Count > 0, () => {
            foreach (var point in FresnelCurvePoints)
            {
                writer.Write(point.Color);
                writer.Write(point.Offset);
            }
        });
    }

    public ToonCurveInfo()
    {
        DiffuseCurvePoints = new List<ColorPoint>();
        SpecularCurvePoints = new List<ColorPoint>();
        FresnelCurvePoints = new List<ColorPoint>();
    }
}

public class ToonCurveDatabase : BinaryFile
{
    public override BinaryFileFlags Flags =>
        BinaryFileFlags.Load | BinaryFileFlags.Save | BinaryFileFlags.HasSectionFormat;

    public List<ToonCurveInfo> ToonCurves { get; }

    public override void Read(EndianBinaryReader reader, ISection section = null)
    {
        int toonCurveCount = reader.ReadInt32();
        long toonCurveOffset = reader.ReadOffset();

        reader.ReadAtOffset(toonCurveOffset, () =>
        {
            ToonCurves.Capacity = toonCurveCount;

            for (int i = 0; i < toonCurveCount; i++)
            {
                ToonCurveInfo toonCurve = new ToonCurveInfo();
                toonCurve.Read(reader);
                ToonCurves.Add(toonCurve);
            }
        });
    }

    public override void Write(EndianBinaryWriter writer, ISection section = null)
    {
        writer.Write(ToonCurves.Count);
        writer.WriteOffset(16, AlignmentMode.Left, () =>
        {
            foreach (var toonCurveInfo in ToonCurves)
            {
                toonCurveInfo.Write(writer);
            }
        });
    }

    public override void Save(string filePath)
    {
        // Assume it's being exported for F2nd PS3
        if (BinaryFormatUtilities.IsClassic(Format) &&
            filePath.EndsWith(".tci", StringComparison.OrdinalIgnoreCase))
        {
            Format = BinaryFormat.F2nd;
            Endianness = Endianness.Big;
        }

        // Or vice versa
        else if (BinaryFormatUtilities.IsModern(Format) &&
                 filePath.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
        {
            Format = BinaryFormat.DT;
            Endianness = Endianness.Little;
        }

        base.Save(filePath);
    }

    public ToonCurveInfo GetToonCurveInfo(string textureName) =>
        ToonCurves.FirstOrDefault(x => x.Name.Equals(textureName, StringComparison.OrdinalIgnoreCase));

    public ToonCurveInfo GetToonCurveInfo(uint textureId) =>
        ToonCurves.FirstOrDefault(x => x.Id.Equals(textureId));

    public ToonCurveDatabase()
    {
        ToonCurves = new List<ToonCurveInfo>();
    }
}