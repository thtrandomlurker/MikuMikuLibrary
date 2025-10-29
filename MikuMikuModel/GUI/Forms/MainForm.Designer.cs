namespace MikuMikuModel.GUI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer mComponents = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            mMainSplitContainer = new SplitContainer();
            mRightSplitContainer = new SplitContainer();
            mNodeTreeView = new MikuMikuModel.Nodes.Wrappers.NodeTreeView();
            mPropertyGrid = new PropertyGrid();
            mMenuStrip = new MenuStrip();
            mFileToolStripMenuItem = new ToolStripMenuItem();
            mOpenToolStripMenuItem = new ToolStripMenuItem();
            mOpenRecentToolStripMenuItem = new ToolStripMenuItem();
            mSaveToolStripMenuItem = new ToolStripMenuItem();
            mSaveAsToolStripMenuItem = new ToolStripMenuItem();
            mCloseToolStripMenuItem = new ToolStripMenuItem();
            mToolStripSeparator2 = new ToolStripSeparator();
            mExitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            mUndoToolStripMenuItem = new ToolStripMenuItem();
            mRedoToolStripMenuItem = new ToolStripMenuItem();
            mConfigurationsToolStripMenuItem = new ToolStripMenuItem();
            mToolsToolStripMenuItem = new ToolStripMenuItem();
            mCombineMotsFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            mGenerateMurmurHashesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            mConvertOsageSkinParametersToToolStripMenuItem = new ToolStripMenuItem();
            mConvertOspToClassicToolStripMenuItem = new ToolStripMenuItem();
            mConvertOspToF2ndToolStripMenuItem = new ToolStripMenuItem();
            mConvertOspToXToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            mConvertObjectSetsToToolStripMenuItem = new ToolStripMenuItem();
            mConvertObjectSetsToTrianglesToolStripMenuItem = new ToolStripMenuItem();
            mConvertObjectSetsToTriangleStripToolStripMenuItem = new ToolStripMenuItem();
            mOptionsToolStripMenuItem = new ToolStripMenuItem();
            mAutoCheckUpdatesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            mStylesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            mCamerasToolStripMenuItem = new ToolStripMenuItem();
            mOrbitCameraToolStripMenuItem = new ToolStripMenuItem();
            mFreeCameraToolStripMenuItem = new ToolStripMenuItem();
            mHelpToolStripMenuItem = new ToolStripMenuItem();
            mUserGuideToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            mCheckForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            mAboutToolStripMenuItem = new ToolStripMenuItem();
            mPanel = new Panel();
            ((ISupportInitialize)mMainSplitContainer).BeginInit();
            mMainSplitContainer.Panel2.SuspendLayout();
            mMainSplitContainer.SuspendLayout();
            ((ISupportInitialize)mRightSplitContainer).BeginInit();
            mRightSplitContainer.Panel1.SuspendLayout();
            mRightSplitContainer.Panel2.SuspendLayout();
            mRightSplitContainer.SuspendLayout();
            mMenuStrip.SuspendLayout();
            mPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mMainSplitContainer
            // 
            mMainSplitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mMainSplitContainer.Location = new Point(14, 36);
            mMainSplitContainer.Margin = new Padding(4, 3, 4, 3);
            mMainSplitContainer.Name = "mMainSplitContainer";
            // 
            // mMainSplitContainer.Panel2
            // 
            mMainSplitContainer.Panel2.Controls.Add(mRightSplitContainer);
            mMainSplitContainer.Size = new Size(831, 458);
            mMainSplitContainer.SplitterDistance = 522;
            mMainSplitContainer.SplitterWidth = 5;
            mMainSplitContainer.TabIndex = 0;
            // 
            // mRightSplitContainer
            // 
            mRightSplitContainer.Dock = DockStyle.Fill;
            mRightSplitContainer.Location = new Point(0, 0);
            mRightSplitContainer.Margin = new Padding(4, 3, 4, 3);
            mRightSplitContainer.Name = "mRightSplitContainer";
            mRightSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // mRightSplitContainer.Panel1
            // 
            mRightSplitContainer.Panel1.Controls.Add(mNodeTreeView);
            // 
            // mRightSplitContainer.Panel2
            // 
            mRightSplitContainer.Panel2.Controls.Add(mPropertyGrid);
            mRightSplitContainer.Size = new Size(304, 458);
            mRightSplitContainer.SplitterDistance = 207;
            mRightSplitContainer.SplitterWidth = 5;
            mRightSplitContainer.TabIndex = 0;
            // 
            // mNodeTreeView
            // 
            mNodeTreeView.Dock = DockStyle.Fill;
            mNodeTreeView.HideSelection = false;
            mNodeTreeView.ImageIndex = 0;
            mNodeTreeView.Location = new Point(0, 0);
            mNodeTreeView.Margin = new Padding(4, 3, 4, 3);
            mNodeTreeView.Name = "mNodeTreeView";
            mNodeTreeView.SelectedImageIndex = 0;
            mNodeTreeView.SelectedNode = null;
            mNodeTreeView.Size = new Size(304, 207);
            mNodeTreeView.TabIndex = 0;
            mNodeTreeView.AfterSelect += OnAfterSelect;
            // 
            // mPropertyGrid
            // 
            mPropertyGrid.Dock = DockStyle.Fill;
            mPropertyGrid.HelpVisible = false;
            mPropertyGrid.Location = new Point(0, 0);
            mPropertyGrid.Margin = new Padding(4, 3, 4, 3);
            mPropertyGrid.Name = "mPropertyGrid";
            mPropertyGrid.PropertySort = PropertySort.Categorized;
            mPropertyGrid.Size = new Size(304, 246);
            mPropertyGrid.TabIndex = 0;
            mPropertyGrid.ToolbarVisible = false;
            mPropertyGrid.PropertyValueChanged += OnPropertyValueChanged;
            // 
            // mMenuStrip
            // 
            mMenuStrip.Dock = DockStyle.Fill;
            mMenuStrip.Items.AddRange(new ToolStripItem[] { mFileToolStripMenuItem, editToolStripMenuItem, mConfigurationsToolStripMenuItem, mToolsToolStripMenuItem, mOptionsToolStripMenuItem, mHelpToolStripMenuItem });
            mMenuStrip.Location = new Point(0, 0);
            mMenuStrip.Name = "mMenuStrip";
            mMenuStrip.Padding = new Padding(7, 2, 0, 2);
            mMenuStrip.Size = new Size(859, 29);
            mMenuStrip.TabIndex = 0;
            mMenuStrip.Text = "menuStrip1";
            // 
            // mFileToolStripMenuItem
            // 
            mFileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mOpenToolStripMenuItem, mOpenRecentToolStripMenuItem, mSaveToolStripMenuItem, mSaveAsToolStripMenuItem, mCloseToolStripMenuItem, mToolStripSeparator2, mExitToolStripMenuItem });
            mFileToolStripMenuItem.Name = "mFileToolStripMenuItem";
            mFileToolStripMenuItem.Size = new Size(37, 25);
            mFileToolStripMenuItem.Text = "File";
            mFileToolStripMenuItem.DropDownOpening += OnFileDropDownOpening;
            // 
            // mOpenToolStripMenuItem
            // 
            mOpenToolStripMenuItem.Name = "mOpenToolStripMenuItem";
            mOpenToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            mOpenToolStripMenuItem.Size = new Size(186, 22);
            mOpenToolStripMenuItem.Text = "Open";
            mOpenToolStripMenuItem.Click += OnOpen;
            // 
            // mOpenRecentToolStripMenuItem
            // 
            mOpenRecentToolStripMenuItem.Name = "mOpenRecentToolStripMenuItem";
            mOpenRecentToolStripMenuItem.Size = new Size(186, 22);
            mOpenRecentToolStripMenuItem.Text = "Open Recent";
            // 
            // mSaveToolStripMenuItem
            // 
            mSaveToolStripMenuItem.Enabled = false;
            mSaveToolStripMenuItem.Name = "mSaveToolStripMenuItem";
            mSaveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            mSaveToolStripMenuItem.Size = new Size(186, 22);
            mSaveToolStripMenuItem.Text = "Save";
            mSaveToolStripMenuItem.Click += OnSave;
            // 
            // mSaveAsToolStripMenuItem
            // 
            mSaveAsToolStripMenuItem.Enabled = false;
            mSaveAsToolStripMenuItem.Name = "mSaveAsToolStripMenuItem";
            mSaveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            mSaveAsToolStripMenuItem.Size = new Size(186, 22);
            mSaveAsToolStripMenuItem.Text = "Save As";
            mSaveAsToolStripMenuItem.Click += OnSaveAs;
            // 
            // mCloseToolStripMenuItem
            // 
            mCloseToolStripMenuItem.Enabled = false;
            mCloseToolStripMenuItem.Name = "mCloseToolStripMenuItem";
            mCloseToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            mCloseToolStripMenuItem.Size = new Size(186, 22);
            mCloseToolStripMenuItem.Text = "Close";
            mCloseToolStripMenuItem.Click += OnNodeClose;
            // 
            // mToolStripSeparator2
            // 
            mToolStripSeparator2.Name = "mToolStripSeparator2";
            mToolStripSeparator2.Size = new Size(183, 6);
            // 
            // mExitToolStripMenuItem
            // 
            mExitToolStripMenuItem.Name = "mExitToolStripMenuItem";
            mExitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            mExitToolStripMenuItem.Size = new Size(186, 22);
            mExitToolStripMenuItem.Text = "Exit";
            mExitToolStripMenuItem.Click += OnExit;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mUndoToolStripMenuItem, mRedoToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 25);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.DropDownClosed += OnEditDropDownClosed;
            editToolStripMenuItem.DropDownOpening += OnEditDropDownOpening;
            // 
            // mUndoToolStripMenuItem
            // 
            mUndoToolStripMenuItem.Name = "mUndoToolStripMenuItem";
            mUndoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            mUndoToolStripMenuItem.Size = new Size(144, 22);
            mUndoToolStripMenuItem.Text = "Undo";
            mUndoToolStripMenuItem.Click += OnUndo;
            // 
            // mRedoToolStripMenuItem
            // 
            mRedoToolStripMenuItem.Name = "mRedoToolStripMenuItem";
            mRedoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            mRedoToolStripMenuItem.Size = new Size(144, 22);
            mRedoToolStripMenuItem.Text = "Redo";
            mRedoToolStripMenuItem.Click += OnRedo;
            // 
            // mConfigurationsToolStripMenuItem
            // 
            mConfigurationsToolStripMenuItem.Name = "mConfigurationsToolStripMenuItem";
            mConfigurationsToolStripMenuItem.Size = new Size(98, 25);
            mConfigurationsToolStripMenuItem.Text = "Configurations";
            mConfigurationsToolStripMenuItem.Click += OnConfigurations;
            // 
            // mToolsToolStripMenuItem
            // 
            mToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mCombineMotsFileToolStripMenuItem, toolStripSeparator1, mGenerateMurmurHashesToolStripMenuItem, toolStripSeparator6, mConvertOsageSkinParametersToToolStripMenuItem, toolStripSeparator7, mConvertObjectSetsToToolStripMenuItem });
            mToolsToolStripMenuItem.Name = "mToolsToolStripMenuItem";
            mToolsToolStripMenuItem.Size = new Size(47, 25);
            mToolsToolStripMenuItem.Text = "Tools";
            // 
            // mCombineMotsFileToolStripMenuItem
            // 
            mCombineMotsFileToolStripMenuItem.Name = "mCombineMotsFileToolStripMenuItem";
            mCombineMotsFileToolStripMenuItem.Size = new Size(264, 22);
            mCombineMotsFileToolStripMenuItem.Text = "Combine divided .mot files into one";
            mCombineMotsFileToolStripMenuItem.Click += OnCombineMotions;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(261, 6);
            // 
            // mGenerateMurmurHashesToolStripMenuItem
            // 
            mGenerateMurmurHashesToolStripMenuItem.Name = "mGenerateMurmurHashesToolStripMenuItem";
            mGenerateMurmurHashesToolStripMenuItem.Size = new Size(264, 22);
            mGenerateMurmurHashesToolStripMenuItem.Text = "Generate murmur hashes";
            mGenerateMurmurHashesToolStripMenuItem.Click += OnGenerateMurmurHashes;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(261, 6);
            // 
            // mConvertOsageSkinParametersToToolStripMenuItem
            // 
            mConvertOsageSkinParametersToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mConvertOspToClassicToolStripMenuItem, mConvertOspToF2ndToolStripMenuItem, mConvertOspToXToolStripMenuItem });
            mConvertOsageSkinParametersToToolStripMenuItem.Name = "mConvertOsageSkinParametersToToolStripMenuItem";
            mConvertOsageSkinParametersToToolStripMenuItem.Size = new Size(264, 22);
            mConvertOsageSkinParametersToToolStripMenuItem.Text = "Convert osage skin parameters to...";
            // 
            // mConvertOspToClassicToolStripMenuItem
            // 
            mConvertOspToClassicToolStripMenuItem.Name = "mConvertOspToClassicToolStripMenuItem";
            mConvertOspToClassicToolStripMenuItem.Size = new Size(117, 22);
            mConvertOspToClassicToolStripMenuItem.Text = "DT/F/FT";
            mConvertOspToClassicToolStripMenuItem.Click += OnConvertOspToClassic;
            // 
            // mConvertOspToF2ndToolStripMenuItem
            // 
            mConvertOspToF2ndToolStripMenuItem.Name = "mConvertOspToF2ndToolStripMenuItem";
            mConvertOspToF2ndToolStripMenuItem.Size = new Size(117, 22);
            mConvertOspToF2ndToolStripMenuItem.Text = "F 2nd";
            mConvertOspToF2ndToolStripMenuItem.Click += OnConvertOspToF2nd;
            // 
            // mConvertOspToXToolStripMenuItem
            // 
            mConvertOspToXToolStripMenuItem.Name = "mConvertOspToXToolStripMenuItem";
            mConvertOspToXToolStripMenuItem.Size = new Size(117, 22);
            mConvertOspToXToolStripMenuItem.Text = "X";
            mConvertOspToXToolStripMenuItem.Click += OnConvertOspToX;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(261, 6);
            // 
            // mConvertObjectSetsToToolStripMenuItem
            // 
            mConvertObjectSetsToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mConvertObjectSetsToTrianglesToolStripMenuItem, mConvertObjectSetsToTriangleStripToolStripMenuItem });
            mConvertObjectSetsToToolStripMenuItem.Name = "mConvertObjectSetsToToolStripMenuItem";
            mConvertObjectSetsToToolStripMenuItem.Size = new Size(264, 22);
            mConvertObjectSetsToToolStripMenuItem.Text = "Convert object sets in directory to...";
            // 
            // mConvertObjectSetsToTrianglesToolStripMenuItem
            // 
            mConvertObjectSetsToTrianglesToolStripMenuItem.Name = "mConvertObjectSetsToTrianglesToolStripMenuItem";
            mConvertObjectSetsToTrianglesToolStripMenuItem.Size = new Size(143, 22);
            mConvertObjectSetsToTrianglesToolStripMenuItem.Text = "Triangles";
            mConvertObjectSetsToTrianglesToolStripMenuItem.Click += OnConvertToTriangles;
            // 
            // mConvertObjectSetsToTriangleStripToolStripMenuItem
            // 
            mConvertObjectSetsToTriangleStripToolStripMenuItem.Name = "mConvertObjectSetsToTriangleStripToolStripMenuItem";
            mConvertObjectSetsToTriangleStripToolStripMenuItem.Size = new Size(143, 22);
            mConvertObjectSetsToTriangleStripToolStripMenuItem.Text = "Triangle Strip";
            mConvertObjectSetsToTriangleStripToolStripMenuItem.Click += OnConvertToTriangleStrip;
            // 
            // mOptionsToolStripMenuItem
            // 
            mOptionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mAutoCheckUpdatesToolStripMenuItem, toolStripSeparator4, mStylesToolStripMenuItem, toolStripSeparator5, mCamerasToolStripMenuItem });
            mOptionsToolStripMenuItem.Name = "mOptionsToolStripMenuItem";
            mOptionsToolStripMenuItem.Size = new Size(61, 25);
            mOptionsToolStripMenuItem.Text = "Options";
            // 
            // mAutoCheckUpdatesToolStripMenuItem
            // 
            mAutoCheckUpdatesToolStripMenuItem.Checked = true;
            mAutoCheckUpdatesToolStripMenuItem.CheckOnClick = true;
            mAutoCheckUpdatesToolStripMenuItem.CheckState = CheckState.Checked;
            mAutoCheckUpdatesToolStripMenuItem.Name = "mAutoCheckUpdatesToolStripMenuItem";
            mAutoCheckUpdatesToolStripMenuItem.Size = new Size(199, 22);
            mAutoCheckUpdatesToolStripMenuItem.Text = "Auto-check for updates";
            mAutoCheckUpdatesToolStripMenuItem.Click += OnAutoCheckUpdates;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(196, 6);
            // 
            // mStylesToolStripMenuItem
            // 
            mStylesToolStripMenuItem.Name = "mStylesToolStripMenuItem";
            mStylesToolStripMenuItem.Size = new Size(199, 22);
            mStylesToolStripMenuItem.Text = "Styles";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(196, 6);
            // 
            // mCamerasToolStripMenuItem
            // 
            mCamerasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mOrbitCameraToolStripMenuItem, mFreeCameraToolStripMenuItem });
            mCamerasToolStripMenuItem.Name = "mCamerasToolStripMenuItem";
            mCamerasToolStripMenuItem.Size = new Size(199, 22);
            mCamerasToolStripMenuItem.Text = "Cameras";
            // 
            // mOrbitCameraToolStripMenuItem
            // 
            mOrbitCameraToolStripMenuItem.Checked = true;
            mOrbitCameraToolStripMenuItem.CheckState = CheckState.Checked;
            mOrbitCameraToolStripMenuItem.Name = "mOrbitCameraToolStripMenuItem";
            mOrbitCameraToolStripMenuItem.Size = new Size(145, 22);
            mOrbitCameraToolStripMenuItem.Text = "Orbit Camera";
            mOrbitCameraToolStripMenuItem.Click += OnCameraModeClicked;
            // 
            // mFreeCameraToolStripMenuItem
            // 
            mFreeCameraToolStripMenuItem.Name = "mFreeCameraToolStripMenuItem";
            mFreeCameraToolStripMenuItem.Size = new Size(145, 22);
            mFreeCameraToolStripMenuItem.Text = "Free Camera";
            mFreeCameraToolStripMenuItem.Click += OnCameraModeClicked;
            // 
            // mHelpToolStripMenuItem
            // 
            mHelpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mUserGuideToolStripMenuItem, toolStripSeparator3, mCheckForUpdatesToolStripMenuItem, toolStripSeparator2, mAboutToolStripMenuItem });
            mHelpToolStripMenuItem.Name = "mHelpToolStripMenuItem";
            mHelpToolStripMenuItem.Size = new Size(44, 25);
            mHelpToolStripMenuItem.Text = "Help";
            // 
            // mUserGuideToolStripMenuItem
            // 
            mUserGuideToolStripMenuItem.Name = "mUserGuideToolStripMenuItem";
            mUserGuideToolStripMenuItem.Size = new Size(204, 22);
            mUserGuideToolStripMenuItem.Text = "User guide";
            mUserGuideToolStripMenuItem.Click += OnUserGuide;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(201, 6);
            // 
            // mCheckForUpdatesToolStripMenuItem
            // 
            mCheckForUpdatesToolStripMenuItem.Name = "mCheckForUpdatesToolStripMenuItem";
            mCheckForUpdatesToolStripMenuItem.Size = new Size(204, 22);
            mCheckForUpdatesToolStripMenuItem.Text = "Check for updates";
            mCheckForUpdatesToolStripMenuItem.Click += OnCheckForUpdates;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(201, 6);
            // 
            // mAboutToolStripMenuItem
            // 
            mAboutToolStripMenuItem.Name = "mAboutToolStripMenuItem";
            mAboutToolStripMenuItem.Size = new Size(204, 22);
            mAboutToolStripMenuItem.Text = "About Miku Miku Model";
            mAboutToolStripMenuItem.Click += OnAbout;
            // 
            // mPanel
            // 
            mPanel.Controls.Add(mMenuStrip);
            mPanel.Dock = DockStyle.Top;
            mPanel.Location = new Point(0, 0);
            mPanel.Margin = new Padding(4, 3, 4, 3);
            mPanel.Name = "mPanel";
            mPanel.Size = new Size(859, 29);
            mPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(859, 508);
            Controls.Add(mPanel);
            Controls.Add(mMainSplitContainer);
            DoubleBuffered = true;
            KeyPreview = true;
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "Miku Miku Model";
            mMainSplitContainer.Panel2.ResumeLayout(false);
            ((ISupportInitialize)mMainSplitContainer).EndInit();
            mMainSplitContainer.ResumeLayout(false);
            mRightSplitContainer.Panel1.ResumeLayout(false);
            mRightSplitContainer.Panel2.ResumeLayout(false);
            ((ISupportInitialize)mRightSplitContainer).EndInit();
            mRightSplitContainer.ResumeLayout(false);
            mMenuStrip.ResumeLayout(false);
            mMenuStrip.PerformLayout();
            mPanel.ResumeLayout(false);
            mPanel.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mMainSplitContainer;
        private System.Windows.Forms.MenuStrip mMenuStrip;
        private System.Windows.Forms.SplitContainer mRightSplitContainer;
        private System.Windows.Forms.ToolStripMenuItem mFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator mToolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mExitToolStripMenuItem;
        private System.Windows.Forms.Panel mPanel;
        private System.Windows.Forms.PropertyGrid mPropertyGrid;
        private MikuMikuModel.Nodes.Wrappers.NodeTreeView mNodeTreeView;
        private System.Windows.Forms.ToolStripMenuItem mCloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mConfigurationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mHelpToolStripMenuItem;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolStripMenuItem mToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mCombineMotsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mGenerateMurmurHashesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mUndoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mRedoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mUserGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mCheckForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mAboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mStylesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mCamerasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mOrbitCameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mFreeCameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mAutoCheckUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mOpenRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mConvertOsageSkinParametersToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mConvertOspToClassicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mConvertOspToF2ndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mConvertOspToXToolStripMenuItem;
        private ToolStripMenuItem mConvertObjectSetsToToolStripMenuItem;
        private ToolStripMenuItem mConvertObjectSetsToTrianglesToolStripMenuItem;
        private ToolStripMenuItem mConvertObjectSetsToTriangleStripToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
    }
}
