using SharpCompress.Compressors.BZip2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCompress.Compressors;

namespace BuyiTools.Forms.Tools
{
    public partial class FastDLCreator : ToolBase
    {
        public FastDLCreator()
        {
            InitializeComponent();
        }

        private void FastDLCreator_Load(object sender, EventArgs e)
        {
            RegisterContorlSaveData(TxtTarget, TxtMaxCompressSize);
        }

        private static readonly EnumerationOptions enumerationDirOptions = new()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System
        };

        private (DirectoryInfo, FileInfo[]) MustGetTargetFileList()
        {
            var targetPath = Utils.CleanPath(TxtTarget.Text);
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
            DoWorkAsync(() =>
            {
                var data = MustGetTargetFileList();
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

        private void ButMakeBz2_Click(object sender, EventArgs e)
        {
            DoWorkAsync(() =>
            {
                var data = MustGetTargetFileList();
                var fileCount = data.Item2.Length;
                Log($"扫描到 {fileCount} 个文件");
                var root = data.Item1;
                var rootPath = root.FullName;
                var rootParent = root.Parent;
                if (rootParent == null) { throw new Exception("无法在根目录进行操作"); }
                var outDirPath = Path.Combine(rootParent.FullName, $"{root.Name}-bz2");
                var sizeLimit = TxtMaxCompressSize.Value * 1024 * 1024;
                long totalLen = 0;
                long totalLen2 = 0;
                foreach (var rawfile in data.Item2)
                {
                    var rel = Utils.CleanPath(Path.GetRelativePath(rootPath, rawfile.FullName));
                    var outFilePath = Path.Combine(outDirPath, rel) + ".bz2";
                    var dir = Path.GetDirectoryName(outFilePath);
                    if (string.IsNullOrEmpty(dir)) { throw new Exception("无法识别输出文件夹的位置"); }
                    Directory.CreateDirectory(dir);
                    long len = rawfile.Length;
                    totalLen += len;
                    long len2 = 0;
                    if (len > sizeLimit || len < 1)
                    {
                        rawfile.CopyTo(outFilePath);
                        Log($"文件过大或空白,直接复制 {rel} {len}Bytes");
                        len2 = len;
                    }
                    else
                    {
                        using var inStream = rawfile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                        using var outStream = File.Create(outFilePath);
                        using var bz2s = new BZip2Stream(outStream, CompressionMode.Compress, false);
                        inStream.CopyTo(bz2s);
                        bz2s.Close();
                        inStream.Close();
                        outStream.Close();
                        len2 = Utils.GetFileLength(outFilePath);
                        Log($"{rel} 压缩前 {Utils.FormatBytesLength(len)} 压缩后 {Utils.FormatBytesLength(len2)}");
                    }
                    totalLen2 += len2;
                }
                Log($"压缩前 {Utils.FormatBytesLength(totalLen)} 压缩后 {Utils.FormatBytesLength(totalLen2)}");
                Log($"输出文件夹 {outDirPath}");
            });
        }
    }
}
