using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BuyiTools.Tools
{
    /// <summary>
    /// Interaction logic for VtfCompress.xaml
    /// </summary>
    public partial class VtfCompress : ToolPageBase
    {
        public VtfCompress()
        {
            InitializeComponent();
            ListImageWidthLimit.Items.Add(64);
            ListImageWidthLimit.Items.Add(128);
            ListImageWidthLimit.Items.Add(256);
            ListImageWidthLimit.Items.Add(512);
            ListImageWidthLimit.Items.Add(1024);
            ListImageWidthLimit.Items.Add(2048);
            ListImageWidthLimit.Items.Add(4096);
            ListImageWidthLimit.Items.Add("不修改");
        }

        private static readonly string vtfCmdPath = Path.Combine(AppContext.BaseDirectory, "lib", "vtfcmd", "vtfcmd.exe");

        private void VtfCompress_Load(object sender, RoutedEventArgs e)
        {
            ListImageWidthLimit.SelectedIndex = 0;
            this.RegisterContorlSaveData(TxtFolder, TxtFileSizeLimit, ListImageWidthLimit);
            RegisterTextboxDropFilePath(TxtFolder);
        }

        private void ButStart_Click(object sender, EventArgs e)
        {
            var inputPath = Utils.CleanPath(TxtFolder.Text);
            if (string.IsNullOrEmpty(inputPath)) { return; }
            var txtWidthLimit = ListImageWidthLimit.SelectedItem.ToString();
            string[] imageWidthLimitArgs = Array.Empty<string>();
            if (!string.IsNullOrEmpty(txtWidthLimit))
            {
                if (int.TryParse(txtWidthLimit, out int num) && num > 0)
                {
                    var n = num.ToString();
                    imageWidthLimitArgs = new string[] { "-resize", "-rclampwidth", n, "-rclampheight", n };
                }
            }
            var fileSizeLimit = Convert.ToInt64(TxtFileSizeLimit.Value * 1024 * 1024);
            if (fileSizeLimit < 1) { fileSizeLimit = 0; }
            this.DoWorkAsync(() =>
            {
                if (!File.Exists(vtfCmdPath))
                {
                    throw new Exception("依赖的程序不存在 " + vtfCmdPath);
                }
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var files = new Dictionary<FileInfo, string>();
                var fileInfo = new FileInfo(inputPath);
                if (fileInfo.Exists)
                {
                    files.Add(fileInfo, "");
                }
                else
                {
                    var dir = new DirectoryInfo(inputPath);
                    if (!dir.Exists)
                    {
                        throw new Exception("路径不存在 " + dir.FullName);
                    }
                    var subfiles = dir.EnumerateFiles("*.vtf", SearchOption.AllDirectories);
                    foreach (var item in subfiles)
                    {
                        if (item == null) { continue; }
                        var relPath = item.DirectoryName;
                        if (string.IsNullOrEmpty(relPath)) { throw new Exception("无法获取路径"); }
                        relPath = Path.GetRelativePath(dir.FullName, relPath);
                        files.Add(item, relPath);
                    }
                }
                if (files.Count < 1) { throw new Exception("找不到任何vtf文件，不论大小"); }
                long totalSize = 0;
                var toRemove = new List<FileInfo>();
                foreach (var f in files.Keys)
                {
                    if (f.Length < fileSizeLimit)
                    {
                        toRemove.Add(f);
                        Log("跳过较小的文件 " + f.FullName);
                        continue;
                    }
                    totalSize += f.Length;
                }
                foreach (var f in toRemove)
                {
                    files.Remove(f);
                }
                Log($"找到了 {files.Count} 个超过 {Utils.FormatBytesLength(fileSizeLimit)} 的 vtf 文件");
                if (files.Count < 1)
                {
                    Log("工作结束");
                    return;
                }
                var outputRoot = Path.Combine(desktop, $"vtf_{DateTime.Now:HH-mm-ss}");
                Log($"输出文件夹 " + outputRoot);
                long totalSize2 = 0;
                var realCompress = 0;
                foreach (var kv in files)
                {
                    var vtf = kv.Key;
                    var relPath = kv.Value;
                    var outputFolder = outputRoot;
                    if (!string.IsNullOrEmpty(relPath))
                    {
                        outputFolder = Path.Combine(outputFolder, relPath);
                    }
                    Directory.CreateDirectory(outputFolder);
                    var outTga = Path.Combine(outputFolder, Path.ChangeExtension(vtf.Name, ".tga"));
                    Log($"处理 {vtf.FullName}");
                    var args = new List<string>() { "-file", vtf.FullName, "-output", outputFolder, "-exportformat", "tga" };
                    args.AddRange(imageWidthLimitArgs);
                    var msg = Utils.RunCmd(vtfCmdPath, args, null, 1000 * 10);
                    if (!File.Exists(outTga))
                    {
                        Log(msg[0]);
                        Log(msg[1]);
                        throw new Exception("无法生成tga");
                    }
                    var outVtf = Path.ChangeExtension(outTga, ".vtf");
                    args = new List<string>() { "-file", outTga, "-output", outputFolder, "-format", "DXT1", "-alphaformat", "DXT5" };
                    args.AddRange(imageWidthLimitArgs);
                    msg = Utils.RunCmd(vtfCmdPath, args, null, 1000 * 10);
                    var outVtfInfo = new FileInfo(outVtf);
                    if (!outVtfInfo.Exists)
                    {
                        Log(msg[0]);
                        Log(msg[1]);
                        throw new Exception("无法生成vtf");
                    }
                    totalSize2 += outVtfInfo.Length;
                    File.Delete(outTga);
                    realCompress += 1;
                }
                Log($"完成 压缩前:{Utils.FormatBytesLength(totalSize)}  压缩后:{Utils.FormatBytesLength(totalSize2)}");
            });
        }

    }
}
