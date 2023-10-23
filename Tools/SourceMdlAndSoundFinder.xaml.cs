using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace BuyiTools.Tools
{

    public partial class SourceMdlAndSoundFinder : ToolPageBase
    {
        public SourceMdlAndSoundFinder()
        {
            InitializeComponent();
        }

        private void ToolPageBase_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterContorlSaveData(txtCodeFolder, txtGameFolder);
            RegisterTextboxDropFilePath(txtCodeFolder, txtGameFolder);
        }

        private void ButWork_Click(object sender, RoutedEventArgs e)
        {
            var codeFolder = Utils.CleanPath(txtCodeFolder.Text);
            var gameFolder = Utils.CleanPath(txtGameFolder.Text);
            DoWorkAsync(() =>
            {
                if (File.Exists(codeFolder))
                {
                    codeFolder = Path.GetDirectoryName(codeFolder);
                    if (string.IsNullOrEmpty(codeFolder))
                    {
                        throw new Exception("无法确定代码文件夹");
                    }
                }
                var codeDir = new DirectoryInfo(codeFolder);
                if (!codeDir.Exists)
                {
                    throw new Exception("代码文件夹不存在");
                }
                var gameDir = new DirectoryInfo(gameFolder);
                if (!gameDir.Exists)
                {
                    throw new Exception("资源文件夹不存在");
                }
                var allFiles = codeDir.EnumerateFiles("*.*", SearchOption.AllDirectories);
                var goodExts = new string[] { ".lua", ".txt", ".h", ".c", ".sp" };
                var allTargets = new List<string>();
                foreach (var codeFile in allFiles)
                {
                    if (!goodExts.Contains(codeFile.Extension.ToLower()))
                    {
                        continue;
                    }
                    Log($"扫描代码文件 {Path.GetRelativePath(codeFolder, codeFile.FullName)}");
                    var ls = ScanCodeFile(codeFile.FullName);
                    foreach (var item in ls)
                    {
                        if (allTargets.Contains(item)) { continue; }
                        allTargets.Add(item);
                    }
                }
                Log($"代码扫描完成，一共有 {allTargets.Count} 个文件名");
                var targetPath = $"scan_{DateTime.Now:HH-mm-ss}";
                Log($"输出结果会放在桌面的 {targetPath} 文件夹");
                targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), targetPath);
                gameFolder = gameDir.FullName;
                var mdlExts = new string[] { ".vvd", ".ani", ".dx80.vtx", ".dx90.vtx", ".sw.vtx", ".phy" };
                string? materialsDir = Path.Combine(gameFolder, "materials");
                if (!Directory.Exists(materialsDir))
                {
                    materialsDir = null;
                }
                var materialTarget = Path.Combine(targetPath, "materials");
                var fails = new List<string>();
                foreach (var item in allTargets)
                {
                    bool ok = false;
                    if (item.EndsWith(".mdl"))
                    {
                        ok = TryBringFile(item, gameFolder, targetPath);
                        if (ok)
                        {
                            if (materialsDir != null)
                            {
                                var f = Path.Combine(gameFolder, item);
                                var r = MdlTextureFinder.CopyMaterialForModel(f, materialsDir, materialTarget);
                                List<string>[] arrays1 = { r.LostVmtNames, r.LostVtfNames };
                                foreach (var array in arrays1)
                                {
                                    foreach (var v in array)
                                    {
                                        Log($"丢失贴图: {v}");
                                    }
                                }
                            }
                            foreach (var e in mdlExts)
                            {
                                _ = TryBringFile(Path.ChangeExtension(item, e), gameFolder, targetPath);
                            }
                        }
                    }
                    else
                    {
                        ok = TryBringFile(Path.Combine("sound", item), gameFolder, targetPath);
                    }
                    if (!ok)
                    {
                        fails.Add(item);
                    }
                }
                foreach (var item in fails)
                {
                    Log($"找不到文件：  {item}");
                }
                Log($"输出结果放在 {targetPath}");
            });
        }

        private static readonly Regex regFileName = new("[\"'](models/[^'\"]+\\.mdl|[^'\"]+\\.wav|[^'\"]+\\.mp3|[^'\"]+\\.ogg)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        private static IList<string> ScanCodeFile(string filename)
        {
            var txt = File.ReadAllText(filename);
            var ls = new List<string>();
            if (string.IsNullOrWhiteSpace(txt)) { return ls; }
            var mc = regFileName.Matches(txt);
            foreach (var item in mc)
            {
                if (item is not Match match) { continue; }
                var str = match.Value.Trim().Trim('"', '\'').ToLower();
                if (ls.Contains(str)) { continue; }
                ls.Add(str);
            }
            return ls;
        }

        private bool TryBringFile(string itemPath, string gamePath, string targetPath)
        {
            var origin = new FileInfo(Path.Combine(gamePath, itemPath));
            if (origin.Exists)
            {
                var t = Path.Combine(targetPath, itemPath);
                var dir = Path.GetDirectoryName(t);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                origin.CopyTo(t, true);
                Log($"复制文件 {itemPath}");
                return true;
            }
            return false;
        }
    }
}
