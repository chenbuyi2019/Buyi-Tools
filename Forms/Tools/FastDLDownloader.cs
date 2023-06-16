using SharpCompress.Compressors.BZip2;
using SharpCompress.Compressors;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuyiTools.Forms.Tools
{
    public partial class FastDLDownloader : ToolBase
    {
        public FastDLDownloader()
        {
            InitializeComponent();
        }

        private void FastDLDownloader_Load(object sender, EventArgs e)
        {
            TextBox[] textboxs = { TxtUserAgent, TxtLinks, TxtHost };
            var monoFont = new Font("Consolas", 10.0f);
            foreach (var t in textboxs)
            {
                t.Font = monoFont;
                RegisterContorlSaveData(t);
            }
        }

        private void ButUseDefaultUA_Click(object sender, EventArgs e)
        {
            TxtUserAgent.Text = "Half-Life 2";
        }

        private void ButStartDownload_Click(object sender, EventArgs e)
        {
            var host = TxtHost.Text.Trim();
            var rawLinks = TxtLinks.Lines;
            var ua = TxtUserAgent.Text.Trim();
            DoWorkAsync(() =>
            {
                if (host.Length < 1) { throw new Exception("前缀是空白"); }
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
                SetFullProgress(links.Count);
                var failed = new Dictionary<string, string>();
                foreach (var link in links)
                {
                    AddProgress(1);
                    var url = $"{host}/{link}";
                    var outFile = Path.Combine(outputFd.FullName, link);
                    var dir = Path.GetDirectoryName(outFile);
                    if (string.IsNullOrEmpty(dir)) { throw new Exception($"无法获取输出文件夹路径 {outFile}"); }
                    try
                    {
                        var bz2url = url + ".bz2";
                        Log($"尝试 {bz2url}");
                        using var httpStream = httpClient.GetStreamAsync(bz2url).Result;
                        using var bz2Stream = new BZip2Stream(httpStream, CompressionMode.Decompress, true);
                        Directory.CreateDirectory(dir);
                        using var fileStream = File.Create(outFile);
                        bz2Stream.CopyTo(fileStream);
                        Log($"成功 {Utils.FormatBytesLength(fileStream.Position)} {link}");
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
