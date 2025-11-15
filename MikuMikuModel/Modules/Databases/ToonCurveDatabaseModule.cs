using MikuMikuLibrary.Databases;
using MikuMikuLibrary.IO;

namespace MikuMikuModel.Modules.Databases;

public class ToonCurveDatabaseModule : FormatModule<ToonCurveDatabase>
{
    public override IReadOnlyList<FormatExtension> Extensions { get; } = new[]
    {
        new FormatExtension("ToonCurve Database (Classic)", "bin", FormatExtensionFlags.Import | FormatExtensionFlags.Export),
        new FormatExtension("ToonCurve Database (Modern)", "tci", FormatExtensionFlags.Import | FormatExtensionFlags.Export)
    };

    public override bool Match(string fileName)
    {
        if (fileName.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
        {
            if (fileName.StartsWith("mdata_", StringComparison.OrdinalIgnoreCase))
                fileName = fileName.Remove(0, 6);

            return Path.GetFileNameWithoutExtension(fileName)
                .Equals("tc_db", StringComparison.OrdinalIgnoreCase);
        }

        return base.Match(fileName);
    }

    protected override ToonCurveDatabase ImportCore(Stream source, string fileName)
    {
        return BinaryFile.Load<ToonCurveDatabase>(source, true);
    }

    protected override void ExportCore(ToonCurveDatabase model, Stream destination, string fileName)
    {
        model.Save(destination, true);
    }
}