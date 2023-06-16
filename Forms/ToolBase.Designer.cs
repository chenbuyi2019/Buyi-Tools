namespace BuyiTools
{
    partial class ToolBase
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
            components = new System.ComponentModel.Container();
            MenuCheckBoxList = new ContextMenuStrip(components);
            CheckAllToolStripMenuItem = new ToolStripMenuItem();
            UncheckAllToolStripMenuItem = new ToolStripMenuItem();
            CheckInvertToolStripMenuItem = new ToolStripMenuItem();
            ViewCheckedToolStripMenuItem = new ToolStripMenuItem();
            MenuCheckBoxList.SuspendLayout();
            SuspendLayout();
            // 
            // MenuCheckBoxList
            // 
            MenuCheckBoxList.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            MenuCheckBoxList.Items.AddRange(new ToolStripItem[] { CheckAllToolStripMenuItem, UncheckAllToolStripMenuItem, CheckInvertToolStripMenuItem, ViewCheckedToolStripMenuItem });
            MenuCheckBoxList.Name = "MenuCheckBoxList";
            MenuCheckBoxList.Size = new Size(173, 92);
            // 
            // CheckAllToolStripMenuItem
            // 
            CheckAllToolStripMenuItem.Name = "CheckAllToolStripMenuItem";
            CheckAllToolStripMenuItem.Size = new Size(172, 22);
            CheckAllToolStripMenuItem.Text = "全打勾";
            CheckAllToolStripMenuItem.Click += CheckAllToolStripMenuItem_Click;
            // 
            // UncheckAllToolStripMenuItem
            // 
            UncheckAllToolStripMenuItem.Name = "UncheckAllToolStripMenuItem";
            UncheckAllToolStripMenuItem.Size = new Size(172, 22);
            UncheckAllToolStripMenuItem.Text = "全不打勾";
            UncheckAllToolStripMenuItem.Click += UncheckAllToolStripMenuItem_Click;
            // 
            // CheckInvertToolStripMenuItem
            // 
            CheckInvertToolStripMenuItem.Name = "CheckInvertToolStripMenuItem";
            CheckInvertToolStripMenuItem.Size = new Size(172, 22);
            CheckInvertToolStripMenuItem.Text = "反打勾";
            CheckInvertToolStripMenuItem.Click += CheckInvertToolStripMenuItem_Click;
            // 
            // ViewCheckedToolStripMenuItem
            // 
            ViewCheckedToolStripMenuItem.Name = "ViewCheckedToolStripMenuItem";
            ViewCheckedToolStripMenuItem.Size = new Size(172, 22);
            ViewCheckedToolStripMenuItem.Text = "查看已勾选的项目";
            ViewCheckedToolStripMenuItem.Click += ViewCheckedToolStripMenuItem_Click;
            // 
            // ToolBase
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(248, 248, 248);
            DoubleBuffered = true;
            Font = new Font("Microsoft YaHei UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "ToolBase";
            Size = new Size(289, 138);
            MenuCheckBoxList.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ToolStripMenuItem CheckAllToolStripMenuItem;
        private ToolStripMenuItem UncheckAllToolStripMenuItem;
        private ToolStripMenuItem CheckInvertToolStripMenuItem;
        private ToolStripMenuItem ViewCheckedToolStripMenuItem;
        private ContextMenuStrip MenuCheckBoxList;
    }
}
