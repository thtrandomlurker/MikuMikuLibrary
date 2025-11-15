using MikuMikuLibrary.Databases;
using MikuMikuLibrary.IO;
using MikuMikuModel.Nodes.Collections;
using MikuMikuModel.Nodes.IO;
using MikuMikuModel.Nodes.TypeConverters;

namespace MikuMikuModel.Nodes.Databases;

public class ToonCurveDatabaseNode : BinaryFileNode<ToonCurveDatabase>
{
    public override NodeFlags Flags =>
        NodeFlags.Add | NodeFlags.Export | NodeFlags.Replace | NodeFlags.Rename;

    protected override void Initialize()
    {
        AddExportHandler<ToonCurveDatabase>(filePath => Data.Save(filePath));
        AddReplaceHandler<ToonCurveDatabase>(BinaryFile.Load<ToonCurveDatabase>);

        base.Initialize();
    }

    protected override void PopulateCore()
    {
        Nodes.Add(new ListNode<ToonCurveInfo>("ToonCurves", Data.ToonCurves, x => x.Name));
    }

    protected override void SynchronizeCore()
    {
    }

    public ToonCurveDatabaseNode(string name, ToonCurveDatabase data) : base(name, data)
    {
    }

    public ToonCurveDatabaseNode(string name, Func<Stream> streamGetter) : base(name, streamGetter)
    {
    }
}

public class ToonCurveInfoNode : Node<ToonCurveInfo>
{
    public override NodeFlags Flags => NodeFlags.Rename;

    [Category("General")]
    [TypeConverter(typeof(IdTypeConverter))]
    public uint Id
    {
        get => GetProperty<uint>();
        set => SetProperty(value);
    }

    protected override void Initialize()
    {
    }

    protected override void PopulateCore()
    {
    }

    protected override void SynchronizeCore()
    {
    }

    public ToonCurveInfoNode(string name, ToonCurveInfo data) : base(name, data)
    {
    }
}