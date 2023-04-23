namespace BuyiTools.Tools
{
    partial class FileDeleteTool
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
            TxtWorkingDir = new TextBox();
            label1 = new Label();
            TxtTargetFiles = new TextBox();
            label2 = new Label();
            ListFileSets = new CheckedListBox();
            label3 = new Label();
            label4 = new Label();
            ButStart = new Button();
            CheckScanOnly = new CheckBox();
            SuspendLayout();
            // 
            // TxtWorkingDir
            // 
            TxtWorkingDir.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            TxtWorkingDir.Location = new Point(83, 11);
            TxtWorkingDir.MaxLength = 1000;
            TxtWorkingDir.Name = "TxtWorkingDir";
            TxtWorkingDir.Size = new Size(679, 23);
            TxtWorkingDir.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 11);
            label1.Name = "label1";
            label1.Size = new Size(74, 19);
            label1.TabIndex = 4;
            label1.Text = "工作文件夹";
            // 
            // TxtTargetFiles
            // 
            TxtTargetFiles.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            TxtTargetFiles.Location = new Point(83, 40);
            TxtTargetFiles.MaxLength = 9900;
            TxtTargetFiles.Multiline = true;
            TxtTargetFiles.Name = "TxtTargetFiles";
            TxtTargetFiles.ScrollBars = ScrollBars.Both;
            TxtTargetFiles.Size = new Size(679, 161);
            TxtTargetFiles.TabIndex = 7;
            TxtTargetFiles.WordWrap = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 80);
            label2.Name = "label2";
            label2.Size = new Size(74, 57);
            label2.TabIndex = 6;
            label2.Text = "要删的文件\r\n相对路径\r\n一行一个";
            // 
            // ListFileSets
            // 
            ListFileSets.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            ListFileSets.FormattingEnabled = true;
            ListFileSets.Location = new Point(83, 207);
            ListFileSets.Name = "ListFileSets";
            ListFileSets.Size = new Size(174, 194);
            ListFileSets.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 207);
            label3.Name = "label3";
            label3.Size = new Size(61, 19);
            label3.TabIndex = 9;
            label3.Text = "目标预设";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(263, 207);
            label4.Name = "label4";
            label4.Size = new Size(334, 76);
            label4.TabIndex = 10;
            label4.Text = "说明：\r\n工具会把工作文件夹内相对路径符合目标的文件全部删除\r\n不区分大小写，不会进回收站\r\n目标预设是一些常用的游戏自带资源，用来进行去重";
            // 
            // ButStart
            // 
            ButStart.Location = new Point(675, 210);
            ButStart.Name = "ButStart";
            ButStart.Size = new Size(87, 38);
            ButStart.TabIndex = 11;
            ButStart.Text = "开始";
            ButStart.UseVisualStyleBackColor = true;
            ButStart.Click += ButStart_Click;
            // 
            // CheckScanOnly
            // 
            CheckScanOnly.AutoSize = true;
            CheckScanOnly.Location = new Point(656, 254);
            CheckScanOnly.Name = "CheckScanOnly";
            CheckScanOnly.Size = new Size(106, 23);
            CheckScanOnly.TabIndex = 12;
            CheckScanOnly.Text = "只扫描不删除";
            CheckScanOnly.TextAlign = ContentAlignment.MiddleCenter;
            CheckScanOnly.UseVisualStyleBackColor = true;
            // 
            // FileDeleteTool
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(CheckScanOnly);
            Controls.Add(ButStart);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(ListFileSets);
            Controls.Add(TxtTargetFiles);
            Controls.Add(label2);
            Controls.Add(TxtWorkingDir);
            Controls.Add(label1);
            Name = "FileDeleteTool";
            Size = new Size(774, 435);
            Load += FileDeleteTool_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtWorkingDir;
        private Label label1;
        private TextBox TxtTargetFiles;
        private Label label2;
        private CheckedListBox ListFileSets;
        private Label label3;
        private Label label4;
        private Button ButStart;
        private CheckBox CheckScanOnly;
    }
}
