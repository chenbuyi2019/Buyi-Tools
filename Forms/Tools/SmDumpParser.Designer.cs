namespace BuyiTools.Forms.Tools
{
    partial class SmDumpParser
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
            ButReadClipboard = new Button();
            ButReadFile = new Button();
            label1 = new Label();
            ListDumpCount = new ListView();
            tabs = new TabControl();
            tabPage1 = new TabPage();
            ListDumpDetail = new ListView();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            ListDumpCompare = new ListView();
            tabs.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // ButReadClipboard
            // 
            ButReadClipboard.Location = new Point(12, 11);
            ButReadClipboard.Name = "ButReadClipboard";
            ButReadClipboard.Size = new Size(103, 29);
            ButReadClipboard.TabIndex = 0;
            ButReadClipboard.Text = "读取剪贴板";
            ButReadClipboard.UseVisualStyleBackColor = true;
            ButReadClipboard.Click += ButReadClipboard_Click;
            // 
            // ButReadFile
            // 
            ButReadFile.Location = new Point(121, 11);
            ButReadFile.Name = "ButReadFile";
            ButReadFile.Size = new Size(103, 29);
            ButReadFile.TabIndex = 1;
            ButReadFile.Text = "读取文件";
            ButReadFile.UseVisualStyleBackColor = true;
            ButReadFile.Click += ButReadFile_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(240, 11);
            label1.Name = "label1";
            label1.Size = new Size(329, 38);
            label1.TabIndex = 2;
            label1.Text = "说明： 本工具用来解析 sm_dump_handles 生成的文件\r\n请完整提供文件内容，包括表头";
            // 
            // ListDumpCount
            // 
            ListDumpCount.Dock = DockStyle.Fill;
            ListDumpCount.FullRowSelect = true;
            ListDumpCount.Location = new Point(3, 3);
            ListDumpCount.MultiSelect = false;
            ListDumpCount.Name = "ListDumpCount";
            ListDumpCount.ShowGroups = false;
            ListDumpCount.Size = new Size(683, 391);
            ListDumpCount.TabIndex = 3;
            ListDumpCount.UseCompatibleStateImageBehavior = false;
            ListDumpCount.View = View.Details;
            // 
            // tabs
            // 
            tabs.Controls.Add(tabPage1);
            tabs.Controls.Add(tabPage2);
            tabs.Controls.Add(tabPage3);
            tabs.Dock = DockStyle.Bottom;
            tabs.Location = new Point(0, 52);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(697, 429);
            tabs.TabIndex = 4;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(ListDumpDetail);
            tabPage1.Location = new Point(4, 28);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(689, 397);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "明细";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // ListDumpDetail
            // 
            ListDumpDetail.Dock = DockStyle.Fill;
            ListDumpDetail.FullRowSelect = true;
            ListDumpDetail.Location = new Point(3, 3);
            ListDumpDetail.MultiSelect = false;
            ListDumpDetail.Name = "ListDumpDetail";
            ListDumpDetail.ShowGroups = false;
            ListDumpDetail.Size = new Size(683, 391);
            ListDumpDetail.TabIndex = 4;
            ListDumpDetail.UseCompatibleStateImageBehavior = false;
            ListDumpDetail.View = View.Details;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(ListDumpCount);
            tabPage2.Location = new Point(4, 28);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(689, 397);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "统计";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(ListDumpCompare);
            tabPage3.Location = new Point(4, 28);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(689, 397);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "统计 对比上次";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // ListDumpCompare
            // 
            ListDumpCompare.Dock = DockStyle.Fill;
            ListDumpCompare.FullRowSelect = true;
            ListDumpCompare.Location = new Point(3, 3);
            ListDumpCompare.MultiSelect = false;
            ListDumpCompare.Name = "ListDumpCompare";
            ListDumpCompare.ShowGroups = false;
            ListDumpCompare.Size = new Size(683, 391);
            ListDumpCompare.TabIndex = 4;
            ListDumpCompare.UseCompatibleStateImageBehavior = false;
            ListDumpCompare.View = View.Details;
            // 
            // SmDumpParser
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(tabs);
            Controls.Add(label1);
            Controls.Add(ButReadFile);
            Controls.Add(ButReadClipboard);
            Name = "SmDumpParser";
            Size = new Size(697, 481);
            Load += SmDumpParser_Load;
            tabs.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ButReadClipboard;
        private Button ButReadFile;
        private Label label1;
        private ListView ListDumpCount;
        private TabControl tabs;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ListView ListDumpDetail;
        private TabPage tabPage3;
        private ListView ListDumpCompare;
    }
}
