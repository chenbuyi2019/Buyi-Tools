using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BuyiTools.Tools
{
    /// <summary>
    /// Interaction logic for FastDLDownloader.xaml
    /// </summary>
    public partial class FastDLDownloader : ToolPageBase
    {
        public FastDLDownloader()
        {
            InitializeComponent();
        }

        private void FastDLDownloader_Load(object sender, RoutedEventArgs e)
        {
            RegisterContorlSaveData(TxtUserAgent, TxtLinks, TxtHost);
        }

        private void ButUseDefaultUA_Click(object sender, EventArgs e)
        {
            TxtUserAgent.Text = "Half-Life 2";
        }

        private void ButStartDownload_Click(object sender, EventArgs e)
        {
            var host = TxtHost.Text.Trim();
            var rawLinks = TxtLinks.Text.Split('\r', '\n');
            var ua = TxtUserAgent.Text.Trim();
            DoWorkAsync(() =>
            {
                if (!host.StartsWith("http")) { throw new Exception("前缀不是 http 开头"); }
                if (rawLinks.Length < 1) { throw new Exception("没有要下载的文件"); }
                host = host.TrimEnd('\\', '/');
                var httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 15) };
                var hd = httpClient.DefaultRequestHeaders;
                hd.UserAgent.Clear();
                if (ua.Length > 0)
                {
                    hd.UserAgent.ParseAdd(ua);
                }
                var outputFdName = $"fastdl-{DateTime.Now:HH-mm-ss}";
                var outputFd = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputFdName));
                if (outputFd == null)
                {
                    throw new Exception($"无法创建输出文件夹 {outputFdName}");
                }
                Log($"输出文件夹在桌面的 {outputFd.Name}");
                var links = new List<string>();
                foreach (var rawlink in rawLinks)
                {
                    var link = rawlink.Trim().TrimStart('\\', '/');
                    if (link.Length < 1) { continue; }
                    if (links.Contains(link)) { continue; }
                    links.Add(link);
                }
                if (links.Count < 1) { throw new Exception("没有需要下载的文件"); }
                var failed = new Dictionary<string, string>();
                foreach (var link in links)
                {
                    var url = $"{host}/{link}";
                    var outFile = Path.Combine(outputFd.FullName, link);
                    var dir = Path.GetDirectoryName(outFile);
                    if (string.IsNullOrEmpty(dir)) { throw new Exception($"无法获取输出文件夹路径 {outFile}"); }
                    try
                    {
                        var bz2url = url + ".bz2";
                        Log($"尝试 {bz2url}");
                        using var httpStream = httpClient.GetStreamAsync(bz2url).Result;
                        Directory.CreateDirectory(dir);
                        var outfilebz2 = outFile + ".bz2";
                        using var fileStream = File.Create(outfilebz2);
                        httpStream.CopyTo(fileStream);
                        fileStream.Close();
                        httpStream.Close();
                        Utils.Run7zCmd("e", outfilebz2, "-o" + dir.Replace('\\', '/'), "-y");
                        File.Delete(outfilebz2);
                        var info = new FileInfo(outFile);
                        if (!info.Exists)
                        {
                            throw new Exception($"无法下载和解压");
                        }
                        Log($"成功 {Utils.FormatBytesLength(info.Length)} {link}");
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Log($"出错 {link} {ex.Message}");
                    }
                    try
                    {
                        Log($"尝试 {url}");
                        using var httpStream = httpClient.GetStreamAsync(url).Result;
                        Directory.CreateDirectory(dir);
                        using var fileStream = File.Create(outFile);
                        httpStream.CopyTo(fileStream);
                        Log($"成功 {Utils.FormatBytesLength(fileStream.Position)} {link}");
                    }
                    catch (Exception ex)
                    {
                        Log($"出错 {link} {ex.Message}");
                        failed.Add(link, ex.Message);
                    }
                }
                if (failed.Count > 0)
                {
                    Log($"一共 {failed.Count} 个下载失败");
                    foreach (var item in failed)
                    {
                        Log($"{item.Key}  {item.Value}");
                    }
                }
                else
                {
                    Log($"无下载失败");
                }
                Log($"输出文件夹在桌面的 {outputFd.Name}");
            });
        }

    }
}
