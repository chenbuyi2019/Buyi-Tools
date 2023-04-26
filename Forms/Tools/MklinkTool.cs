
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
                foreach (var item in files)
                {
                    ListFiles.Items.Add(item.Name);
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
                    if (item == null) { throw new Exception("null 1，请刷新列表后再试"); }
                    var n = item.ToString();
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
                if (usedStr.Count < 1) { throw new Exception("没有任何要建立链接的文件或文件夹"); }
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
                if (targets.Count < 1) { throw new Exception("没有目标文件夹，请一行写一个"); }
                foreach (var dir in targets)
                {
                    foreach (var f in srcFiles)
                    {
                        var targetPath = Path.Combine(dir.FullName, f.Name);
                        if (File.Exists(targetPath))
                        {
                            Log($"文件已经存在 跳过 {targetPath}");
                            continue;
                        }
                        File.CreateSymbolicLink(targetPath, f.FullName);
                        Log($"成功建立 {targetPath}");
                    }
                    foreach (var f in srcDirs)
                    {
                        var targetPath = Path.Combine(dir.FullName, f.Name);
                        if (Directory.Exists(targetPath))
                        {
                            Log($"文件已经存在 跳过 {targetPath}");
                            continue;
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
}
