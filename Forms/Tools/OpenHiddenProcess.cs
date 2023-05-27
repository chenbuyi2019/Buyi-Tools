using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuyiTools.Forms.Tools
{
    public partial class OpenHiddenProcess : ToolBase
    {

        private readonly static string[] bannedProcessNames = { "svchost",
            "SearchIndexer" ,
            "services","ChsIME",
            "smss","ctfmon",
            "explorer",
            "dwm" ,"dllhost","csrss"};

        public OpenHiddenProcess()
        {
            InitializeComponent();
        }

        private void OpenHiddenSrcds_Load(object sender, EventArgs e)
        {
            RegisterContorlSaveData(TxtTargetName);
        }

        private void ButShowThem_Click(object sender, EventArgs e)
        {
            DoWork(() => { ChangeTargetProcessWindowVisible(true); });
        }

        private void ButHideThem_Click(object sender, EventArgs e)
        {
            DoWork(() => { ChangeTargetProcessWindowVisible(false); });
        }

        private void ChangeTargetProcessWindowVisible(bool visible)
        {
            var targetName = TxtTargetName.Text.Trim();
            if (targetName.Length < 1) { throw new Exception("进程名字为空"); }
            foreach (var n in bannedProcessNames)
            {
                if (targetName.Equals(n, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception($"不能对这个进程进行操作 {n}");
                }
            }
            var all = Process.GetProcessesByName(targetName);
            if (all.Length < 1)
            {
                Log($"找不到任何相关的进程 {targetName}");
                return;
            }
            Log($"共找到 {all.Length} 个进程 {targetName}");
            int job = visible ? 1 : 0;
            Log($"任务:{(visible ? "显示" : "隐藏")}");
            var idlist = new List<uint>();
            for (int i = 0; i < all.Length; i++)
            {
                using var process = all[i];
                idlist.Add(Convert.ToUInt32(process.Id));
            }
            var proc = new EnumWindowsProc((IntPtr hWnd, IntPtr lParam) =>
            {
                GetWindowThreadProcessId(hWnd, out uint pid);
                if (pid > 0 && idlist.Contains(pid))
                {
                    Log($"找到窗口 {hWnd:X}");
                    ShowWindow(hWnd, job);
                }
                return true;
            });
            EnumWindows(proc, IntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}
