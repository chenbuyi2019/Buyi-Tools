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
            components = new System.ComponentModel.Container();
            TopMenu = new ToolStrip();
            MenuTools = new ToolStripDropDownButton();
            MenuAdvanced = new ToolStripDropDownButton();
            ButRestartAsAdmin = new ToolStripMenuItem();
            LastErrorDetailToolStripMenuItem = new ToolStripMenuItem();
            MenuAbout = new ToolStripDropDownButton();
            githubUrlToolStripMenuItem = new ToolStripMenuItem();
            buildTimeToolStripMenuItem = new ToolStripMenuItem();
            TxtLog = new TextBox();
            PnTool = new Panel();
            TimerMoveRandom = new System.Windows.Forms.Timer(components);
            TopMenu.SuspendLayout();
            SuspendLayout();
            // 
            // TopMenu
            // 
            TopMenu.BackColor = Color.FromArgb(224, 224, 224);
            TopMenu.Font = new Font("Microsoft YaHei UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            TopMenu.Items.AddRange(new ToolStripItem[] { MenuTools, MenuAdvanced, MenuAbout });
            TopMenu.Location = new Point(0, 0);
            TopMenu.Name = "TopMenu";
            TopMenu.Size = new Size(580, 27);
            TopMenu.TabIndex = 0;
            TopMenu.Text = "toolStrip1";
            // 
            // MenuTools
            // 
            MenuTools.DisplayStyle = ToolStripItemDisplayStyle.Text;
            MenuTools.ImageTransparentColor = Color.Magenta;
            MenuTools.Name = "MenuTools";
            MenuTools.Size = new Size(52, 24);
            MenuTools.Text = "工具";
            // 
            // MenuAdvanced
            // 
            MenuAdvanced.DisplayStyle = ToolStripItemDisplayStyle.Text;
            MenuAdvanced.DropDownItems.AddRange(new ToolStripItem[] { LastErrorDetailToolStripMenuItem, ButRestartAsAdmin });
            MenuAdvanced.ImageTransparentColor = Color.Magenta;
            MenuAdvanced.Name = "MenuAdvanced";
            MenuAdvanced.Size = new Size(52, 24);
            MenuAdvanced.Text = "高级";
            // 
            // ButRestartAsAdmin
            // 
            ButRestartAsAdmin.Name = "ButRestartAsAdmin";
            ButRestartAsAdmin.Size = new Size(228, 24);
            ButRestartAsAdmin.Text = "管理员权限重启本软件";
            ButRestartAsAdmin.Click += ButRestartAsAdmin_Click;
            // 
            // LastErrorDetailToolStripMenuItem
            // 
            LastErrorDetailToolStripMenuItem.Name = "LastErrorDetailToolStripMenuItem";
            LastErrorDetailToolStripMenuItem.Size = new Size(213, 24);
            LastErrorDetailToolStripMenuItem.Text = "查看最近的错误细节";
            LastErrorDetailToolStripMenuItem.Click += LastErrorDetailToolStripMenuItem_Click;
            // 
            // MenuAbout
            // 
            MenuAbout.DisplayStyle = ToolStripItemDisplayStyle.Text;
            MenuAbout.DropDownItems.AddRange(new ToolStripItem[] { githubUrlToolStripMenuItem, buildTimeToolStripMenuItem });
            MenuAbout.ImageTransparentColor = Color.Magenta;
            MenuAbout.Name = "MenuAbout";
            MenuAbout.Size = new Size(52, 24);
            MenuAbout.Text = "关于";
            // 
            // githubUrlToolStripMenuItem
            // 
            githubUrlToolStripMenuItem.Name = "githubUrlToolStripMenuItem";
            githubUrlToolStripMenuItem.Size = new Size(184, 24);
            githubUrlToolStripMenuItem.Text = "源码 on Github";
            githubUrlToolStripMenuItem.Click += GithubUrlToolStripMenuItem_Click;
            // 
            // buildTimeToolStripMenuItem
            // 
            buildTimeToolStripMenuItem.Name = "buildTimeToolStripMenuItem";
            buildTimeToolStripMenuItem.Size = new Size(184, 24);
            buildTimeToolStripMenuItem.Text = "编译时间";
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
            // TimerMoveRandom
            // 
            TimerMoveRandom.Enabled = true;
            TimerMoveRandom.Interval = 150;
            TimerMoveRandom.Tick += TimerMoveRandom_Tick;
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
            Opacity = 0D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "布衣工具箱";
            FormClosed += MainForm_FormClosed;
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
        private ToolStripDropDownButton MenuTools;
        private ToolStripDropDownButton MenuAdvanced;
        private ToolStripMenuItem ButRestartAsAdmin;
        private ToolStripDropDownButton MenuAbout;
        private ToolStripMenuItem githubUrlToolStripMenuItem;
        private ToolStripMenuItem buildTimeToolStripMenuItem;
        private ToolStripMenuItem LastErrorDetailToolStripMenuItem;
        private System.Windows.Forms.Timer TimerMoveRandom;
    }
}