namespace BuyiTools.Forms.Tools
{
    partial class OpenHiddenProcess
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
            TxtTargetName = new TextBox();
            ButShowThem = new Button();
            ButHideThem = new Button();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 16);
            label1.Name = "label1";
            label1.Size = new Size(87, 19);
            label1.TabIndex = 0;
            label1.Text = "目标进程名：";
            // 
            // TxtTargetName
            // 
            TxtTargetName.Location = new Point(16, 38);
            TxtTargetName.Name = "TxtTargetName";
            TxtTargetName.Size = new Size(300, 24);
            TxtTargetName.TabIndex = 1;
            TxtTargetName.Text = "srcds";
            // 
            // ButShowThem
            // 
            ButShowThem.Location = new Point(16, 76);
            ButShowThem.Name = "ButShowThem";
            ButShowThem.Size = new Size(111, 30);
            ButShowThem.TabIndex = 2;
            ButShowThem.Text = "显示它们";
            ButShowThem.UseVisualStyleBackColor = true;
            ButShowThem.Click += ButShowThem_Click;
            // 
            // ButHideThem
            // 
            ButHideThem.Location = new Point(166, 76);
            ButHideThem.Name = "ButHideThem";
            ButHideThem.Size = new Size(111, 30);
            ButHideThem.TabIndex = 3;
            ButHideThem.Text = "隐藏它们";
            ButHideThem.UseVisualStyleBackColor = true;
            ButHideThem.Click += ButHideThem_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 116);
            label2.Name = "label2";
            label2.Size = new Size(334, 57);
            label2.TabIndex = 4;
            label2.Text = "说明：\r\n根据进程的名字，把所有同名进程的主窗口隐藏或显示。\r\n目标的名字不要写后缀名 .exe";
            // 
            // OpenHiddenProcess
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(label2);
            Controls.Add(ButHideThem);
            Controls.Add(ButShowThem);
            Controls.Add(TxtTargetName);
            Controls.Add(label1);
            Name = "OpenHiddenProcess";
            Size = new Size(454, 183);
            Load += OpenHiddenSrcds_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox TxtTargetName;
        private Button ButShowThem;
        private Button ButHideThem;
        private Label label2;
    }
}
