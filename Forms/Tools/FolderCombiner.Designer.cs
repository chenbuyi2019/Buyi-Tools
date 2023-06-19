namespace BuyiTools.Forms.Tools
{
    partial class FolderCombiner
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
            TxtFolders = new TextBox();
            label2 = new Label();
            TxtOutput = new TextBox();
            label3 = new Label();
            ButWork = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 9);
            label1.Name = "label1";
            label1.Size = new Size(139, 19);
            label1.TabIndex = 0;
            label1.Text = "要组合的文件夹列表：";
            // 
            // TxtFolders
            // 
            TxtFolders.Location = new Point(14, 31);
            TxtFolders.Multiline = true;
            TxtFolders.Name = "TxtFolders";
            TxtFolders.ScrollBars = ScrollBars.Both;
            TxtFolders.Size = new Size(774, 160);
            TxtFolders.TabIndex = 1;
            TxtFolders.WordWrap = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 194);
            label2.Name = "label2";
            label2.Size = new Size(87, 19);
            label2.TabIndex = 2;
            label2.Text = "输出文件夹：";
            // 
            // TxtOutput
            // 
            TxtOutput.Location = new Point(14, 216);
            TxtOutput.MaxLength = 1000;
            TxtOutput.Name = "TxtOutput";
            TxtOutput.Size = new Size(774, 24);
            TxtOutput.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 243);
            label3.Name = "label3";
            label3.Size = new Size(520, 38);
            label3.TabIndex = 4;
            label3.Text = "说明： 本工具把要组合的文件夹里的子文件按原目录结构全部集中复制在输出文件夹里。\r\n遇到同名存在的会跳过。";
            // 
            // ButWork
            // 
            ButWork.Location = new Point(680, 243);
            ButWork.Name = "ButWork";
            ButWork.Size = new Size(102, 38);
            ButWork.TabIndex = 5;
            ButWork.Text = "组合";
            ButWork.UseVisualStyleBackColor = true;
            ButWork.Click += ButWork_Click;
            // 
            // FolderCombiner
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(ButWork);
            Controls.Add(label3);
            Controls.Add(TxtOutput);
            Controls.Add(label2);
            Controls.Add(TxtFolders);
            Controls.Add(label1);
            Name = "FolderCombiner";
            Size = new Size(799, 353);
            Load += FolderCombiner_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox TxtFolders;
        private Label label2;
        private TextBox TxtOutput;
        private Label label3;
        private Button ButWork;
    }
}
