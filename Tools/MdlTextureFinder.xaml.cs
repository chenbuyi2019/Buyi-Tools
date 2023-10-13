using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BuyiTools.Tools
{
    public partial class MdlTextureFinder : ToolPageBase
    {
        public MdlTextureFinder()
        {
            InitializeComponent();
        }
        private void MdlTextureFinder_Load(object sender, RoutedEventArgs e)
        {
            RegisterContorlSaveData(TxtMaterialsFolder, TxtMdlFiles);
            RegisterTextboxDropFilePath(TxtMaterialsFolder, TxtMdlFiles);
        }

        private async void ButPackMaterial_Click(object sender, EventArgs e)
        {
            if (TxtMdlFiles.Text.Length < 10)
            {
                Log("没有 mdl 文件路径");
                return;
            }
            var materialsDir = Utils.CleanPath(TxtMaterialsFolder.Text);
            if (materialsDir.Length < 10)
            {
                Log("没有 materials 文件夹 路径");
                return;
            }
            char[] chars = { '\r', '\n' };
            var lines = TxtMdlFiles.Text.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            var mdls = new List<string>();
            var totalLost = 0;
            await DoWorkAsync(() =>
            {
                const string MDLext = ".mdl";
                foreach (var line in lines)
                {
                    var filepath = Utils.CleanPath(line);
                    if (!File.Exists(filepath)) { throw new Exception($"文件不存在 {line}"); }
                    if (!MDLext.Equals(Path.GetExtension(filepath), Utils.IgnoreCase))
                    {
                        Log($"跳过 {filepath}");
                        continue;
                    }
                    mdls.Add(filepath);
                }
                if (mdls.Count < 1) { throw new Exception("没有任何mdl文件"); }
                if (!Directory.Exists(materialsDir)) { throw new Exception($"文件夹不存在 {materialsDir}"); }
                Log($"共 {mdls.Count} 个文件要处理");
                var destDir = $"vmt_{DateTime.Now:MMdd_HHmmss}";
                Log($"输出文件夹在桌面的 {destDir}");
                destDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), destDir);
                foreach (var filepath in mdls)
                {
                    Log($"读取 {filepath}");
                    var r = SourceMDLFileTextureInfo.ParseFile(filepath);
                    r.SearchVmtVtfFiles(materialsDir);
                    Log($"扫描到 vmt {r.VmtFileNames.Count}个，vtf {r.FoundVtfFiles.Count + r.LostVtfNames.Count}个");
                    List<string>[] arrays1 = { r.LostVmtNames, r.LostVtfNames };
                    foreach (var array in arrays1)
                    {
                        foreach (var v in array)
                        {
                            totalLost += 1;
                            Log($"丢失: {v}");
                        }
                    }
                    List<string>[] arrays2 = { r.FoundVmtFiles, r.FoundVtfFiles };
                    foreach (var array in arrays2)
                    {
                        foreach (var v in array)
                        {
                            var relPath = Path.GetRelativePath(r.MaterialsDir, v);
                            var destPath = Path.Combine(destDir, relPath);
                            var dir = Path.GetDirectoryName(destPath);
                            if (string.IsNullOrEmpty(dir)) { throw new Exception($"无法获取文件夹路径 {v}"); }
                            Directory.CreateDirectory(dir);
                            File.Copy(v, destPath, true);
                        }
                    }
                }
                if (totalLost > 0)
                {
                    Log($"共丢失 {totalLost} 个文件");
                }
                else
                {
                    Log($"全部贴图已集齐，无缺失");
                }
                Log($"输出文件夹 {destDir}");
            });
            TxtMdlFiles.Text = string.Join("\r\n", mdls);
        }

    }

    internal class SourceMDLFileTextureInfo
    {
        public SourceMDLFileTextureInfo()
        {
        }

        private const int Header_TextureOffset = 204;
        private const int TextureInfoSize = 64;

        public static SourceMDLFileTextureInfo ParseFile(string filename)
        {
            var fileLen = Utils.GetFileLength(filename);
            if (fileLen < 100) { throw new Exception(""); }
            using var filestream = File.OpenRead(filename);
            using var reader = new BinaryReader(filestream);
            var header = new string(reader.ReadChars(4));
            if (!header.Equals("IDST"))
            {
                throw new Exception("文件头不是IDST");
            }
            filestream.Seek(Header_TextureOffset, SeekOrigin.Begin);
            var textureCount = reader.ReadInt32();
            if (textureCount > 255)
            {
                throw new Exception($"读取到贴图数量不合理 {textureCount}");
            }
            if (textureCount < 1) { return new SourceMDLFileTextureInfo() { FilePath = filename }; }
            var textureOffset = reader.ReadInt32();
            if (textureOffset < filestream.Position || textureOffset >= fileLen)
            {
                throw new Exception($"读取到贴图offset不合理 {textureOffset}");
            }
            var textureDirCount = reader.ReadInt32();
            if (textureDirCount < 1 || textureDirCount > 255)
            {
                throw new Exception($"读取到贴图目录数量不合理 {textureDirCount}");
            }
            var textureDirOffset = reader.ReadInt32();
            if (textureDirOffset < 1 || textureDirOffset >= fileLen)
            {
                throw new Exception($"读取到贴图目录offset不合理 {textureDirOffset}");
            }
            filestream.Seek(textureOffset, SeekOrigin.Begin);
            var nameOffsets = new List<long>();
            for (int i = 0; i < textureCount; i++)
            {
                nameOffsets.Add(filestream.Position + reader.ReadInt32());
                reader.ReadBytes(TextureInfoSize - 4);
            }
            var fileNames = new List<string>();
            foreach (var offset in nameOffsets)
            {
                filestream.Seek(offset, SeekOrigin.Begin);
                var str = Utils.ReadNullTerminatedStr(filestream);
                if (!string.IsNullOrWhiteSpace(str))
                {
                    fileNames.Add(str);
                }
            }
            nameOffsets.Clear();
            filestream.Seek(textureDirOffset, SeekOrigin.Begin);
            for (int i = 0; i < textureDirCount; i++)
            {
                nameOffsets.Add(reader.ReadInt32());
            }
            var dirNames = new List<string>();
            foreach (var offset in nameOffsets)
            {
                filestream.Seek(offset, SeekOrigin.Begin);
                var str = Utils.ReadNullTerminatedStr(filestream);
                if (!string.IsNullOrWhiteSpace(str))
                {
                    dirNames.Add(str);
                }
            }
            var result = new SourceMDLFileTextureInfo()
            {
                FilePath = filename,
                VmtFolderNames = dirNames,
                VmtFileNames = fileNames
            };
            return result;
        }

        public string FilePath { get; set; } = string.Empty;
        public List<string> VmtFileNames { get; private set; } = new List<string>();
        public List<string> VmtFolderNames { get; private set; } = new List<string>();
        public string MaterialsDir { get; private set; } = string.Empty;
        public List<string> FoundVmtFiles { get; private set; } = new List<string>();
        public List<string> LostVmtNames { get; private set; } = new List<string>();
        public List<string> FoundVtfFiles { get; private set; } = new List<string>();
        public List<string> LostVtfNames { get; private set; } = new List<string>();

        public void SearchVmtVtfFiles(string materialsDir)
        {
            if (!Directory.Exists(materialsDir))
            {
                throw new Exception($"文件夹不存在 {materialsDir}");
            }
            var foundVmtFiles = new List<string>();
            var usedVmtNames = new List<string>();
            var lostVmtNames = new List<string>();
            var vtfNames = new List<string>();
            foreach (var fd in VmtFolderNames)
            {
                var dir = Path.Combine(materialsDir, fd);
                if (!Directory.Exists(dir)) { continue; }
                foreach (var name in VmtFileNames)
                {
                    var lowerName = name.ToLower();
                    if (!usedVmtNames.Contains(lowerName)) { usedVmtNames.Add(lowerName); }
                    var fn = Path.ChangeExtension(name, ".vmt");
                    fn = Path.Combine(dir, fn);
                    if (!File.Exists(fn)) { continue; }
                    var vtfs = SearchVtfFromVmtFile(fn, materialsDir);
                    if (vtfs == null) { continue; }
                    foreach (var v in vtfs)
                    {
                        if (Utils.FindStrInListIgnoreCase(vtfNames, v) < 0) { vtfNames.Add(v); }
                    }
                    foundVmtFiles.Add(fn);
                }
            }
            foreach (var name in VmtFileNames)
            {
                var lowerName = name.ToLower();
                if (!usedVmtNames.Contains(lowerName)) { lostVmtNames.Add(name); }
            }
            var foundVtfFiles = new List<string>();
            var lostVtfNames = new List<string>();
            foreach (var name in vtfNames)
            {
                var fullpath = Path.Combine(materialsDir, name + ".vtf");
                if (File.Exists(fullpath))
                {
                    foundVtfFiles.Add(fullpath);
                }
                else
                {
                    lostVtfNames.Add(name);
                }
            }
            this.MaterialsDir = materialsDir;
            this.FoundVmtFiles = foundVmtFiles;
            this.LostVmtNames = lostVmtNames;
            this.FoundVtfFiles = foundVtfFiles;
            this.LostVtfNames = lostVtfNames;
        }

        private static IList<string> ReadVmtWords(Stream stream, bool ignoreComments = true)
        {
            var result = new List<string>();
            var reader = new BinaryReader(stream);
            var buffer = new List<char>();
            var wordComplete = false;
            var inQuote = false;
            var inComment = false;
            const char quote = '"';
            const char tab = '\t';
            const char spc = ' ';
            const char cr = '\r';
            const char lf = '\n';
            const char slash = '/';
            int slashAsc = Convert.ToInt32(slash);
            while (true)
            {
                if (wordComplete && buffer.Count > 0)
                {
                    if (!ignoreComments || !inComment)
                    {
                        var word = new string(buffer.ToArray());
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            result.Add(word);
                        }
                    }
                    inQuote = false;
                    inComment = false;
                    buffer.Clear();
                }
                wordComplete = false;
                if (reader.PeekChar() < 0) { break; }
                var c = reader.ReadChar();
                if (inComment)
                {
                    if (c == cr || c == lf)
                    {
                        wordComplete = true;
                        buffer.Insert(0, slash);
                        continue;
                    }
                    buffer.Add(c);
                }
                else
                {
                    if (inQuote)
                    {
                        if (c == quote || c == cr || c == lf)
                        {
                            wordComplete = true;
                            continue;
                        }
                        buffer.Add(c);
                    }
                    else
                    {
                        if (c == quote)
                        {
                            inQuote = true;
                            wordComplete = true;
                            continue;
                        }
                        if (c == cr || c == lf || c == spc || c == tab)
                        {
                            wordComplete = true;
                            continue;
                        }
                        if (c == slash && reader.PeekChar() == slashAsc)
                        {
                            wordComplete = true;
                            inComment = true;
                            continue;
                        }
                        buffer.Add(c);
                    }
                }
            }
            return result;
        }

        private static string? ReadNextWord(IList<string> words, string key)
        {
            var index = Utils.FindStrInListIgnoreCase(words, key);
            if (index < 0 || index + 1 >= words.Count) { return null; }
            return words[index + 1];
        }

        private static IList<string>? SearchVtfFromVmtFile(string vmtFile, string materialsDir)
        {
            vmtFile = vmtFile.Replace("\\", "/").Replace("../", "");
            if (!Path.IsPathRooted(vmtFile))
            {
                vmtFile = Path.Combine(materialsDir, vmtFile);
            }
            if (!File.Exists(vmtFile)) { return null; }
            using var stream = File.OpenRead(vmtFile);
            var words = ReadVmtWords(stream, true);
            if (words.Count < 3) { return null; }
            var result = new List<string>();
            var gotPatch = Utils.FindStrInListIgnoreCase(words, "patch") >= 0 && Utils.FindStrInListIgnoreCase(words, "include") >= 0;
            if (gotPatch)
            {
                var includeVmt = ReadNextWord(words, "include");
                if (!string.IsNullOrWhiteSpace(includeVmt))
                {
                    includeVmt = includeVmt.Replace("\\", "/");
                    includeVmt = Utils.StrTrimPrefix(includeVmt, "materials/");
                    var includeVtf = SearchVtfFromVmtFile(includeVmt, materialsDir);
                    if (includeVtf != null)
                    {
                        foreach (var v in includeVtf)
                        {
                            if (Utils.FindStrInListIgnoreCase(result, v) < 0) { result.Add(v); }
                        }
                    }
                }
            }
            foreach (var key in VtfFilenameKeys)
            {
                var v = ReadNextWord(words, key);
                if (string.IsNullOrWhiteSpace(v)) { continue; }
                v = Utils.StrTrimSuffix(v, ".vtf");
                if (Utils.FindStrInListIgnoreCase(result, v) >= 0) { continue; }
                result.Add(v);
            }
            return result;
        }

        private static readonly string[] VtfFilenameKeys = {
            "$basetexture" ,
            "$basetexture2",
            "$bumpmap",
            "$detail",
            "$blendmodulatetexture",
            "$selfillummask",
            "$selfillumtexture",
            "$lightwarptexture",
            "$ambientoccltexture",
            "$phongexponenttexture",
            "$phongwarptexture",
            "$envmapmask",
            "$tintmasktexture"
        };
    }
}
