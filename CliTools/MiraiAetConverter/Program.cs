using MikuMikuLibrary.Aets;
using MikuMikuLibrary.IO;

namespace MiraiAetConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MiraiAetSet set = BinaryFile.Load<MiraiAetSet>(args[0]);

            Console.WriteLine("AET Information:");
            Console.WriteLine($"\tNum Scenes: {set.Scenes.Count}");

            using (var newSet = set.GetClassicAetSet())
            {
                newSet.Save(args[1]);
            }
        }
    }
}
