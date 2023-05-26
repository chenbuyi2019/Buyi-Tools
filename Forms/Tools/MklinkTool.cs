
using System.Text;

namespace BuyiTools.Forms.Tools
{
    public partial class MklinkTool : ToolBase
    {
        public MklinkTool()
        {
            InitializeComponent();
        }

        private void MklinkTool_Load(object sender, EventArgs e)
        {
            RegisterContorlSaveData(TxtParentFolder, TxtTargetFolders);
            RegisterTextboxDropFilePath(TxtParentFolder, TxtTargetFolders);
        }

        private void TxtParentFolder_TextChanged(object sender, EventArgs e)
        {
            ButRefreshFileList_Click(sender, e);
        }

        private void ButRefreshFileList_Click(object sender, EventArgs e)
        {
            try
            {
                ListFiles.Items.Clear();
                var t = Utils.CleanPath(TxtParentFolder.Text);
                if (!Path.IsPathRooted(t))
                {
                    throw new Exception("请写绝对路径");
                }
                var dir = new DirectoryInfo(t);
                var files = dir.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
                var sb = new StringBuilder();
                foreach (var item in files)
                {
                    sb.Clear();
                    if (item.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        sb.Append(" 目录");
                    }
                    else
                    {
                        sb.Append(" 文件");
                    }
                    if (item.LinkTarget != null)
                    {
                        sb.Append(" 链接");
                    }
                    if (item.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        sb.Append(" 隐藏");
                    }
                    if (item.Attributes.HasFlag(FileAttributes.ReadOnly))
                    {
                        sb.Append(" 只读");
                    }
                    if (item.Attributes.HasFlag(FileAttributes.System))
                    {
                        sb.Append(" 系统");
                    }
                    var m = new SimpleDataObject(item.Name, $"{item.Name}  [{sb.ToString().Trim()}]");
                    ListFiles.Items.Add(m);
                }
            }
            catch (Exception ex)
            {
                Log($"扫描出错 {ex.Message}");
            }
            ListFiles_SelectedIndexChanged(sender, e);
        }

        private void ListFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            labSelectedCount.Text = $"已勾选: {ListFiles.CheckedIndices.Count} / {ListFiles.Items.Count} ";
        }

        private async void ButCreate_Click(object sender, EventArgs e)
        {
            var ex = await DoWorkAsync(() =>
            {
                var parentPath = Utils.CleanPath(TxtParentFolder.Text);
                if (!Path.IsPathRooted(parentPath))
                {
                    throw new Exception("母体文件夹应该是绝对路径");
                }
                var parentDir = new DirectoryInfo(parentPath);
                if (!parentDir.Exists)
                {
                    throw new Exception("母体文件夹不存在");
                }
                var srcFiles = new List<FileInfo>();
                var srcDirs = new List<DirectoryInfo>();
                var usedStr = new List<string>();
                foreach (var item in ListFiles.CheckedItems)
                {
                    if (item == null || item is not SimpleDataObject) { throw new Exception("null 1，请刷新列表后再试"); }
                    var dataobj = (SimpleDataObject)item;
                    var n = dataobj.Data;
                    if (string.IsNullOrWhiteSpace(n)) { throw new Exception("null 2，请刷新列表后再试"); }
                    var n2 = n.ToLower();
                    if (usedStr.Contains(n2)) { throw new Exception($"重复的文件名 {n}"); }
                    var p = Path.Combine(parentDir.FullName, n);
                    var info = new FileInfo(p);
                    if (info.Exists)
                    {
                        srcFiles.Add(info);
                    }
                    else
                    {
                        var dir = new DirectoryInfo(p);
                        if (dir.Exists)
                        {
                            srcDirs.Add(dir);
                        }
                        else
                        {
                            throw new Exception($"文件或文件夹不存在，请刷新列表 {p}");
                        }
                    }
                    usedStr.Add(n2);
                }
                var totalCount = usedStr.Count;
                if (totalCount < 1) { throw new Exception("没有任何要建立链接的文件或文件夹"); }
                usedStr.Clear();
                var targetLines = TxtTargetFolders.Lines;
                var targets = new List<DirectoryInfo>();
                foreach (var rawline in targetLines)
                {
                    var line = Utils.CleanPath(rawline);
                    if (line.Length < 1) { continue; }
                    if (!Path.IsPathRooted(line)) { throw new Exception($"必须写完整路径，不能使用相对路径 {line}"); }
                    var dir = new DirectoryInfo(line);
                    if (!dir.Exists)
                    {
                        throw new Exception($"目标文件夹不存在 {line}");
                    }
                    var preview = dir.FullName.ToLower();
                    if (usedStr.Contains(preview)) { throw new Exception($"目标文件夹重复 {preview}"); }
                    if (preview.Equals(parentDir.FullName, Utils.IgnoreCase)) { throw new Exception("目标文件夹和母体重复"); }
                    targets.Add(dir);
                    usedStr.Add(preview);
                }
                totalCount *= targets.Count;
                if (targets.Count < 1) { throw new Exception("没有目标文件夹，请一行写一个"); }
                SetFullProgress(totalCount);
                bool forceOverwrite = checkForceRemoveTargets.Checked;
                foreach (var dir in targets)
                {
                    foreach (var f in srcFiles)
                    {
                        AddProgress();
                        var targetPath = Path.Combine(dir.FullName, f.Name);
                        if (File.Exists(targetPath))
                        {
                            if (forceOverwrite)
                            {
                                File.Delete(targetPath);
                                Log($"删除已存在的文件 {targetPath}");
                            }
                            else
                            {
                                Log($"文件已经存在 跳过 {targetPath}");
                                continue;
                            }
                        }
                        File.CreateSymbolicLink(targetPath, f.FullName);
                        Log($"成功建立 {targetPath}");
                    }
                    foreach (var f in srcDirs)
                    {
                        AddProgress();
                        var targetPath = Path.Combine(dir.FullName, f.Name);
                        if (Directory.Exists(targetPath))
                        {
                            if (forceOverwrite)
                            {
                                Directory.Delete(targetPath, true);
                                Log($"删除已存在的文件夹 {targetPath}");
                            }
                            else
                            {
                                Log($"文件夹已经存在 跳过 {targetPath}");
                                continue;
                            }
                        }
                        Directory.CreateSymbolicLink(targetPath, f.FullName);
                        Log($"成功建立 {targetPath}");
                    }
                }
            });
            if (ex != null && ex.Source != null && ex.Source.Contains("System.Private.CoreLib", Utils.IgnoreCase))
            {
                Log($"请尝试用管理员权限运行本软件 {ex.Source}");
            }
        }

    }

    public class SimpleDataObject
    {
        public SimpleDataObject(string data, string display)
        {
            this.Data = data;
            this._display = display;
        }

        public string Data { get; }
        private readonly string _display;

        public override string ToString()
        {
            return this._display;
        }
    }
}
