using System;
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
                   SourceMDLFileInfo.ParseFile(filepath);
               }
           });
        }

    }

    internal class SourceMDLFileInfo
    {
        public SourceMDLFileInfo()
        {
        }

        public static SourceMDLFileInfo ParseFile(string filename)
        {
            using var filestream = File.OpenRead(filename);
            using var reader = new BinaryReader(filestream);
            var header = new string(reader.ReadChars(4));
            if (!header.Equals("IDST"))
            {
                throw new Exception("文件头不是IDST");
            }
            var result = new SourceMDLFileInfo();
            return result;
        }
    }
}
