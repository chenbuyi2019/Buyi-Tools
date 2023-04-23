using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuyiTools.Tools
{
    public partial class FileDeleteTool : ToolBase
    {
        public FileDeleteTool()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = Utils.RunCmd("ipconfig", null, null, 1000);
            Log(f[0]);
            Log(f[1]);
        }
    }
}
