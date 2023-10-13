using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BuyiTools.Tools
{
    public partial class FastDLCreator : ToolPageBase
    {
        public FastDLCreator()
        {
            InitializeComponent();
        }

        private void FastDLCreator_Load(object sender, RoutedEventArgs e)
        {
            RegisterContorlSaveData(TxtTarget, TxtMaxCompressSize);
            RegisterTextboxDropFilePath(TxtTarget);
        }

        private static readonly EnumerationOptions enumerationDirOptions = new()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System
        };

        private (DirectoryInfo, FileInfo[]) MustGetTargetFileList(string targetPath)
        {
            targetPath = Utils.CleanPath(targetPath);
            if (targetPath.Length < 1 || !Path.IsPathRooted(targetPath)) { throw new Exception("只能识别绝对路径"); }
            var dirInfo = new DirectoryInfo(targetPath);
            if (dirInfo.Exists)
            {
                var array = dirInfo.GetFiles("*", enumerationDirOptions);
                if (array.Length < 1) { throw new Exception("目标文件夹是空的"); }
                return (dirInfo, array);
            }
            throw new Exception("目标路径不存在文件夹");
        }

        private void ButCopyPaths_Click(object sender, EventArgs e)
        {
            var targetPath = TxtTarget.Text;
            DoWorkAsync(() =>
            {
                var data = MustGetTargetFileList(targetPath);
                var fileCount = data.Item2.Length;
                Log($"扫描到 {fileCount} 个文件");
                var sb = new StringBuilder();
                var rootPath = data.Item1.FullName;
                foreach (var file in data.Item2)
                {
                    var rel = Utils.CleanPath(Path.GetRelativePath(rootPath, file.FullName));
                    sb.AppendLine(rel);
                }
                this.Invoke(() =>
                {
                    Clipboard.SetText(sb.ToString());
                    Log("已将结果放入剪贴板中");
                });
            });
        }

        private async void ButRenameLower_Click(object sender, EventArgs e)
        {
            await RenameFiles(false);
        }

        private async void ButRenameUpper_Click(object sender, EventArgs e)
        {
            await RenameFiles(true);
        }

        private async Task RenameFiles(bool upperCase)
        {
            var q = upperCase ? "大写" : "小写";
            var window = this.Window;
            if (window == null) { return; }
            var r = await window.ShowMessageAsync("请问", $"你确定要全部改成{q}文件名吗？ \n此操作无法撤回", MessageDialogStyle.AffirmativeAndNegative);
            if (r != MessageDialogResult.Affirmative)
            {
                Log("取消重命名操作");
                return;
            }
            var targetPath = TxtTarget.Text;
            await DoWorkAsync(() =>
            {
                var data = MustGetTargetFileList(targetPath);
                var fileCount = data.Item2.Length;
                Log($"扫描到 {fileCount} 个文件");
                foreach (var file in data.Item2)
                {
                    var n1 = file.Name;
                    var n = upperCase ? n1.ToUpper() : n1.ToLower();
                    if (!n.Equals(n1))
                    {
                        var dir = file.DirectoryName;
                        if (string.IsNullOrEmpty(dir))
                        {
                            throw new Exception($"无法定位重命名路径 {file.FullName}");
                        }
                        file.MoveTo(Path.Combine(dir, n));
                        Log($"重命名了 {file.FullName}");
                    }
                }
                Log($"扫描和处理文件夹...");
                while (true)
                {
                    var nochange = true;
                    var dirs = data.Item1.EnumerateDirectories("*", enumerationDirOptions);
                    foreach (var d in dirs)
                    {
                        var n1 = d.Name;
                        var n = upperCase ? n1.ToUpper() : n1.ToLower();
                        if (!n.Equals(n1))
                        {
                            var parent = d.Parent;
                            if (parent == null) { continue; }
                            var dir = parent.FullName;
                            var middle = string.Empty;
                            for (uint i = 0; i < 999999; i++)
                            {
                                middle = Path.Combine(dir, $"{i}{n}");
                                if (Directory.Exists(middle) || File.Exists(middle)) { continue; }
                                break;
                            }
                            d.MoveTo(middle);
                            var final = Path.Combine(dir, n);
                            d.MoveTo(final);
                            Log($"重命名了 {d.FullName}");
                            nochange = false;
                            break;
                        }
                    }
                    if (nochange) { break; }
                }
            });
        }

        private void ButMakeBz2_Click(object sender, EventArgs e)
        {
            var targetPath = TxtTarget.Text;
            var sizeLimit = TxtMaxCompressSize.Value * 1024 * 1024;
            DoWorkAsync(() =>
            {
                var data = MustGetTargetFileList(targetPath);
                var fileCount = data.Item2.Length;
                Log($"扫描到 {fileCount} 个文件");
                var root = data.Item1;
                var rootPath = root.FullName;
                var rootParent = root.Parent;
                if (rootParent == null) { throw new Exception("无法在根目录进行操作"); }
                var outDirPath = Path.Combine(rootParent.FullName, $"{root.Name}-bz2");
                long totalLen = 0;
                long totalLen2 = 0;
                foreach (var rawfile in data.Item2)
                {
                    var rel = Utils.CleanPath(Path.GetRelativePath(rootPath, rawfile.FullName));
                    var outFilePath = Path.Combine(outDirPath, rel);
                    var dir = Path.GetDirectoryName(outFilePath);
                    if (string.IsNullOrEmpty(dir)) { throw new Exception("无法识别输出文件夹的位置"); }
                    Directory.CreateDirectory(dir);
                    if (File.Exists(outFilePath))
                    {
                        File.Delete(outFilePath);
                    }
                    long len = rawfile.Length;
                    totalLen += len;
                    long len2 = 0;
                    if (len > sizeLimit || len < 1)
                    {
                        Log($"文件过大或空白,直接复制 {rel} {Utils.FormatBytesLength(len)}");
                        rawfile.CopyTo(outFilePath, true);
                        len2 = len;
                    }
                    else
                    {
                        outFilePath += ".bz2";
                        if (File.Exists(outFilePath))
                        {
                            File.Delete(outFilePath);
                        }
                        Utils.Run7zCmd("a", outFilePath, rawfile.FullName);
                        len2 = Utils.GetFileLength(outFilePath);
                        Log($"{rel} 压缩前 {Utils.FormatBytesLength(len)} 压缩后 {Utils.FormatBytesLength(len2)}");
                    }
                    totalLen2 += len2;
                }
                Log($"总计 压缩前 {Utils.FormatBytesLength(totalLen)} 压缩后 {Utils.FormatBytesLength(totalLen2)}");
                Log($"输出文件夹 {outDirPath}");
            });
        }

    }
}
