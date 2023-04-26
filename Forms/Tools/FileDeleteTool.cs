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
    public partial class FileDeleteTool : ToolBase
    {
        private static readonly string FileDeleteSetsDir = Path.Combine(AppContext.BaseDirectory, "FileDeleteSets");
        private static readonly Dictionary<string, string> FileDeleteSets = new();

        public FileDeleteTool()
        {
            InitializeComponent();
        }

        private void FileDeleteTool_Load(object sender, EventArgs e)
        {
            RegisterContorlSaveData(TxtTargetFiles, TxtWorkingDir);
            RegisterTextboxDropFilePath(TxtWorkingDir);
            var dir = new DirectoryInfo(FileDeleteSetsDir);
            var files = dir.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                if (f.Length > 0 && f.Length < 1024 * 1024 * 2)
                {
                    var txt = File.ReadAllText(f.FullName);
                    var pureName = Path.ChangeExtension(f.Name, null);
                    FileDeleteSets.Add(pureName, txt);
                    ListFileSets.Items.Add(pureName);
                }
                else
                {
                    Log($"跳过预设文件，因为文件过大 {f.FullName}");
                }
            }
        }

        private void ButStart_Click(object sender, EventArgs e)
        {
            DoWorkAsync(() =>
            {
                var scanOnly = CheckScanOnly.Checked;
                var p = Utils.CleanPath(TxtWorkingDir.Text);
                if (!Path.IsPathRooted(p)) { throw new Exception("工作文件夹路径必须是绝对路径"); }
                var dirInfo = new DirectoryInfo(p);
                if (!dirInfo.Exists) { throw new Exception($"工作文件夹不存在 {dirInfo.FullName}"); }
                var targets = new List<string>();
                ParseLines(targets, TxtTargetFiles.Text);
                foreach (var raw in ListFileSets.CheckedItems)
                {
                    var key = raw.ToString();
                    if (string.IsNullOrEmpty(key)) { continue; }
                    FileDeleteSets.TryGetValue(key, out string? txt);
                    if (string.IsNullOrWhiteSpace(txt)) { throw new Exception($"空白的预设 {key}"); }
                    ParseLines(targets, txt);
                }
                Log($"准备的需删除文件总条目 {targets.Count} 行，下面开始扫描");
                var files = dirInfo.GetFiles("*", SearchOption.AllDirectories);
                long totalSize = 0;
                var toRemove = new List<FileInfo>();
                foreach (var file in files)
                {
                    var relPath = Utils.CleanPath(Path.GetRelativePath(dirInfo.FullName, file.FullName));
                    if (targets.Contains(relPath.ToLower()))
                    {
                        toRemove.Add(file);
                        totalSize += file.Length;
                        Log(relPath);
                    }
                }
                Log($"实际需要删除 {toRemove.Count} 个文件，总计 {Utils.FormatBytesLength(totalSize)}");
                if (scanOnly)
                {
                    Log("扫描完成，不实际删除");
                }
                else
                {
                    Log($"下面开始实际删除");
                    foreach (var file in toRemove)
                    {
                        file.Delete();
                    }
                    Log("删除完毕");
                }
            });
        }

        private static void ParseLines(List<string> list, string str)
        {
            var lines = str.Split('\n', '\r');
            foreach (var rawline in lines)
            {
                var line = Utils.CleanPath(rawline).ToLower();
                if (line.Length > 0) { list.Add(line); }
            }
        }

    }
}
