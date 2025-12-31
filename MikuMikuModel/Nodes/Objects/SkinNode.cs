using MikuMikuLibrary.Archives;
using MikuMikuLibrary.Bones;
using MikuMikuLibrary.Extensions;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.IO.Common;
using MikuMikuLibrary.Objects;
using MikuMikuLibrary.Objects.Extra;
using MikuMikuLibrary.Objects.Extra.Blocks;
using MikuMikuLibrary.Objects.Extra.Parameters;
using MikuMikuLibrary.Parameters;
using MikuMikuLibrary.Parameters.Extensions;
using MikuMikuModel.Configurations;
using MikuMikuModel.GUI.Forms;
using MikuMikuModel.Modules;
using MikuMikuModel.Nodes.Collections;
using MikuMikuModel.Nodes.IO;
using OpenTK.Graphics.OpenGL;
using System.Transactions;

namespace MikuMikuModel.Nodes.Objects;

public class SkinNode : Node<Skin>
{
    public override NodeFlags Flags => NodeFlags.Add;

    [Category("General")]
    [DisplayName("Ex data blocks")]
    public List<IBlock> Blocks => GetProperty<List<IBlock>>();

    private Skin PrompImportExData()
    {
        string filePath =
            ModuleImportUtilities.SelectModuleImport(new[]
                { typeof(FarcArchive), typeof(ObjectSet) });

        if (string.IsNullOrEmpty(filePath))
            return null;

        ObjectSet objSet;

        if (filePath.EndsWith(".farc", StringComparison.OrdinalIgnoreCase))
            objSet = BinaryFileNode<ObjectSet>.PromptFarcArchiveViewForm(filePath,
                "Select a file to replace with.",
                "This archive has no object set file.");

        else
            objSet = BinaryFile.Load<ObjectSet>(filePath);

        if (objSet == null)
            return null;

        if (objSet.Objects.Count == 0 || !objSet.Objects.Any(x => x.Skin != null && x.Skin.Blocks.Count > 0))
        {
            MessageBox.Show("This object set has no objects with ex data.", Program.Name, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }

        if (objSet.Objects.Count == 1)
            return objSet.Objects[0].Skin;

        using (var listNode = new ListNode<Object>("Objects", objSet.Objects, x => x.Name))
        using (var nodeSelectForm =
               new NodeSelectForm<Object>(listNode, obj => obj.Skin != null && obj.Skin.Blocks.Count > 0))
        {
            nodeSelectForm.Text = "Please select an object.";

            if (nodeSelectForm.ShowDialog() == DialogResult.OK)
                return ((Object)nodeSelectForm.TopNode.Data).Skin;
        }

        return null;
    }

    protected override void Initialize()
    {
        AddCustomHandler("Import ex data", () =>
        {
            var skin = PrompImportExData();

            if (skin == null)
                return;

            var nodeBlocks = skin.Blocks.OfType<NodeBlock>().ToList();

            using (var itemSelectForm = new ItemSelectForm<NodeBlock>(nodeBlocks.Select(
                       x => (x, $"{x.Signature} - {(x is OsageBlock osageBlock ? osageBlock.ExternalName : x.Name)}")).OrderBy(x => x.Item2))
                   {
                       Text = "Please select the blocks you want to import.",
                       GroupBoxText = "Blocks"
                   })
            {
                if (itemSelectForm.ShowDialog() != DialogResult.OK)
                    return;

                var importedBlocks = new List<NodeBlock>(skin.Blocks.Count);

                foreach (var nodeBlock in itemSelectForm.CheckedItems)
                {
                    importedBlocks.AddRange(nodeBlock.TraverseParents(nodeBlocks));
                    importedBlocks.Add(nodeBlock);
                }

                Data.Blocks.AddRange(importedBlocks.Distinct());
            }

            OnPropertyChanged(nameof(Data.Blocks));
        }, Keys.None, CustomHandlerFlags.ClearMementos | CustomHandlerFlags.Repopulate);

        AddCustomHandler("Replace ex data", () =>
        {
            var skin = PrompImportExData();

            if (skin == null)
                return;

            Data.Blocks.Clear();
            Data.Blocks.AddRange(skin.Blocks);

            OnPropertyChanged(nameof(Data.Blocks));
        }, Keys.None, CustomHandlerFlags.ClearMementos | CustomHandlerFlags.Repopulate);

        AddCustomHandlerSeparator();

        AddCustomHandler("Export Internal Skin Parameter", () =>
        {
            var configuration = ConfigurationList.Instance.CurrentConfiguration;
            using (SaveFileDialog dlg = new SaveFileDialog() { Filter = "Skin Parameter (*.txt)|*.txt" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ParameterTreeWriter externalSkinParam = new ParameterTreeWriter();
                    foreach (var block in Data.Blocks)
                    {
                        if (block is OsageBlock osgBlock)
                        {
                            if (osgBlock.InternalSkinParameter != null)
                            {
                                externalSkinParam.PushScope(osgBlock.ExternalName);
                                {
                                    externalSkinParam.Write("node", osgBlock.Nodes, (OsageNode x) =>
                                    {
                                        externalSkinParam.Write("coli_r", osgBlock.InternalSkinParameter.CollisionRadius);
                                        externalSkinParam.Write("hinge_ymin", -osgBlock.InternalSkinParameter.HingeY);
                                        externalSkinParam.Write("hinge_ymax", osgBlock.InternalSkinParameter.HingeY);
                                        externalSkinParam.Write("hinge_zmin", -osgBlock.InternalSkinParameter.HingeZ);
                                        externalSkinParam.Write("hinge_zmax", osgBlock.InternalSkinParameter.HingeZ);
                                        externalSkinParam.Write("weight", 1.0f);
                                        externalSkinParam.Write("inertial_cancel", 0.0f);
                                    });
                                    externalSkinParam.PushScope("root");
                                    {
                                        externalSkinParam.Write("air_res", osgBlock.InternalSkinParameter.AirResistance);
                                        externalSkinParam.Write("coli", osgBlock.InternalSkinParameter.Collisions, (OsageInternalCollisionParameter x) =>
                                        {
                                            externalSkinParam.Write("type", (int)x.CollisionType);
                                            externalSkinParam.Write("radius", x.CollisionRadius);
                                            externalSkinParam.PushScope("bone");
                                            {
                                                externalSkinParam.PushScope(0);
                                                {
                                                    // try to get the name
                                                    string boneName = configuration?.BoneData.Skeletons[0].ObjectBoneNames[(int)x.Head];
                                                    externalSkinParam.Write("name", boneName);
                                                    externalSkinParam.Write("posx", x.HeadPosition.X);
                                                    externalSkinParam.Write("posy", x.HeadPosition.Y);
                                                    externalSkinParam.Write("posz", x.HeadPosition.Z);
                                                }
                                                externalSkinParam.PopScope();

                                                externalSkinParam.PushScope(1);
                                                {
                                                    // try to get the name
                                                    string boneName = configuration?.BoneData.Skeletons[0].ObjectBoneNames[(int)(x.Tail == 0 ? x.Head : x.Tail)];
                                                    externalSkinParam.Write("name", boneName);
                                                    externalSkinParam.Write("posx", x.TailPosition.X);
                                                    externalSkinParam.Write("posy", x.TailPosition.Y);
                                                    externalSkinParam.Write("posz", x.TailPosition.Z);
                                                }
                                                externalSkinParam.PopScope();
                                            }
                                            externalSkinParam.PopScope();
                                        });
                                        externalSkinParam.Write("coli_type", 0);
                                        externalSkinParam.Write("force", osgBlock.InternalSkinParameter.Force);
                                        externalSkinParam.Write("force_gain", osgBlock.InternalSkinParameter.ForceGain);
                                        externalSkinParam.Write("friction", osgBlock.InternalSkinParameter.Friction);
                                        externalSkinParam.Write("init_rot_y", 0f);
                                        externalSkinParam.Write("init_rot_z", 0f);
                                        externalSkinParam.Write("rot_y", osgBlock.InternalSkinParameter.RotationY);
                                        externalSkinParam.Write("rot_z", osgBlock.InternalSkinParameter.RotationZ);
                                        externalSkinParam.Write("stiffness", 0f);
                                        externalSkinParam.Write("wind_afc", osgBlock.InternalSkinParameter.WindAffection);
                                    }
                                    externalSkinParam.PopScope();
                                }
                                externalSkinParam.PopScope();
                            }
                        }
                        else if (block is ClothBlock clsBlock)
                        {
                            if (clsBlock.InternalSkinParameter != null)
                            {
                                externalSkinParam.PushScope(clsBlock.Name);
                                {
                                    externalSkinParam.PushScope("root");
                                    {
                                        externalSkinParam.Write("air_res", clsBlock.InternalSkinParameter.AirResistance);
                                        externalSkinParam.Write("coli", clsBlock.InternalSkinParameter.Collisions, (OsageInternalCollisionParameter x) =>
                                        {
                                            externalSkinParam.Write("type", (int)x.CollisionType);
                                            externalSkinParam.Write("radius", x.CollisionRadius);
                                            externalSkinParam.PushScope("bone");
                                            {
                                                externalSkinParam.PushScope(0);
                                                {
                                                    // try to get the name
                                                    string boneName = configuration?.BoneData.Skeletons[0].ObjectBoneNames[(int)x.Head];
                                                    externalSkinParam.Write("name", boneName);
                                                    externalSkinParam.Write("posx", x.HeadPosition.X);
                                                    externalSkinParam.Write("posy", x.HeadPosition.Y);
                                                    externalSkinParam.Write("posz", x.HeadPosition.Z);
                                                }
                                                externalSkinParam.PopScope();

                                                externalSkinParam.PushScope(1);
                                                {
                                                    // try to get the name
                                                    string boneName = configuration?.BoneData.Skeletons[0].ObjectBoneNames[(int)x.Tail];
                                                    externalSkinParam.Write("name", boneName);
                                                    externalSkinParam.Write("posx", x.TailPosition.X);
                                                    externalSkinParam.Write("posy", x.TailPosition.Y);
                                                    externalSkinParam.Write("posz", x.TailPosition.Z);
                                                }
                                                externalSkinParam.PopScope();
                                            }
                                            externalSkinParam.PopScope();
                                        });
                                        externalSkinParam.Write("coli_type", 0);
                                        externalSkinParam.Write("force", clsBlock.InternalSkinParameter.Force);
                                        externalSkinParam.Write("force_gain", clsBlock.InternalSkinParameter.ForceGain);
                                        externalSkinParam.Write("friction", clsBlock.InternalSkinParameter.Friction);
                                        externalSkinParam.Write("init_rot_y", 0f);
                                        externalSkinParam.Write("init_rot_z", 0f);
                                        externalSkinParam.Write("rot_y", clsBlock.InternalSkinParameter.RotationY);
                                        externalSkinParam.Write("rot_z", clsBlock.InternalSkinParameter.RotationZ);
                                        externalSkinParam.Write("stiffness", 0f);
                                        externalSkinParam.Write("wind_afc", clsBlock.InternalSkinParameter.WindAffection);
                                    }
                                    externalSkinParam.PopScope();
                                }
                                externalSkinParam.PopScope();
                            }
                        }
                    }
                    externalSkinParam.Save(dlg.FileName);
                }
            }
        }, Keys.None, CustomHandlerFlags.None);

        AddCustomHandler("Create Internal Skin Parameter", () =>
        {
            var configuration = ConfigurationList.Instance.CurrentConfiguration;
            using (OpenFileDialog dlg = new OpenFileDialog() { Filter = "Skin Parameter (*.txt)|*.txt" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ParameterTree externalSkinParam = new ParameterTree(new EndianBinaryReader(File.OpenRead(dlg.FileName), Endianness.Little));
                    foreach (var block in Data.Blocks)
                    {
                        if (block is OsageBlock osgBlock)
                        {
                            if (externalSkinParam.OpenScope(osgBlock.ExternalName))
                            {
                                OsageInternalSkinParameter skp = new OsageInternalSkinParameter();

                                if (externalSkinParam.OpenScope("node"))
                                {
                                    if (externalSkinParam.OpenScope(0))
                                    {
                                        skp.CollisionRadius = externalSkinParam.Get<float>("coli_r");
                                        skp.HingeY = externalSkinParam.Get<float>("hinge_ymax");
                                        skp.HingeZ = externalSkinParam.Get<float>("hinge_zmax");
                                        externalSkinParam.CloseScope();
                                    }
                                    externalSkinParam.CloseScope();
                                }

                                if (externalSkinParam.OpenScope("root"))
                                {
                                    skp.AirResistance = externalSkinParam.Get<float>("air_res");
                                    externalSkinParam.Enumerate("coli", i =>
                                    {
                                        OsageInternalCollisionParameter coll = new OsageInternalCollisionParameter();
                                        coll.CollisionType = (OsageInternalCollisionType)externalSkinParam.Get<int>("type");
                                        coll.CollisionRadius = externalSkinParam.Get<float>("radius");
                                        if (externalSkinParam.OpenScope("bone"))
                                        {
                                            if (externalSkinParam.OpenScope(0))
                                            {
                                                coll.Head = (uint)configuration?.BoneData.Skeletons[0].ObjectBoneNames.FindIndex(x => x == externalSkinParam.Get<string>("name"));
                                                coll.HeadPosition = new Vector3(
                                                    externalSkinParam.Get<float>("posx"),
                                                    externalSkinParam.Get<float>("posy"),
                                                    externalSkinParam.Get<float>("posz"));
                                                externalSkinParam.CloseScope();
                                            }
                                            if (externalSkinParam.OpenScope(1))
                                            {
                                                coll.Tail = (uint)configuration?.BoneData.Skeletons[0].ObjectBoneNames.FindIndex(x => x == externalSkinParam.Get<string>("name"));
                                                coll.TailPosition = new Vector3(
                                                    externalSkinParam.Get<float>("posx"),
                                                    externalSkinParam.Get<float>("posy"),
                                                    externalSkinParam.Get<float>("posz"));
                                                externalSkinParam.CloseScope();
                                            }
                                            externalSkinParam.CloseScope();
                                        }
                                        skp.Collisions.Add(coll);
                                    });
                                    skp.Force = externalSkinParam.Get<float>("force");
                                    skp.ForceGain = externalSkinParam.Get<float>("force_gain");
                                    skp.Friction = externalSkinParam.Get<float>("friction");
                                    skp.Name = osgBlock.ExternalName;
                                    skp.RotationY = externalSkinParam.Get<float>("rot_y");
                                    skp.RotationZ = externalSkinParam.Get<float>("rot_z");
                                    skp.WindAffection = externalSkinParam.Get<float>("wind_afc");
                                    osgBlock.InternalSkinParameter = skp;
                                    externalSkinParam.CloseScope();
                                }
                                externalSkinParam.CloseScope();
                            }
                        }
                        else if (block is ClothBlock clsBlock)
                        {
                            if (externalSkinParam.OpenScope(clsBlock.Name))
                            {
                                OsageInternalSkinParameter skp = new OsageInternalSkinParameter();

                                if (externalSkinParam.OpenScope("root"))
                                {
                                    skp.AirResistance = externalSkinParam.Get<float>("air_res");
                                    externalSkinParam.Enumerate("coli", i =>
                                    {
                                        OsageInternalCollisionParameter coll = new OsageInternalCollisionParameter();
                                        coll.CollisionType = (OsageInternalCollisionType)externalSkinParam.Get<int>("type");
                                        coll.CollisionRadius = externalSkinParam.Get<float>("radius");
                                        if (externalSkinParam.OpenScope("bone"))
                                        {
                                            if (externalSkinParam.OpenScope(0))
                                            {
                                                coll.Head = (uint)configuration?.BoneData.Skeletons[0].ObjectBoneNames.FindIndex(x => x == externalSkinParam.Get<string>("name"));
                                                coll.HeadPosition = new Vector3(
                                                    externalSkinParam.Get<float>("posx"),
                                                    externalSkinParam.Get<float>("posy"),
                                                    externalSkinParam.Get<float>("posz"));
                                                externalSkinParam.CloseScope();
                                            }
                                            if (externalSkinParam.OpenScope(1))
                                            {
                                                coll.Tail = (uint)configuration?.BoneData.Skeletons[0].ObjectBoneNames.FindIndex(x => x == externalSkinParam.Get<string>("name"));
                                                coll.TailPosition = new Vector3(
                                                    externalSkinParam.Get<float>("posx"),
                                                    externalSkinParam.Get<float>("posy"),
                                                    externalSkinParam.Get<float>("posz"));
                                                externalSkinParam.CloseScope();
                                            }
                                            externalSkinParam.CloseScope();
                                        }
                                        skp.Collisions.Add(coll);
                                    });
                                    skp.CollisionRadius = externalSkinParam.Get<float>("coli_r");
                                    skp.Force = externalSkinParam.Get<float>("force");
                                    skp.ForceGain = externalSkinParam.Get<float>("force_gain");
                                    skp.Friction = externalSkinParam.Get<float>("friction");
                                    skp.HingeY = externalSkinParam.Get<float>("hinge_y");
                                    skp.HingeY = externalSkinParam.Get<float>("hinge_z");
                                    skp.Name = clsBlock.Name;
                                    skp.RotationY = externalSkinParam.Get<float>("rot_y");
                                    skp.RotationZ = externalSkinParam.Get<float>("rot_z");
                                    skp.WindAffection = externalSkinParam.Get<float>("wind_afc");
                                    clsBlock.InternalSkinParameter = skp;
                                    externalSkinParam.CloseScope();
                                }
                                externalSkinParam.CloseScope();
                            }
                        }
                    }
                }
            }
        }, Keys.None, CustomHandlerFlags.ClearMementos | CustomHandlerFlags.Repopulate);

        AddCustomHandler("Create base skin parameter", () =>
        {
            List<OsageCollisionParameter> collisionParameters = new List<OsageCollisionParameter>();

            int sumNumVertices = 0;

            foreach (var mesh in FindParent<ObjectNode>().Data.Meshes)
            {
                sumNumVertices += mesh.Positions.Length;
            }

            Vector3[] groupedVertexPositions = new Vector3[sumNumVertices];
            Vector4[] groupedVertexWeights = new Vector4[sumNumVertices];
            MikuMikuLibrary.Numerics.Vector4Int[] remappedVertexIndices = new MikuMikuLibrary.Numerics.Vector4Int[sumNumVertices];

            int baseIndex = 0;


            // this *might* cause some slight issues but...
            foreach (var mesh in FindParent<ObjectNode>().Data.Meshes)
            {
                foreach (var submesh in mesh.SubMeshes)
                {
                    foreach (var index in submesh.Indices)
                    {
                        Console.WriteLine(index);
                        if (submesh.PrimitiveType == MikuMikuLibrary.Objects.PrimitiveType.TriangleStrip ? index != 0xFFFF : true)
                        {
                            groupedVertexPositions[baseIndex + index] = mesh.Positions[index];
                            groupedVertexWeights[baseIndex + index] = mesh.BlendWeights[index];
                            MikuMikuLibrary.Numerics.Vector4Int oldIndices = mesh.BlendIndices[index];
                            remappedVertexIndices[baseIndex + index] = new MikuMikuLibrary.Numerics.Vector4Int(oldIndices.X == -1 ? -1 : submesh.BoneIndices[oldIndices.X], oldIndices.Y == -1 ? -1 : submesh.BoneIndices[oldIndices.Y], oldIndices.Z == -1 ? -1 : submesh.BoneIndices[oldIndices.Z], oldIndices.W == -1 ? -1 : submesh.BoneIndices[oldIndices.W]);
                        }
                    }
                }
                baseIndex += mesh.Positions.Length;
            }

            if (ConfigurationList.Instance?.CurrentConfiguration?.BoneData != null)
            {
                if (ConfigurationList.Instance.CurrentConfiguration.BoneData.Skeletons.Count > 0)
                {
                    Skeleton cmnSkel = ConfigurationList.Instance.CurrentConfiguration.BoneData.Skeletons.First(x => x.Name == "CMN");
                    for (int i = 0; i < Data.Bones.Count; i++)
                    {
                        var bone = Data.Bones[i];
                        
                        if (!bone.Name.StartsWith("nl_") && Data.Bones.Where(x => x.Parent?.Name == bone.Name).Count() == 1)
                        {
                            Matrix4x4.Invert(bone.InverseBindPoseMatrix, out var bindPoseMatrix);

                            Matrix4x4.Decompose(bindPoseMatrix, out var scale, out var rotation, out var translation);
                            rotation = Quaternion.Normalize(rotation);
                            var child = Data.Bones.FirstOrDefault(x => x.Parent?.Name == bone.Name, null);
                            // if this and child bones are in the list of ObjectBones, then allow collision creation.
                            if (child != null)
                            {
                                if (!child.Name.StartsWith("nl_"))
                                {
                                    Matrix4x4.Invert(child.InverseBindPoseMatrix, out var childBindPoseMatrix);

                                    Matrix4x4.Decompose(childBindPoseMatrix, out var childScale, out var childRotation, out var childTranslation);
                                    childRotation = Quaternion.Normalize(childRotation);
                                    if (cmnSkel.ObjectBoneNames.Contains(bone.Name) && cmnSkel.ObjectBoneNames.Contains(child.Name))
                                    {
                                        // MessageBox.Show($"Estimating collision param between bones {bone.Name} and {child.Name}");
                                        OsageCollisionParameter colParam = new OsageCollisionParameter();
                                        colParam.Bone0.Name = bone.Name;
                                        colParam.Bone1.Name = child.Name;

                                        float radiusAppx = 0.0f;
                                        float weightSum = 0.0f;

                                        for (int v = 0; v < sumNumVertices; v++)
                                        {
                                            for (int w = 0; w < 4; w++)
                                            {
                                                if (remappedVertexIndices[v][w] == i)
                                                {
                                                    if (groupedVertexWeights[v][w] >= 0.25)
                                                    {
                                                        for (int o = 0; o < 1; o++)
                                                        {
                                                            Vector3 samplePosition = Vector3.Lerp(translation, childTranslation, 0.5f);
                                                            float vertexDistance = Math.Clamp(Math.Abs(Vector3.Distance(samplePosition, groupedVertexPositions[v])), 0.0f, 0.25f);
                                                            float distanceFalloff = (1.0f / (1.0f + vertexDistance * vertexDistance));
                                                            radiusAppx += Math.Abs(Vector3.Distance(samplePosition, groupedVertexPositions[v])) * groupedVertexWeights[v][w] * distanceFalloff;
                                                            weightSum += groupedVertexWeights[v][w] * distanceFalloff;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        colParam.Radius = radiusAppx / weightSum;
                                        // MessageBox.Show($"Estimated radius surrounding bone {bone.Name} was {colParam.Radius}");

                                        colParam.Type = 2;

                                        collisionParameters.Add(colParam);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            OsageSkinParameterSet skp = new OsageSkinParameterSet();
            foreach (var block in Data.Blocks)
            {
                if (block is OsageBlock osgBlock)
                {
                    OsageSkinParameter param = new OsageSkinParameter();
                    param.Name = osgBlock.ExternalName;
                    foreach (var node in osgBlock.Nodes)
                    {
                        OsageNodeParameter nodeParam = new OsageNodeParameter();
                        nodeParam.Radius = 0.082000f;
                        nodeParam.HingeYMin = -179.000000f;
                        nodeParam.HingeYMax = 179.000000f;
                        nodeParam.HingeZMin = -179.000000f;
                        nodeParam.HingeZMax = 179.000000f;
                        nodeParam.Weight = 1.000000f;

                        BoneInfo nodeBoneInfo = Data.Bones.FirstOrDefault(x => x?.Name == node.Name, null);

                        if (nodeBoneInfo != null)
                        {
                            Matrix4x4.Invert(nodeBoneInfo.InverseBindPoseMatrix, out var nodeBindPoseMatrix);

                            Matrix4x4.Decompose(nodeBindPoseMatrix, out var nodeScale, out var nodeRotation, out var nodeTranslation);
                            nodeRotation = Quaternion.Normalize(nodeRotation);

                            BoneInfo nodeChildBoneInfo = Data.Bones.FirstOrDefault(x => x.Parent?.Name == nodeBoneInfo.Name);


                            if (nodeChildBoneInfo != null)
                            {
                                Matrix4x4.Invert(nodeChildBoneInfo.InverseBindPoseMatrix, out var nodeChildBindPoseMatrix);

                                Matrix4x4.Decompose(nodeChildBindPoseMatrix, out var nodeChildScale, out var nodeChildRotation, out var nodeChildTranslation);
                                nodeChildRotation = Quaternion.Normalize(nodeChildRotation);

                                int nodeBoneIndex = Data.Bones.IndexOf(nodeBoneInfo);

                                float radiusAppx = 0.0f;
                                float weightSum = 0.0f;

                                for (int v = 0; v < sumNumVertices; v++)
                                {
                                    for (int w = 0; w < 4; w++)
                                    {
                                        if (remappedVertexIndices[v][w] == nodeBoneIndex)
                                        {
                                            if (groupedVertexWeights[v][w] >= 0.25)
                                            {
                                                for (int o = 0; o < 1; o++)
                                                {
                                                    Vector3 samplePosition = Vector3.Lerp(nodeTranslation, nodeChildTranslation, 0.5f);
                                                    float vertexDistance = Math.Clamp(Math.Abs(Vector3.Distance(samplePosition, groupedVertexPositions[v])), 0.0f, node.Length / 2);
                                                    float distanceFalloff = (1.0f / (1.0f + vertexDistance * vertexDistance));
                                                    radiusAppx += Math.Abs(Vector3.Distance(samplePosition, groupedVertexPositions[v])) * groupedVertexWeights[v][w] * distanceFalloff;
                                                    weightSum += groupedVertexWeights[v][w] * distanceFalloff;
                                                }
                                            }
                                        }
                                    }
                                }

                                nodeParam.Radius = radiusAppx / weightSum;
                            }
                            else
                            {
                                Vector3 nodeEnd = nodeTranslation + Vector3.Transform(Vector3.UnitX * node.Length, nodeRotation);

                                int nodeBoneIndex = Data.Bones.IndexOf(nodeBoneInfo);

                                float radiusAppx = 0.0f;
                                float weightSum = 0.0f;

                                for (int v = 0; v < sumNumVertices; v++)
                                {
                                    for (int w = 0; w < 4; w++)
                                    {
                                        if (remappedVertexIndices[v][w] == nodeBoneIndex)
                                        {
                                            if (groupedVertexWeights[v][w] >= 0.25)
                                            {
                                                for (int o = 0; o < 1; o++)
                                                {
                                                    Vector3 samplePosition = Vector3.Lerp(nodeTranslation, nodeEnd, 0.5f);
                                                    float vertexDistance = Math.Clamp(Math.Abs(Vector3.Distance(samplePosition, groupedVertexPositions[v])), 0.0f, node.Length / 2);
                                                    float distanceFalloff = (1.0f / (1.0f + vertexDistance * vertexDistance));
                                                    radiusAppx += Math.Abs(Vector3.Distance(samplePosition, groupedVertexPositions[v])) * groupedVertexWeights[v][w] * distanceFalloff;
                                                    weightSum += groupedVertexWeights[v][w] * distanceFalloff;
                                                }
                                            }
                                        }
                                    }
                                }

                                nodeParam.Radius = radiusAppx / weightSum;
                            }

                            var collisionCandidates = collisionParameters.Select(colParam =>
                            {
                                BoneInfo colHeadBoneInfo = Data.Bones.FirstOrDefault(x => x.Name == colParam.Bone0.Name, null);
                                BoneInfo colTailBoneInfo = Data.Bones.FirstOrDefault(x => x.Name == colParam.Bone1.Name, null);

                                Matrix4x4.Invert(colHeadBoneInfo.InverseBindPoseMatrix, out var colHeadBindPoseMatrix);

                                Matrix4x4.Decompose(colHeadBindPoseMatrix, out var colHeadScale, out var colHeadRotation, out var colHeadTranslation);
                                colHeadRotation = Quaternion.Normalize(colHeadRotation);

                                Matrix4x4.Invert(colTailBoneInfo.InverseBindPoseMatrix, out var colTailBindPoseMatrix);

                                Matrix4x4.Decompose(colTailBindPoseMatrix, out var colTailScale, out var colTailRotation, out var colTailTranslation);
                                colTailRotation = Quaternion.Normalize(colTailRotation);

                                float headDistance = Math.Abs(Vector3.Distance(nodeTranslation, colHeadTranslation));
                                float tailDistance = Math.Abs(Vector3.Distance(nodeTranslation, colTailTranslation));

                                float avgDistance = 0.0f;
                                for (int i = 0; i < 1; i++)
                                {
                                    Vector3 samplePosition = Vector3.Lerp(colHeadTranslation, colTailTranslation, 0.5f);
                                    avgDistance += Math.Abs(Vector3.Distance(nodeTranslation, samplePosition));
                                }


                                return new { Parameter = colParam, HeadDistance = avgDistance / 10.0f };
                            }).OrderBy(x => x.HeadDistance).Take(11);

                            foreach (var candidate in collisionCandidates)
                            {
                                if (!param.Collisions.Any(x => x.Bone0.Name == candidate.Parameter.Bone0.Name || x.Bone1.Name == candidate.Parameter.Bone1.Name) && param.Collisions.Count < 11)
                                {
                                    param.Collisions.Add(candidate.Parameter);
                                }
                            }
                        }
                        else
                        {
                            nodeParam.Radius = param.Nodes.Last().Radius;
                        }
                        param.Nodes.Add(nodeParam);
                    }

                    param.CollisionType = (int)OsageInternalCollisionType.Cylinder;

                    param.AirResistance = 0.600000f;
                    param.Force = 0.050000f;
                    param.ForceGain = 0.600000f;
                    param.Friction = 1.000000f;
                    param.InitRotationY = 0.000000f;
                    param.InitRotationZ = 0.000000f;
                    param.MoveCancel = 0.000000f;
                    param.RotationY = 0.000000f;
                    param.RotationZ = 0.000000f;
                    param.Stiffness = 0.000000f;
                    param.WindAffection = 0.000000f;

                    skp.Parameters.Add(param);

                }
            }

            using (SaveFileDialog dlg = new SaveFileDialog() { Filter = "Skin Parameter Classic (*.txt)|*.txt|Skin Parameter Modern (*.osp)|*.osp" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    skp.Save(dlg.FileName);
                }
            }
        });

        AddCustomHandlerSeparator();

        AddCustomHandler("Import ex data from JSON", () =>
        {
            //var skin = PrompImportExData();
            // Testing imports from Json
            // So this should just simply be...
            //taking this straight from keikei's part for testing. thanks oomf
            string jsonFilePath = null;
            // open json
            using (var jsonFileDialog = new OpenFileDialog()
                   {
                       Title = "Select NodeBlock JSON file.",
                       Filter = "JSON files (*.json)|*.json|All files(*.*)|*.*",
                       FilterIndex = 0,
                       RestoreDirectory = true,
                   })
            {
                if (jsonFileDialog.ShowDialog() == DialogResult.OK)
                    jsonFilePath = jsonFileDialog.FileName;
            }

            try
            {
                var placeholderImportedBlocks = File.ReadAllText(jsonFilePath);
                List<NodeBlock> skin = JsonSerializer.Deserialize<List<NodeBlock>>(
                    placeholderImportedBlocks, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });

                if (skin == null)
                    return;
                var nodeBlocks = skin;

                using (var itemSelectForm = new ItemSelectForm<NodeBlock>(nodeBlocks.Select(
                               x => (x,
                                   $"{x.Signature} - {(x is OsageBlock osageBlock ? osageBlock.ExternalName : x.Name)}"))
                           .OrderBy(x => x.Item2))
                       {
                           Text = "Please select the blocks you want to import.",
                           GroupBoxText = "Blocks"
                       })
                {
                    if (itemSelectForm.ShowDialog() != DialogResult.OK)
                        return;

                    var importedBlocks = skin;

                    foreach (var nodeBlock in itemSelectForm.CheckedItems)
                    {
                        importedBlocks.AddRange(nodeBlock.TraverseParents(nodeBlocks));
                        importedBlocks.Add(nodeBlock);
                    }

                    Data.Blocks.AddRange(importedBlocks.Distinct());
                }
            }
            catch (System.ArgumentNullException)
            {
                return;
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message, Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OnPropertyChanged(nameof(Data.Blocks));
        }, Keys.None, CustomHandlerFlags.ClearMementos | CustomHandlerFlags.Repopulate);

        AddCustomHandler("Export ex data to JSON", () =>
        {
            var skin = PrompImportExData();

            if (skin == null)
                return;

            var nodeBlocks = skin.Blocks.OfType<NodeBlock>().ToList();

            using (var itemSelectForm = new ItemSelectForm<NodeBlock>(nodeBlocks.Select(
                       x => (x, $"{x.Signature} - {(x is OsageBlock osageBlock ? osageBlock.ExternalName : x.Name)}")).OrderBy(x => x.Item2))
                   {
                       Text = "Please select the blocks you want to export.",
                       GroupBoxText = "Blocks"
                   })
            {
                if (itemSelectForm.ShowDialog() != DialogResult.OK)
                    return;

                var importedBlocks = new List<NodeBlock>(skin.Blocks.Count);

                foreach (var nodeBlock in itemSelectForm.CheckedItems)
                {
                    importedBlocks.AddRange(nodeBlock.TraverseParents(nodeBlocks));
                    importedBlocks.Add(nodeBlock);
                }

                // Borrowing the Module Export Utilities to try this...
                // And this works!
                var filePath = ModuleExportUtilities.SelectModuleExport<Stream>();
                File.WriteAllText(filePath, JsonSerializer.Serialize(importedBlocks.Distinct(), new JsonSerializerOptions{ WriteIndented = true }));
            }

        }, Keys.None, CustomHandlerFlags.ClearMementos | CustomHandlerFlags.Repopulate);

        AddCustomHandler("Replace ex data from JSON", () =>
        {
            //taking this straight from keikei's part for testing. thanks oomf
            string jsonFilePath = null;
            // open json
            using (var jsonFileDialog = new OpenFileDialog()
                   {
                       Title = "Select NodeBlock json file.",
                       Filter = "JSON files (*.json)|*.json|All files(*.*)|*.*",
                       FilterIndex = 0,
                       RestoreDirectory = true,
                   })
            {
                if (jsonFileDialog.ShowDialog() == DialogResult.OK)
                    jsonFilePath = jsonFileDialog.FileName;
            }

            try
            {
                var placeholderImportedBlocks = File.ReadAllText(jsonFilePath);
                List<NodeBlock> importedBlocks = JsonSerializer.Deserialize<List<NodeBlock>>(
                    placeholderImportedBlocks, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                Data.Blocks.Clear();
                Data.Blocks.AddRange(importedBlocks.Distinct());
            }
            catch (System.ArgumentNullException)
            {
                return;
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message, Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OnPropertyChanged(nameof(Data.Blocks));
        }, Keys.None, CustomHandlerFlags.ClearMementos | CustomHandlerFlags.Repopulate);

    }

    protected override void PopulateCore()
    {
        Nodes.Add(new ListNode<BoneInfo>("Bones", Data.Bones, x => x.Name));
    }

    protected override void SynchronizeCore()
    {
    }

    public SkinNode(string name, Skin data) : base(name, data)
    {
    }
}