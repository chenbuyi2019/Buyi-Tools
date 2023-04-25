namespace BuyiTools.Forms.Tools
{
    partial class FastDLCreator
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
            TxtTarget = new TextBox();
            label2 = new Label();
            TxtMaxCompressSize = new NumericUpDown();
            ButCopyPaths = new Button();
            ButMakeBz2 = new Button();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)TxtMaxCompressSize).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 18);
            label1.Name = "label1";
            label1.Size = new Size(87, 19);
            label1.TabIndex = 0;
            label1.Text = "目标文件夹：";
            // 
            // TxtTarget
            // 
            TxtTarget.Location = new Point(139, 18);
            TxtTarget.MaxLength = 600;
            TxtTarget.Name = "TxtTarget";
            TxtTarget.Size = new Size(603, 24);
            TxtTarget.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 50);
            label2.Name = "label2";
            label2.Size = new Size(147, 19);
            label2.TabIndex = 2;
            label2.Text = "超过多少MB的不压缩：";
            // 
            // TxtMaxCompressSize
            // 
            TxtMaxCompressSize.Location = new Point(160, 48);
            TxtMaxCompressSize.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            TxtMaxCompressSize.Name = "TxtMaxCompressSize";
            TxtMaxCompressSize.Size = new Size(102, 24);
            TxtMaxCompressSize.TabIndex = 3;
            TxtMaxCompressSize.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // ButCopyPaths
            // 
            ButCopyPaths.Location = new Point(400, 108);
            ButCopyPaths.Name = "ButCopyPaths";
            ButCopyPaths.Size = new Size(168, 27);
            ButCopyPaths.TabIndex = 4;
            ButCopyPaths.Text = "复制文件列表";
            ButCopyPaths.UseVisualStyleBackColor = true;
            ButCopyPaths.Click += ButCopyPaths_Click;
            // 
            // ButMakeBz2
            // 
            ButMakeBz2.Location = new Point(574, 108);
            ButMakeBz2.Name = "ButMakeBz2";
            ButMakeBz2.Size = new Size(168, 27);
            ButMakeBz2.TabIndex = 5;
            ButMakeBz2.Text = "生成 Bz2 文件";
            ButMakeBz2.UseVisualStyleBackColor = true;
            ButMakeBz2.Click += ButMakeBz2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 91);
            label3.Name = "label3";
            label3.Size = new Size(278, 76);
            label3.TabIndex = 6;
            label3.Text = "说明：\r\n工具会把目标文件夹里的全部文件打包为 .bz2\r\n但是会跳过文件大小过大的文件\r\n输出会在目标文件夹旁的 \"名字-bz2\" 文件夹里";
            // 
            // FastDLCreator
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(label3);
            Controls.Add(ButMakeBz2);
            Controls.Add(ButCopyPaths);
            Controls.Add(TxtMaxCompressSize);
            Controls.Add(label2);
            Controls.Add(TxtTarget);
            Controls.Add(label1);
            Name = "FastDLCreator";
            Size = new Size(756, 188);
            Load += FastDLCreator_Load;
            ((System.ComponentModel.ISupportInitialize)TxtMaxCompressSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox TxtTarget;
        private Label label2;
        private NumericUpDown TxtMaxCompressSize;
        private Button ButCopyPaths;
        private Button ButMakeBz2;
        private Label label3;
    }
}
