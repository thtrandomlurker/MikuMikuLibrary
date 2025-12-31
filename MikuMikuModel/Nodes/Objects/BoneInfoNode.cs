using MikuMikuLibrary.Extensions;
using MikuMikuLibrary.Objects;
using MikuMikuLibrary.Objects.Extra;
using MikuMikuLibrary.Objects.Extra.Blocks;
using MikuMikuModel.GUI.Forms;
using MikuMikuModel.Nodes.TypeConverters;

namespace MikuMikuModel.Nodes.Objects;

public class BoneInfoNode : Node<BoneInfo>
{
    public override NodeFlags Flags => NodeFlags.Rename;

    [Category("General")]
    [DisplayName("Parent name")]
    public string ParentBone =>
        GetProperty<BoneInfo>(nameof(BoneInfo.Parent))?.Name;

    [Category("General")]
    [TypeConverter(typeof(IdTypeConverter))]
    public uint Id
    {
        get => GetProperty<uint>();
        set => SetProperty(value);
    }

    [Category("General")]
    [DisplayName("Inverse bind pose matrix")]
    public Matrix4x4 InverseBindPoseMatrix
    {
        get => GetProperty<Matrix4x4>();
        set => SetProperty(value);
    }

    [Category("General")]
    [DisplayName("Belongs in ex data")]
    public bool IsEx => GetProperty<bool>();

    protected override void Initialize()
    {
        AddCustomHandler("Create Expression Block from Bone", () =>
        {
            Matrix4x4.Invert(Data.InverseBindPoseMatrix, out var bindPoseMatrix);
            var matrix = Matrix4x4.Multiply(bindPoseMatrix,
                Data.Parent?.InverseBindPoseMatrix ?? Matrix4x4.Identity);

            Matrix4x4.Decompose(matrix, out var scale, out var rotation, out var translation);
            rotation = Quaternion.Normalize(rotation);

            ExpressionBlock exp = new ExpressionBlock();
            exp.Position = translation;
            exp.Rotation = rotation.ToEulerAngles();
            exp.Scale = scale;
            exp.Name = $"{Data.Name}";
            exp.ParentName = Data.Parent?.Name ?? "";

            SkinNode skin = Parent.Parent as SkinNode;
            skin.Data.Blocks.Add(exp);
        });
        AddCustomHandler("Create Osage Block from Bone", () => {
            Matrix4x4.Invert(Data.InverseBindPoseMatrix, out var bindPoseMatrix);
            var matrix = Matrix4x4.Multiply(bindPoseMatrix,
                Data.Parent?.InverseBindPoseMatrix ?? Matrix4x4.Identity);

            Matrix4x4.Decompose(matrix, out var scale, out var rotation, out var translation);
            rotation = Quaternion.Normalize(rotation);

            List<(BoneInfoNode, string)> info = new List<(BoneInfoNode, string)>();
            // a BoneInfo's node always points to the folder containing all BoneInfoNodes present inside an object.



            foreach (var node in Parent.Nodes)
            {
                info.Add(((BoneInfoNode)node, node.Name));
            }

            using (ItemSelectForm<BoneInfoNode> boneInfoSelectForm = new ItemSelectForm<BoneInfoNode>(info) { Text = "Select Osage Chain Bones." })
            {
                if (boneInfoSelectForm.ShowDialog() == DialogResult.OK)
                {
                    OsageBlock osg = new OsageBlock();
                    osg.Position = translation;
                    osg.Rotation = rotation.ToEulerAngles();
                    osg.Scale = scale;
                    osg.Name = $"e_{Data.Name}";
                    osg.ParentName = Data.Parent?.Name ?? "";
                    osg.ExternalName = $"c_{Data.Name}_osg";

                    bool extraNode = false;

                    if (MessageBox.Show("Add extra end node to osage block?", "Create Osage Block on Bone", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        extraNode = true;
                    }

                    List<BoneInfoNode> sortedNodes = new List<BoneInfoNode>();

                    void NodeSort(BoneInfoNode boneInfo)
                    {
                        sortedNodes.Add(boneInfo);
                        BoneInfoNode child = boneInfoSelectForm.CheckedItems.FirstOrDefault(x => x.Data.Parent.Name == boneInfo.Data.Name, null);
                        if (child != null)
                        {
                            NodeSort(child);
                        }
                    }
                    NodeSort(this);

                    for (int i = 0; i < sortedNodes.Count; i++)
                    {
                        float length = 0;
                        BoneInfoNode node = sortedNodes[i];
                        Matrix4x4.Invert(node.Data.InverseBindPoseMatrix, out var nodeBindPoseMatrix);
                        var nodeMatrix = Matrix4x4.Multiply(nodeBindPoseMatrix,
                            node.Data.Parent?.InverseBindPoseMatrix ?? Matrix4x4.Identity);

                        Matrix4x4.Decompose(nodeMatrix, out var nodeScale, out var nodeRotation, out var nodeTranslation);
                        if (i != sortedNodes.Count - 1)
                        {
                            BoneInfoNode child = sortedNodes[i + 1];
                            Matrix4x4.Invert(child.Data.InverseBindPoseMatrix, out var childBindPoseMatrix);
                            var childMatrix = Matrix4x4.Multiply(childBindPoseMatrix,
                                child.Data.Parent?.InverseBindPoseMatrix ?? Matrix4x4.Identity);

                            Matrix4x4.Decompose(childMatrix, out var childScale, out var childRotation, out var childTranslation);

                            // translation is the offset from parent to child, therefore length would be sqrt(x*x + y*y + z*z)
                            length = (float)Math.Sqrt(childTranslation.X * childTranslation.X + childTranslation.Y * childTranslation.Y + childTranslation.Z * childTranslation.Z);
                        }
                        else
                        {
                            // translation is the offset from parent to child, therefore length would be sqrt(x*x + y*y + z*z)
                            length = (float)Math.Sqrt(nodeTranslation.X * nodeTranslation.X + nodeTranslation.Y * nodeTranslation.Y + nodeTranslation.Z * nodeTranslation.Z);
                        }

                        OsageNode osgnode = new OsageNode();
                        osgnode.Name = sortedNodes[i].Name;
                        osgnode.Length = length;
                        if (sortedNodes[i].Name != Data.Name)
                        {
                            osgnode.Rotation = nodeRotation.ToEulerAngles();
                        }

                        osg.Nodes.Add(osgnode);
                    }

                    if (extraNode)
                    {
                        OsageNode endNode = new OsageNode();
                        endNode.Name = $"{Data.Name}_end";
                        endNode.Length = osg.Nodes.Last().Length;
                        endNode.Rotation = Vector3.Zero;
                        osg.Nodes.Add(endNode);
                    }

                    SkinNode skinNode = Parent.Parent as SkinNode;
                    skinNode.Data.Blocks.Add(osg);

                    NotifyModified(NodeModifyFlags.Collection);
                }
            }
        });
    }

    protected override void PopulateCore()
    {
    }

    protected override void SynchronizeCore()
    {
    }

    public BoneInfoNode(string name, BoneInfo data) : base(name, data)
    {
    }
}