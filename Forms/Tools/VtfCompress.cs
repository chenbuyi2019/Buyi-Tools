using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuyiTools.Forms.Tools
{
    public partial class VtfCompress : ToolBase
    {
        private static readonly string vtfCmdPath = Path.Combine(AppContext.BaseDirectory, "vtfcmd.exe");

        public VtfCompress()
        {
            InitializeComponent();
        }

        private void VtfCompress_Load(object sender, EventArgs e)
        {
            this.RegisterContorlSaveData(TxtFolder, TxtFileSizeLimit);
            RegisterTextboxDropFilePath(TxtFolder);
        }

        private static string CompressVtf(FileInfo vtf, string output)
        {
            if (!vtf.Exists) { throw new Exception("文件不存在 " + vtf.FullName); }
            if (vtf.Extension.Equals(".vtf", Utils.IgnoreCase))
            {
                throw new Exception("文件不是vtf格式 " + vtf.FullName);
            }
            if (vtf.Length < 10) { throw new Exception("文件损坏 " + vtf.FullName); }
            Directory.CreateDirectory(output);
            var outFile = Path.Combine(output, vtf.Name);
            return outFile;
        }

        private void ButStart_Click(object sender, EventArgs e)
        {
            var inputPath = Utils.CleanPath(TxtFolder.Text);
            if (string.IsNullOrEmpty(inputPath)) { return; }
            this.DoWorkAsync(() =>
            {
                if (!File.Exists(vtfCmdPath))
                {
                    throw new Exception("依赖的程序不存在 " + vtfCmdPath);
                }
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileInfo = new FileInfo(inputPath);
                if (fileInfo.Exists)
                {
                    var outFile = CompressVtf(fileInfo, desktop);
                    Log("已经输出到: " + outFile);
                }
            });
        }

        private void ButDownloadVtfcmd_Click(object sender, EventArgs e)
        {
            using var proc = Process.Start("explorer.exe", "https://web.archive.org/web/20191223154323if_/http://nemesis.thewavelength.net/files/files/vtflib132-bin.zip");
        }
    }
}
