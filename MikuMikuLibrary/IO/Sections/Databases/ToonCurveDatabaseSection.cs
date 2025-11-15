using MikuMikuLibrary.Databases;
using MikuMikuLibrary.IO.Sections.IO;

namespace MikuMikuLibrary.IO.Sections.Databases;

[Section("MTCI")]
public class ToonCurveDatabaseSection : BinaryFileSection<ToonCurveDatabase>
{
    public override SectionFlags Flags => SectionFlags.None;

    public ToonCurveDatabaseSection(SectionMode mode, ToonCurveDatabase data = null) : base(mode, data)
    {
    }
}