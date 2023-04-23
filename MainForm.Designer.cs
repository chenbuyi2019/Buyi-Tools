namespace BuyiTools
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TopMenu = new ToolStrip();
            ListTools = new ToolStripDropDownButton();
            ListAdvanced = new ToolStripDropDownButton();
            ButRestartAsAdmin = new ToolStripMenuItem();
            TxtLog = new TextBox();
            PnTool = new Panel();
            TopMenu.SuspendLayout();
            SuspendLayout();
            // 
            // TopMenu
            // 
            TopMenu.BackColor = Color.FromArgb(224, 224, 224);
            TopMenu.Font = new Font("Microsoft YaHei UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            TopMenu.Items.AddRange(new ToolStripItem[] { ListTools, ListAdvanced });
            TopMenu.Location = new Point(0, 0);
            TopMenu.Name = "TopMenu";
            TopMenu.Size = new Size(580, 27);
            TopMenu.TabIndex = 0;
            TopMenu.Text = "toolStrip1";
            // 
            // ListTools
            // 
            ListTools.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ListTools.ImageTransparentColor = Color.Magenta;
            ListTools.Name = "ListTools";
            ListTools.Size = new Size(52, 24);
            ListTools.Text = "工具";
            // 
            // ListAdvanced
            // 
            ListAdvanced.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ListAdvanced.DropDownItems.AddRange(new ToolStripItem[] { ButRestartAsAdmin });
            ListAdvanced.ImageTransparentColor = Color.Magenta;
            ListAdvanced.Name = "ListAdvanced";
            ListAdvanced.Size = new Size(52, 24);
            ListAdvanced.Text = "高级";
            // 
            // ButRestartAsAdmin
            // 
            ButRestartAsAdmin.Name = "ButRestartAsAdmin";
            ButRestartAsAdmin.Size = new Size(183, 24);
            ButRestartAsAdmin.Text = "管理员权限重启";
            ButRestartAsAdmin.Click += ButRestartAsAdmin_Click;
            // 
            // TxtLog
            // 
            TxtLog.Dock = DockStyle.Bottom;
            TxtLog.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            TxtLog.Location = new Point(0, 161);
            TxtLog.Multiline = true;
            TxtLog.Name = "TxtLog";
            TxtLog.ReadOnly = true;
            TxtLog.ScrollBars = ScrollBars.Both;
            TxtLog.Size = new Size(580, 194);
            TxtLog.TabIndex = 1;
            // 
            // PnTool
            // 
            PnTool.Dock = DockStyle.Fill;
            PnTool.Location = new Point(0, 27);
            PnTool.Name = "PnTool";
            PnTool.Size = new Size(580, 134);
            PnTool.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(580, 355);
            Controls.Add(PnTool);
            Controls.Add(TxtLog);
            Controls.Add(TopMenu);
            DoubleBuffered = true;
            Font = new Font("Microsoft YaHei UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "布衣工具箱";
            Load += MainForm_Load;
            TopMenu.ResumeLayout(false);
            TopMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip TopMenu;
        private TextBox TxtLog;
        private Panel PnTool;
        private ToolStripDropDownButton ListTools;
        private ToolStripDropDownButton ListAdvanced;
        private ToolStripMenuItem ButRestartAsAdmin;
    }
}