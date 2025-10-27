using MikuMikuLibrary.Textures;
using MikuMikuLibrary.Textures.Processing;
using MikuMikuModel.GUI.Controls;

namespace MikuMikuModel.GUI.Forms
{
    public partial class ToonCurveEditorForm : Form
    {
        ToonCurveGeneratorTexture mTexture;
        Object mObject;
        TextureSet mTextureSet;
        ListBox mActiveListBox;
        List<ColorPoint> mActiveCurveList;
        CurveType mActiveCurveType;
        public ToonCurveEditorForm(ToonCurveGeneratorTexture tex, Object obj, TextureSet texSet)
        {
            InitializeComponent();
            mTexture = tex;
            mObject = obj;
            mTextureSet = texSet;
            foreach (var point in tex.DiffuseCurvePoints)
            {
                DiffuseCurvePoints.Items.Add($"{point.Color}");
            }
            foreach (var point in tex.SpecularCurvePoints)
            {
                SpecularCurvePoints.Items.Add($"{point.Color}");
            }
            foreach (var point in tex.FresnelCurvePoints)
            {
                FresnelCurvePoints.Items.Add($"{point.Color}");
            }
            mTexture.UpdateCurve(CurveType.Diffuse);
            mTexture.UpdateCurve(CurveType.Specular);
            mTexture.UpdateCurve(CurveType.Fresnel);
            mActiveListBox = DiffuseCurvePoints;
            mActiveCurveList = mTexture.DiffuseCurvePoints;
            mActiveCurveType = CurveType.Diffuse;
            mActiveCurveList[0].Active = true;
            mActiveListBox.SelectedIndex = 0;

            ActiveCurveName.Text = "Active Curve: Diffuse";

            ToonCurvePreview.Image = TextureDecoder.DecodeToBitmap(mTexture);
        }

