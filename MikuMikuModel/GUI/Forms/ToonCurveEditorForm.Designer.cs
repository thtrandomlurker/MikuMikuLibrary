namespace MikuMikuModel.GUI.Forms
{
    partial class ToonCurveEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DiffuseCurvePoints = new ListBox();
            CurveOffset = new TrackBar();
            ToonCurvePreview = new PictureBox();
            SpecularCurvePoints = new ListBox();
            FresnelCurvePoints = new ListBox();
            AddPoint = new Button();
            ActiveCurveName = new Label();
            RemovePoint = new Button();
            SetPointColor = new Button();
            ((ISupportInitialize)CurveOffset).BeginInit();
            ((ISupportInitialize)ToonCurvePreview).BeginInit();
            SuspendLayout();
            // 
            // DiffuseCurvePoints
            // 
            DiffuseCurvePoints.FormattingEnabled = true;
            DiffuseCurvePoints.ItemHeight = 15;
            DiffuseCurvePoints.Location = new Point(12, 12);
            DiffuseCurvePoints.Name = "DiffuseCurvePoints";
            DiffuseCurvePoints.Size = new Size(114, 319);
            DiffuseCurvePoints.TabIndex = 0;
            DiffuseCurvePoints.SelectedIndexChanged += DiffuseCurvePoints_SelectedIndexChanged;
            DiffuseCurvePoints.MouseDown += DiffuseCurvePoints_MouseDown;
            // 
            // CurveOffset
            // 
            CurveOffset.Location = new Point(12, 337);
            CurveOffset.Maximum = 255;
            CurveOffset.Name = "CurveOffset";
            CurveOffset.Size = new Size(512, 45);
            CurveOffset.TabIndex = 1;
            CurveOffset.TickStyle = TickStyle.None;
            CurveOffset.ValueChanged += CurveOffset_ValueChanged;
            // 
            // ToonCurvePreview
            // 
            ToonCurvePreview.Location = new Point(12, 366);
            ToonCurvePreview.Name = "ToonCurvePreview";
            ToonCurvePreview.Size = new Size(512, 16);
            ToonCurvePreview.SizeMode = PictureBoxSizeMode.StretchImage;
            ToonCurvePreview.TabIndex = 2;
            ToonCurvePreview.TabStop = false;
            // 
            // SpecularCurvePoints
            // 
            SpecularCurvePoints.FormattingEnabled = true;
            SpecularCurvePoints.ItemHeight = 15;
            SpecularCurvePoints.Location = new Point(132, 12);
            SpecularCurvePoints.Name = "SpecularCurvePoints";
            SpecularCurvePoints.Size = new Size(114, 319);
            SpecularCurvePoints.TabIndex = 0;
            SpecularCurvePoints.SelectedIndexChanged += SpecularCurvePoints_SelectedIndexChanged;
            SpecularCurvePoints.MouseDown += SpecularCurvePoints_MouseDown;
            // 
            // FresnelCurvePoints
            // 
            FresnelCurvePoints.FormattingEnabled = true;
            FresnelCurvePoints.ItemHeight = 15;
            FresnelCurvePoints.Location = new Point(252, 12);
            FresnelCurvePoints.Name = "FresnelCurvePoints";
            FresnelCurvePoints.Size = new Size(114, 319);
            FresnelCurvePoints.TabIndex = 0;
            FresnelCurvePoints.SelectedIndexChanged += FresnelCurvePoints_SelectedIndexChanged;
            FresnelCurvePoints.MouseDown += FresnelCurvePoints_MouseDown;
            // 
            // AddPoint
            // 
            AddPoint.Location = new Point(372, 30);
            AddPoint.Name = "AddPoint";
            AddPoint.Size = new Size(152, 23);
            AddPoint.TabIndex = 3;
            AddPoint.Text = "Add Point";
            AddPoint.UseVisualStyleBackColor = true;
            AddPoint.Click += AddPoint_Click;
            // 
            // ActiveCurveName
            // 
            ActiveCurveName.AutoSize = true;
            ActiveCurveName.Location = new Point(372, 12);
            ActiveCurveName.Name = "ActiveCurveName";
            ActiveCurveName.Size = new Size(74, 15);
            ActiveCurveName.TabIndex = 4;
            ActiveCurveName.Text = "Active Curve";
            // 
            // RemovePoint
            // 
            RemovePoint.Location = new Point(372, 59);
            RemovePoint.Name = "RemovePoint";
            RemovePoint.Size = new Size(152, 23);
            RemovePoint.TabIndex = 5;
            RemovePoint.Text = "Remove Point";
            RemovePoint.UseVisualStyleBackColor = true;
            RemovePoint.Click += RemovePoint_Click;
            // 
            // SetPointColor
            // 
            SetPointColor.Location = new Point(372, 88);
            SetPointColor.Name = "SetPointColor";
            SetPointColor.Size = new Size(152, 23);
            SetPointColor.TabIndex = 6;
            SetPointColor.Text = "Set Color";
            SetPointColor.UseVisualStyleBackColor = true;
            SetPointColor.Click += SetPointColor_Click;
            // 
            // ToonCurveEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(536, 420);
            Controls.Add(SetPointColor);
            Controls.Add(RemovePoint);
            Controls.Add(ActiveCurveName);
            Controls.Add(AddPoint);
            Controls.Add(ToonCurvePreview);
            Controls.Add(CurveOffset);
            Controls.Add(FresnelCurvePoints);
            Controls.Add(SpecularCurvePoints);
            Controls.Add(DiffuseCurvePoints);
            Name = "ToonCurveEditorForm";
            Text = "ToonCurveEditorForm";
            ((ISupportInitialize)CurveOffset).EndInit();
            ((ISupportInitialize)ToonCurvePreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox DiffuseCurvePoints;
        private TrackBar CurveOffset;
        private PictureBox ToonCurvePreview;
        private ListBox SpecularCurvePoints;
        private ListBox FresnelCurvePoints;
        private Button AddPoint;
        private Label ActiveCurveName;
        private Button RemovePoint;
        private Button SetPointColor;
    }
}