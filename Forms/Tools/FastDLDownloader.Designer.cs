namespace BuyiTools.Forms.Tools
{
    partial class FastDLDownloader
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
            label4 = new Label();
            ButStartDownload = new Button();
            ButUseDefaultUA = new Button();
            TxtUserAgent = new TextBox();
            label3 = new Label();
            TxtLinks = new TextBox();
            label2 = new Label();
            TxtHost = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(155, 311);
            label4.Name = "label4";
            label4.Size = new Size(329, 57);
            label4.TabIndex = 26;
            label4.Text = "说明：本工具模拟 Source 引擎游戏进行 fastDL 下载。\r\n先尝试下载 xx.bz2 ，再尝试下载 xx 本身。\r\n下载后的文件会输出在桌面的新文件夹里。";
            // 
            // ButStartDownload
            // 
            ButStartDownload.Location = new Point(23, 311);
            ButStartDownload.Name = "ButStartDownload";
            ButStartDownload.Size = new Size(115, 36);
            ButStartDownload.TabIndex = 25;
            ButStartDownload.Text = "开始下载";
            ButStartDownload.UseVisualStyleBackColor = true;
            ButStartDownload.Click += ButStartDownload_Click;
            // 
            // ButUseDefaultUA
            // 
            ButUseDefaultUA.Location = new Point(553, 270);
            ButUseDefaultUA.Name = "ButUseDefaultUA";
            ButUseDefaultUA.Size = new Size(110, 37);
            ButUseDefaultUA.TabIndex = 24;
            ButUseDefaultUA.Text = "默认UA";
            ButUseDefaultUA.UseVisualStyleBackColor = true;
            ButUseDefaultUA.Click += ButUseDefaultUA_Click;
            // 
            // TxtUserAgent
            // 
            TxtUserAgent.Location = new Point(12, 276);
            TxtUserAgent.MaxLength = 500;
            TxtUserAgent.Name = "TxtUserAgent";
            TxtUserAgent.Size = new Size(535, 24);
            TxtUserAgent.TabIndex = 23;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 254);
            label3.Name = "label3";
            label3.Size = new Size(91, 19);
            label3.TabIndex = 22;
            label3.Text = "User Agent：";
            // 
            // TxtLinks
            // 
            TxtLinks.Location = new Point(12, 76);
            TxtLinks.MaxLength = 999999;
            TxtLinks.Multiline = true;
            TxtLinks.Name = "TxtLinks";
            TxtLinks.ScrollBars = ScrollBars.Both;
            TxtLinks.Size = new Size(659, 175);
            TxtLinks.TabIndex = 21;
            TxtLinks.WordWrap = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 54);
            label2.Name = "label2";
            label2.Size = new Size(74, 19);
            label2.TabIndex = 20;
            label2.Text = "文件列表：";
            // 
            // TxtHost
            // 
            TxtHost.Location = new Point(12, 27);
            TxtHost.MaxLength = 500;
            TxtHost.Name = "TxtHost";
            TxtHost.Size = new Size(659, 24);
            TxtHost.TabIndex = 19;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 5);
            label1.Name = "label1";
            label1.Size = new Size(152, 19);
            label1.TabIndex = 18;
            label1.Text = "统一前缀（比如域名）：";
            // 
            // FastDLDownloader
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(label4);
            Controls.Add(ButStartDownload);
            Controls.Add(ButUseDefaultUA);
            Controls.Add(TxtUserAgent);
            Controls.Add(label3);
            Controls.Add(TxtLinks);
            Controls.Add(label2);
            Controls.Add(TxtHost);
            Controls.Add(label1);
            Name = "FastDLDownloader";
            Size = new Size(674, 395);
            Load += FastDLDownloader_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label4;
        private Button ButStartDownload;
        private Button ButUseDefaultUA;
        private TextBox TxtUserAgent;
        private Label label3;
        private TextBox TxtLinks;
        private Label label2;
        private TextBox TxtHost;
        private Label label1;
    }
}
