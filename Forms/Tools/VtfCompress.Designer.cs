namespace BuyiTools.Forms.Tools
{
    partial class VtfCompress
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
            TxtFolder = new TextBox();
            label2 = new Label();
            TxtFileSizeLimit = new NumericUpDown();
            label3 = new Label();
            checkResizeImage = new CheckBox();
            ButStart = new Button();
            ButDownloadVtfcmd = new Button();
            ((System.ComponentModel.ISupportInitialize)TxtFileSizeLimit).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 9);
            label1.Name = "label1";
            label1.Size = new Size(182, 19);
            label1.TabIndex = 0;
            label1.Text = "请输入vtf文件或文件夹路径：";
            // 
            // TxtFolder
            // 
            TxtFolder.Location = new Point(3, 31);
            TxtFolder.Name = "TxtFolder";
            TxtFolder.Size = new Size(607, 24);
            TxtFolder.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 69);
            label2.Name = "label2";
            label2.Size = new Size(173, 19);
            label2.TabIndex = 2;
            label2.Text = "只压缩超过多少MB的文件：";
            // 
            // TxtFileSizeLimit
            // 
            TxtFileSizeLimit.DecimalPlaces = 1;
            TxtFileSizeLimit.Increment = new decimal(new int[] { 2, 0, 0, 65536 });
            TxtFileSizeLimit.Location = new Point(182, 69);
            TxtFileSizeLimit.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            TxtFileSizeLimit.Name = "TxtFileSizeLimit";
            TxtFileSizeLimit.Size = new Size(74, 24);
            TxtFileSizeLimit.TabIndex = 3;
            TxtFileSizeLimit.Value = new decimal(new int[] { 14, 0, 0, 65536 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 112);
            label3.Name = "label3";
            label3.Size = new Size(493, 95);
            label3.TabIndex = 4;
            label3.Text = "本工具会把文件夹路径内的超过指定文件大小的 vtf 进行压缩。（单个vtf文件也行）\r\n压缩的成果会被放在桌面上的新建文件夹。 \r\n压缩一律使用 DXT1+DXT5 Alpha 。\r\n\r\n你需要手动下载 VTFCmd 并安装到本程序的同一目录下。";
            // 
            // checkResizeImage
            // 
            checkResizeImage.AutoSize = true;
            checkResizeImage.Location = new Point(391, 70);
            checkResizeImage.Name = "checkResizeImage";
            checkResizeImage.Size = new Size(160, 23);
            checkResizeImage.TabIndex = 5;
            checkResizeImage.Text = "缩小图片到50%的尺寸";
            checkResizeImage.UseVisualStyleBackColor = true;
            // 
            // ButStart
            // 
            ButStart.Location = new Point(449, 186);
            ButStart.Name = "ButStart";
            ButStart.Size = new Size(161, 37);
            ButStart.TabIndex = 6;
            ButStart.Text = "开始";
            ButStart.UseVisualStyleBackColor = true;
            ButStart.Click += ButStart_Click;
            // 
            // ButDownloadVtfcmd
            // 
            ButDownloadVtfcmd.Location = new Point(449, 143);
            ButDownloadVtfcmd.Name = "ButDownloadVtfcmd";
            ButDownloadVtfcmd.Size = new Size(161, 37);
            ButDownloadVtfcmd.TabIndex = 7;
            ButDownloadVtfcmd.Text = "下载VTFCmd";
            ButDownloadVtfcmd.UseVisualStyleBackColor = true;
            ButDownloadVtfcmd.Click += ButDownloadVtfcmd_Click;
            // 
            // VtfCompress
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(ButDownloadVtfcmd);
            Controls.Add(ButStart);
            Controls.Add(checkResizeImage);
            Controls.Add(label3);
            Controls.Add(TxtFileSizeLimit);
            Controls.Add(label2);
            Controls.Add(TxtFolder);
            Controls.Add(label1);
            Name = "VtfCompress";
            Size = new Size(627, 239);
            Load += VtfCompress_Load;
            ((System.ComponentModel.ISupportInitialize)TxtFileSizeLimit).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox TxtFolder;
        private Label label2;
        private NumericUpDown TxtFileSizeLimit;
        private Label label3;
        private CheckBox checkResizeImage;
        private Button ButStart;
        private Button ButDownloadVtfcmd;
    }
}
