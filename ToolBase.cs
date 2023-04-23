using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuyiTools
{
    public partial class ToolBase : UserControl
    {
        public ToolBase()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public event EventHandler<string>? LogSent;

        protected void Log(string msg)
        {
            if (LogSent == null) { return; }
            LogSent(this, msg);
        }

    }
}
