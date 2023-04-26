using BuyiTools.Forms.Tools;
using System.Diagnostics;
using System.Resources;
using System.Xml.Linq;
using Windows.System;

namespace BuyiTools
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private readonly Dictionary<string, Type> tools = new();
        private string? currentToolName;
        private ToolBase? currentTool;

        private void MainForm_Load(object sender, EventArgs e)
        {
            buildTimeToolStripMenuItem.Text = $"编译时间 {Properties.Resources.BuildDate.Trim()}";
            RegisterTool<MklinkTool>("量产 Mklink");
            RegisterTool<FileDeleteTool>("删除相对文件");
            RegisterTool<FastDLCreator>("FastDL 生成");
            var args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
            {
                var toolName = args[1];
                OpenToolByName(toolName);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        #region 界面
        private void TimerMoveRandom_Tick(object sender, EventArgs e)
        {
            TimerMoveRandom.Dispose();
            var r = new Random();
            this.Left -= r.Next(-100, 100);
            this.Top -= r.Next(-100, 100);
            this.Opacity = 1;
        }

        private void TimerUpdateProgress_Tick(object sender, EventArgs e)
        {
            if (currentTool == null) return;
            BarWorkProgress.Value = Convert.ToInt32(BarWorkProgress.Maximum * currentTool.WorkProgress);
        }
        #endregion

        #region 日志
        private void LogInternal(string str)
        {
            if (string.IsNullOrEmpty(str) || TxtLog.IsDisposed) { return; }
            Debug.WriteLine(str);
            TxtLog.AppendText(str + "\r\n");
            if (TxtLog.TextLength > 9999)
            {
                TxtLog.Text = TxtLog.Text.Substring(5000);
            }
            TxtLog.Select(TxtLog.TextLength, 0);
            TxtLog.ScrollToCaret();
        }

        private void Log(string str)
        {
            this.Invoke(LogInternal, str);
        }
        #endregion

        #region 工具
        private void RegisterTool<T>(string name)
        {
            Type t = typeof(T);
            tools.Add(name, t);
            var button = MenuTools.DropDownItems.Add(name);
            button.Click += (object? sender, EventArgs e) =>
            {
                OpenToolByName(name);
            };
        }

        private void OpenToolByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(currentToolName))
            {
                StartSelfProcess(name, false);
                return;
            }
            InputData.ReadFromFile();
            tools.TryGetValue(name, out Type? t);
            if (t == null) { throw new Exception($"无法找到工具 {name}"); }
            var fullName = t.FullName;
            if (string.IsNullOrEmpty(fullName)) { throw new Exception($"无法获取类名 {t.Name}"); }
            object? obj = t.Assembly.CreateInstance(fullName);
            if (obj == null) { throw new Exception($"无法创建实例 {name} {fullName}"); }
            currentToolName = name;
            this.Text = $"{name} - {this.Text}";
            ToolBase tool = (ToolBase)obj;
            PnTool.Controls.Add(tool);
            if (PnTool.Width < tool.Width) { this.Width = tool.Width + (this.Width - PnTool.Width) + 12; }
            if (PnTool.Height < tool.Width) { this.Height = tool.Height + (this.Height - PnTool.Height) + 12; }
            Log($"启动 {name}");
            currentTool = tool;
            tool.LogSent += (object? sender, string msg) =>
            {
                Log(msg);
            };
            TimerUpdateProgress.Enabled = true;
        }

        #endregion

        #region 高级
        private void LastErrorDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var but = MessageBoxButtons.OK;
            Exception? err = currentTool?.LastError;
            if (err == null)
            {
                MessageBox.Show("还没有任何错误发生", this.Text, but, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(err.ToString(), this.Text, but, MessageBoxIcon.Error);
            }
        }


        private void ButRestartAsAdmin_Click(object sender, EventArgs e)
        {
            StartSelfProcess(currentToolName, true);
        }

        private void StartSelfProcess(string? name, bool runas)
        {
            var p = Environment.ProcessPath;
            if (string.IsNullOrWhiteSpace(p)) { throw new Exception("无法执行"); }
            var info = new ProcessStartInfo
            {
                FileName = p
            };
            if (!string.IsNullOrWhiteSpace(name))
            {
                info.ArgumentList.Add(name);
            }
            if (runas)
            {
                info.UseShellExecute = true;
                info.Verb = "runas";
            }
            try
            {
                using var unused = Process.Start(info);
            }
            catch (Exception ex)
            {
                Log($"进程启动失败 {ex.Message}");
            }
            return;
        }
        #endregion

        #region 关于
        private void GithubUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var proc = Process.Start("explorer.exe", "https://github.com/chenbuyi2019/Buyi-Tools");
        }
        #endregion
    }
}