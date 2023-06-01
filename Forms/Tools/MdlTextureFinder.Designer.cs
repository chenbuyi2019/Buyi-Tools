namespace BuyiTools.Forms.Tools
{
    partial class MdlTextureFinder
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
            TxtMdlFiles = new TextBox();
            label2 = new Label();
            TxtMaterialsFolder = new TextBox();
            ButPackMaterial = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 10);
            label1.Name = "label1";
            label1.Size = new Size(108, 19);
            label1.TabIndex = 0;
            label1.Text = "MDL 文件列表：";
            // 
            // TxtMdlFiles
            // 
            TxtMdlFiles.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            TxtMdlFiles.Location = new Point(13, 32);
            TxtMdlFiles.MaxLength = 9999;
            TxtMdlFiles.Multiline = true;
            TxtMdlFiles.Name = "TxtMdlFiles";
            TxtMdlFiles.ScrollBars = ScrollBars.Both;
            TxtMdlFiles.Size = new Size(798, 103);
            TxtMdlFiles.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 138);
            label2.Name = "label2";
            label2.Size = new Size(120, 19);
            label2.TabIndex = 2;
            label2.Text = "materials 文件夹：";
            // 
            // TxtMaterialsFolder
            // 
            TxtMaterialsFolder.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            TxtMaterialsFolder.Location = new Point(15, 165);
            TxtMaterialsFolder.MaxLength = 999;
            TxtMaterialsFolder.Name = "TxtMaterialsFolder";
            TxtMaterialsFolder.Size = new Size(796, 22);
            TxtMaterialsFolder.TabIndex = 3;
            // 
            // ButPackMaterial
            // 
            ButPackMaterial.Location = new Point(16, 202);
            ButPackMaterial.Name = "ButPackMaterial";
            ButPackMaterial.Size = new Size(140, 35);
            ButPackMaterial.TabIndex = 4;
            ButPackMaterial.Text = "打包它们的贴图";
            ButPackMaterial.UseVisualStyleBackColor = true;
            ButPackMaterial.Click += ButPackMaterial_Click;
            // 
            // MdlTextureFinder
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(ButPackMaterial);
            Controls.Add(TxtMaterialsFolder);
            Controls.Add(label2);
            Controls.Add(TxtMdlFiles);
            Controls.Add(label1);
            Name = "MdlTextureFinder";
            Size = new Size(824, 293);
            Load += MdlTextureFinder_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox TxtMdlFiles;
        private Label label2;
        private TextBox TxtMaterialsFolder;
        private Button ButPackMaterial;
    }
}
