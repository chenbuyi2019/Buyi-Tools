using Microsoft.VisualBasic.Logging;
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
    public partial class MdlTextureFinder : ToolBase
    {
        public MdlTextureFinder()
        {
            InitializeComponent();
        }

        private void MdlTextureFinder_Load(object sender, EventArgs e)
        {
            RegisterContorlSaveData(TxtMaterialsFolder, TxtMdlFiles);
            RegisterTextboxDropFilePath(TxtMaterialsFolder, TxtMdlFiles);
        }

        private void ButPackMaterial_Click(object sender, EventArgs e)
        {
            DoWork(() =>
           {
               if (TxtMdlFiles.TextLength < 10)
               {
                   throw new Exception("没有 mdl 文件路径");
               }
               if (TxtMaterialsFolder.TextLength < 10)
               {
                   throw new Exception("没有 materials 文件夹 路径");
               }
               var materialsDir = Utils.CleanPath(TxtMaterialsFolder.Text);
               if (!Directory.Exists(materialsDir)) { throw new Exception($"文件夹不存在 {materialsDir}"); }
               char[] chars = { '\r', '\n' };
               var lines = TxtMdlFiles.Text.Split(chars, StringSplitOptions.RemoveEmptyEntries);
               var mdls = new List<string>();
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
               Log($"共 {mdls.Count} 个文件要处理");
               TxtMdlFiles.Text = string.Join("\r\n", mdls);
               foreach (var filepath in mdls)
               {
                   Log($"读取 {filepath}");
                   var result = SourceMDLFileInfo.ParseFile(filepath);
               }
           });
        }

    }

    internal class SourceMDLFileInfo
    {
        public SourceMDLFileInfo()
        {
        }

        private const int Header_TextureOffset = 204;
        private const int TextureInfoSize = 64;

        public static SourceMDLFileInfo ParseFile(string filename)
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
            if (textureCount < 1 || textureCount > 255)
            {
                throw new Exception($"读取到贴图数量不合理 {textureCount}");
            }
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
            var nameOffsets = new Dictionary<long, int>();
            for (int i = 0; i < textureCount; i++)
            {
                nameOffsets.Add(filestream.Position, reader.ReadInt32());
                reader.ReadBytes(TextureInfoSize - 4);
            }
            var fileNames = new List<string>();
            foreach (var kv in nameOffsets)
            {
                var offset = kv.Key + kv.Value;
                Debug.WriteLine($"offset: {offset}");
                filestream.Seek(offset, SeekOrigin.Begin);
                var str = Utils.ReadNullTerminatedString(filestream);
                if (!string.IsNullOrWhiteSpace(str))
                {
                    fileNames.Add(str);
                }
            }
            var result = new SourceMDLFileInfo()
            {
                FilePath = filename
            };
            result.MaterialFileNames.AddRange(fileNames);
            return result;
        }

        public string FilePath { get; set; } = string.Empty;
        public List<string> MaterialFileNames { get; } = new();
        public List<string> MaterialFolderNames { get; } = new();

    }
}