        private void DiffuseCurvePoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mActiveListBox.SelectedIndex != -1)
            {
                mActiveCurveList.First(x => x.Active).Active = false;
                mActiveCurveList[mActiveListBox.SelectedIndex].Active = true;
                CurveOffset.Value = mActiveCurveList[mActiveListBox.SelectedIndex].Offset;
            }
        }

        private void CurveOffset_ValueChanged(object sender, EventArgs e)
        {
            mActiveCurveList[mActiveListBox.SelectedIndex].Offset = CurveOffset.Value;
            mTexture.UpdateCurve(mActiveCurveType);

            mActiveListBox.Items.Clear();
            foreach (var point in mActiveCurveList)
            {
                mActiveListBox.Items.Add($"{point.Color}");
            }
            mActiveListBox.SelectedIndex = mActiveCurveList.FindIndex(x => x.Active);

            ToonCurvePreview.Image = TextureDecoder.DecodeToBitmap(mTexture);

            ModelViewControl.Instance.SetModel(mObject, mTextureSet, true);
            ModelViewControl.Instance.Invalidate();
        }

        private void SpecularCurvePoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mActiveListBox.SelectedIndex != -1)
            {
                mActiveCurveList.First(x => x.Active).Active = false;
                mActiveCurveList[mActiveListBox.SelectedIndex].Active = true;
                CurveOffset.Value = mActiveCurveList[mActiveListBox.SelectedIndex].Offset;
            }
        }

        private void FresnelCurvePoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mActiveListBox.SelectedIndex != -1)
            {
                mActiveCurveList.First(x => x.Active).Active = false;
                mActiveCurveList[mActiveListBox.SelectedIndex].Active = true;
                CurveOffset.Value = mActiveCurveList[mActiveListBox.SelectedIndex].Offset;
            }
        }

        private void DiffuseCurvePoints_MouseDown(object sender, MouseEventArgs e)
        {
            if (mActiveCurveList != mTexture.DiffuseCurvePoints)
            {
                mActiveCurveList.First(x => x.Active).Active = false;
                mActiveListBox = DiffuseCurvePoints;
                mActiveCurveList = mTexture.DiffuseCurvePoints;
                mActiveCurveType = CurveType.Diffuse;
                if (mActiveListBox.SelectedIndex == -1)
                {
                    mActiveCurveList[0].Active = true;
                    mActiveListBox.SelectedIndex = 0;
                }
                else
                {
                    mActiveCurveList[mActiveListBox.SelectedIndex].Active = true;
                }

                ActiveCurveName.Text = "Active Curve: Diffuse";
            }
        }

        private void SpecularCurvePoints_MouseDown(object sender, MouseEventArgs e)
        {
            if (mActiveCurveList != mTexture.SpecularCurvePoints)
            {
                mActiveCurveList.First(x => x.Active).Active = false;
                mActiveListBox = SpecularCurvePoints;
                mActiveCurveList = mTexture.SpecularCurvePoints;
                mActiveCurveType = CurveType.Specular;
                if (mActiveListBox.SelectedIndex == -1)
                {
                    mActiveCurveList[0].Active = true;
                    mActiveListBox.SelectedIndex = 0;
                }
                else
                {
                    mActiveCurveList[mActiveListBox.SelectedIndex].Active = true;
                }

                ActiveCurveName.Text = "Active Curve: Specular";
            }
        }

        private void FresnelCurvePoints_MouseDown(object sender, MouseEventArgs e)
        {
            if (mActiveCurveList != mTexture.FresnelCurvePoints)
            {
                mActiveCurveList.First(x => x.Active).Active = false;
                mActiveListBox = FresnelCurvePoints;
                mActiveCurveList = mTexture.FresnelCurvePoints;
                mActiveCurveType = CurveType.Fresnel;
                if (mActiveListBox.SelectedIndex == -1)
                {
                    mActiveCurveList[0].Active = true;
                    mActiveListBox.SelectedIndex = 0;
                }
                else
                {
                    mActiveCurveList[mActiveListBox.SelectedIndex].Active = true;
                }

                ActiveCurveName.Text = "Active Curve: Fresnel";
            }
        }

        private void AddPoint_Click(object sender, EventArgs e)
        {
            int midPoint = 0;
            Vector3 midColor = new Vector3(0.0f, 0.0f, 0.0f);
            foreach (var point in mActiveCurveList)
            {
                midPoint += point.Offset;
                midColor += point.Color;
            }
            midPoint /= mActiveCurveList.Count;
            midColor /= mActiveCurveList.Count;
            mActiveCurveList.First(x => x.Active).Active = false;
            ColorPoint p = new ColorPoint() { Offset = midPoint, Color = midColor, Active = true };
            mActiveCurveList.Add(p);
            mActiveListBox.Items.Add($"{p.Color}");
            mActiveListBox.SelectedIndex = mActiveListBox.Items.Count - 1;
        }

        private void RemovePoint_Click(object sender, EventArgs e)
        {
            if (mActiveCurveList.Count > 2 && mActiveListBox.SelectedIndex != -1)
            {
                int activeIndex = mActiveListBox.SelectedIndex;
                int newIndex = activeIndex > 0 ? activeIndex - 1 : 0;
                mActiveCurveList[newIndex].Active = true;
                mActiveCurveList.RemoveAt(activeIndex);
                mActiveListBox.Items.RemoveAt(activeIndex);
                mActiveListBox.SelectedIndex = newIndex;
            }
        }

        private void SetPointColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    var point = mActiveCurveList[mActiveListBox.SelectedIndex];
                    point.Color = new Vector3(colorDialog.Color.R / 255.0f, colorDialog.Color.G / 255.0f, colorDialog.Color.B / 255.0f);
                    mTexture.UpdateCurve(mActiveCurveType);
                    mActiveListBox.Items[mActiveListBox.SelectedIndex] = $"{point.Color}";
                    ToonCurvePreview.Image = TextureDecoder.DecodeToBitmap(mTexture);
                    ModelViewControl.Instance.SetModel(mObject, mTextureSet, true);
                    ModelViewControl.Instance.Invalidate();
                }
            }
        }
    }
}
