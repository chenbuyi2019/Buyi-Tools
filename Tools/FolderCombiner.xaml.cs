using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BuyiTools.Tools
{
    public partial class FolderCombiner : ToolPageBase
    {
        public FolderCombiner()
        {
            InitializeComponent();
        }

        private void FolderCombiner_Load(object sender, RoutedEventArgs e)
        {
            RegisterContorlSaveData(TxtFolders, TxtOutput);
            RegisterTextboxDropFilePath(TxtFolders, TxtOutput);
        }

        private async void ButWork_Click(object sender, EventArgs e)
        {
            var inputs = TxtFolders.Text.Split('\r','\n');
            var outFolderPath = Utils.CleanPath(TxtOutput.Text);
            await this.DoWorkAsync(() =>
            {
                if (string.IsNullOrEmpty(outFolderPath)) { throw new Exception("输出为空白"); }
                var outFolder = Directory.CreateDirectory(outFolderPath);
                if (outFolder == null || !outFolder.Exists) { throw new Exception($"输出文件夹不存在或无法被创建 {outFolderPath}"); }
                outFolderPath = outFolder.FullName;
                var inputFolders = new List<DirectoryInfo>();
                foreach (var rawline in inputs)
                {
                    var line = rawline.Trim();
                    if (string.IsNullOrEmpty(line)) { continue; }
                    line = Utils.CleanPath(line);
                    var info = new DirectoryInfo(line);
                    if (!info.Exists) { throw new Exception($"文件夹不存在 {info.FullName}"); }
                    inputFolders.Add(info);
                }
                if (inputFolders.Count < 1) { throw new Exception("没有要组合的文件夹"); }
                foreach (var fd in inputFolders)
                {
                    Log($"扫描 {fd.FullName}");
                    var files = fd.EnumerateFiles("*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        var relPath = Path.GetRelativePath(fd.FullName, file.FullName);
                        var targetPath = Path.Combine(outFolderPath, relPath);
                        if (File.Exists(targetPath))
                        {
                            Log($"跳过已存在的 {relPath}");
                            continue;
                        }
                        var targetFolder = Path.GetDirectoryName(targetPath);
                        if (string.IsNullOrEmpty(targetFolder)) { throw new Exception($"无法获取文件夹名 {targetPath}"); }
                        if (targetFolder.Length > 3) { Directory.CreateDirectory(targetFolder); }
                        file.CopyTo(targetPath, true);
                    }
                }
            });
        }

    }
}
