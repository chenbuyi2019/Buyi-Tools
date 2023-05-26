namespace BuyiTools.Forms.Tools
{
    partial class MklinkTool
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            TxtParentFolder = new TextBox();
            TxtTargetFolders = new TextBox();
            ListFiles = new CheckedListBox();
            ButCreate = new Button();
            labSelectedCount = new Label();
            ButRefreshFileList = new Button();
            label4 = new Label();
            checkForceRemoveTargets = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 11);
            label1.Name = "label1";
            label1.Size = new Size(74, 19);
            label1.TabIndex = 0;
            label1.Text = "母体文件夹";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 77);
            label2.Name = "label2";
            label2.Size = new Size(74, 38);
            label2.TabIndex = 1;
            label2.Text = "目标文件夹\r\n一行一个";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 210);
            label3.Name = "label3";
            label3.Size = new Size(61, 19);
            label3.TabIndex = 2;
            label3.Text = "链接对象";
            // 
            // TxtParentFolder
            // 
            TxtParentFolder.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            TxtParentFolder.Location = new Point(93, 8);
            TxtParentFolder.MaxLength = 1000;
            TxtParentFolder.Name = "TxtParentFolder";
            TxtParentFolder.Size = new Size(760, 23);
            TxtParentFolder.TabIndex = 3;
            TxtParentFolder.TextChanged += TxtParentFolder_TextChanged;
            // 
            // TxtTargetFolders
            // 
            TxtTargetFolders.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            TxtTargetFolders.Location = new Point(93, 37);
            TxtTargetFolders.MaxLength = 9900;
            TxtTargetFolders.Multiline = true;
            TxtTargetFolders.Name = "TxtTargetFolders";
            TxtTargetFolders.ScrollBars = ScrollBars.Both;
            TxtTargetFolders.Size = new Size(760, 161);
            TxtTargetFolders.TabIndex = 4;
            TxtTargetFolders.WordWrap = false;
            // 
            // ListFiles
            // 
            ListFiles.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            ListFiles.FormattingEnabled = true;
            ListFiles.Location = new Point(93, 210);
            ListFiles.Name = "ListFiles";
            ListFiles.Size = new Size(353, 184);
            ListFiles.TabIndex = 5;
            ListFiles.SelectedIndexChanged += ListFiles_SelectedIndexChanged;
            // 
            // ButCreate
            // 
            ButCreate.Location = new Point(779, 210);
            ButCreate.Name = "ButCreate";
            ButCreate.Size = new Size(74, 44);
            ButCreate.TabIndex = 6;
            ButCreate.Text = "创建";
            ButCreate.UseVisualStyleBackColor = true;
            ButCreate.Click += ButCreate_Click;
            // 
            // labSelectedCount
            // 
            labSelectedCount.AutoSize = true;
            labSelectedCount.Location = new Point(452, 267);
            labSelectedCount.Name = "labSelectedCount";
            labSelectedCount.Size = new Size(35, 19);
            labSelectedCount.TabIndex = 7;
            labSelectedCount.Text = "数量";
            // 
            // ButRefreshFileList
            // 
            ButRefreshFileList.Location = new Point(452, 210);
            ButRefreshFileList.Name = "ButRefreshFileList";
            ButRefreshFileList.Size = new Size(115, 43);
            ButRefreshFileList.TabIndex = 8;
            ButRefreshFileList.Text = "刷新文件列表";
            ButRefreshFileList.UseVisualStyleBackColor = true;
            ButRefreshFileList.Click += ButRefreshFileList_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(452, 299);
            label4.Name = "label4";
            label4.Size = new Size(356, 76);
            label4.TabIndex = 9;
            label4.Text = "说明：\r\n工具会在每个目标文件夹下创建一个或多个 mklink 符号链接\r\n来源是母体文件夹下的指定对象（在左侧勾选）\r\n如果有同名文件已经存在于目标文件夹，默认会直接跳过";
            // 
            // checkForceRemoveTargets
            // 
            checkForceRemoveTargets.AutoSize = true;
            checkForceRemoveTargets.Location = new Point(573, 210);
            checkForceRemoveTargets.Name = "checkForceRemoveTargets";
            checkForceRemoveTargets.Size = new Size(171, 23);
            checkForceRemoveTargets.TabIndex = 10;
            checkForceRemoveTargets.Text = "强制覆盖已经存在的目标";
            checkForceRemoveTargets.UseVisualStyleBackColor = true;
            // 
            // MklinkTool
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(checkForceRemoveTargets);
            Controls.Add(ButCreate);
            Controls.Add(label4);
            Controls.Add(ButRefreshFileList);
            Controls.Add(labSelectedCount);
            Controls.Add(ListFiles);
            Controls.Add(TxtTargetFolders);
            Controls.Add(TxtParentFolder);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "MklinkTool";
            Size = new Size(865, 419);
            Load += MklinkTool_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox TxtParentFolder;
        private TextBox TxtTargetFolders;
        private CheckedListBox ListFiles;
        private Button ButCreate;
        private Label labSelectedCount;
        private Button ButRefreshFileList;
        private Label label4;
        private CheckBox checkForceRemoveTargets;
    }
}
