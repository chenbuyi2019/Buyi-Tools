
using BuyiTools.Properties;
using BuyiTools.Tools;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Mono.Options;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace BuyiTools
{
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private Version currentVersion = new();
        public string GreatTitle = "布衣工具箱";

        public MainWindow()
        {
            Environment.CurrentDirectory = AppContext.BaseDirectory;
            InitializeComponent();
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            var versionStr = BuyiTools.Properties.Resources.ReleaseVersion;
            currentVersion = Version.Parse(versionStr);
            this.GreatTitle += " " + versionStr;
            this.Title = this.GreatTitle;
            RegisterTools();
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string? initTool = null;
                var options = new OptionSet() {
                    {"tool=", (s)=>{initTool=s; } }
                };
                options.Parse(args);
                if (!string.IsNullOrWhiteSpace(initTool))
                {
                    initTool = initTool.Trim();
                    var tool = GetToolByName(initTool);
                    if (tool == null)
                    {
                        Log($"找不到要启动的工具 {initTool}");
                    }
                    else
                    {
                        OpenTool(tool);
                    }
                }
            }
        }

        private void RegisterTools()
        {
            AllTools.Clear();
            AllTools.Add(new ToolInfo("隐藏进程窗口", nameof(HideProcessWindow)));
            AllTools.Add(new ToolInfo("FastDL 生成", nameof(FastDLCreator)));
            AllTools.Add(new ToolInfo("FastDL 下载", nameof(FastDLDownloader)));
            AllTools.Add(new ToolInfo("删除相对文件", nameof(FileDeleteTool)));
            AllTools.Add(new ToolInfo("文件夹合并", nameof(FolderCombiner)));
            AllTools.Add(new ToolInfo("Mklink 制作", nameof(MklinkTool)));
            AllTools.Add(new ToolInfo("MDL 贴图打包", nameof(MdlTextureFinder)));
            AllTools.Add(new ToolInfo("VTF 贴图压缩", nameof(VtfCompress)));
            AllTools.Add(new ToolInfo("SourceMod Dump Handles 分析", nameof(SmDumpParser)));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            this.Left += rnd.Next(-32, 32);
            this.Top += rnd.Next(-32, 40);
            CheckUpdate();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void CallPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<ToolInfo> AllTools { get; } = new();

        public ToolInfo? OpenedTool { get; set; } = null;
        public ToolPageBase? OpenedPage { get; set; } = null;

        private ToolInfo? GetToolByName(string name)
        {
            return AllTools.FirstOrDefault((x) =>
            {
                return x != null && x.Title.Equals(name, StringComparison.CurrentCultureIgnoreCase);
            });
        }

        private void OpenTool(ToolInfo tool)
        {
            var screenSize = this.GetMonitorWorkSize();
            this.Top = screenSize.Height + 10;
            toolFrame.Source = tool.PageUri;
            OpenedTool = tool;
            this.Width = 1100;
            this.Height = 1100;
            this.Title = $"{tool.Title} - {GreatTitle}";
            Log($"启动 {tool.Title}");
        }

        private void StartSelf(bool admin, params string[] args)
        {
            var info = new ProcessStartInfo
            {
                FileName = Environment.ProcessPath,
                WorkingDirectory = AppContext.BaseDirectory,
                UseShellExecute = true
            };
            if (admin)
            {
                info.Verb = "runas";
            }
            foreach (var arg in args)
            {
                info.ArgumentList.Add(arg);
            }
            try
            {
                using var p = Process.Start(info);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void ToolMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not MenuItem item) { return; }
            if (item.Header is not ToolInfo tool) { return; }
            if (OpenedTool == null)
            {
                OpenTool(tool);
                return;
            }
            else
            {
                StartSelf(false, "-tool", tool.Title);
            }
        }

        private void ToolFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content is not ToolPageBase page) { return; }
            OpenedPage = page;
            page.Window = this;
            page.LogEvent += Page_LogEvent;
            page.Loaded += ToolPage_Loaded;
        }

        private void ToolPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (OpenedPage == null) { return; }
            var sz = OpenedPage.DesiredSize;
            this.Height = this.ActualHeight - toolFrame.ActualHeight - txtLog.ActualHeight + sz.Height + 125;
            toolFrame.Height = sz.Height;
            var screenSize = this.GetMonitorWorkSize();
            this.Width = sz.Width + 5;
            var dpi = VisualTreeHelper.GetDpi(this);
            screenSize.Width /= dpi.DpiScaleX;
            screenSize.Height /= dpi.DpiScaleY;
            var r = new Random();
            this.Left = Math.Max(3, screenSize.Width - this.ActualWidth + r.Next(-10, 20)) / 2;
            this.Top = Math.Max(3, screenSize.Height - this.ActualHeight + r.Next(-10, 15)) / 2;
            this.MinWidth = this.ActualWidth * 0.9;
            this.MinHeight = this.ActualHeight * 0.9;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #region 高级
        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            Utils.OpenProcess(AppContext.BaseDirectory);
        }

        private void BtnRestartAdmin_Click(object sender, EventArgs e)
        {
            if (OpenedTool == null)
            {
                StartSelf(true);
            }
            else
            {
                StartSelf(true, "-tool", OpenedTool.Title);
            }
        }

        private void BtnViewLastError_Click(object sender, EventArgs e)
        {
            var sets = new MetroDialogSettings() { AnimateShow = false, AnimateHide = false };
            if (OpenedPage == null || OpenedPage.LastError == null)
            {
                this.ShowMessageAsync("_", "最近没有出错", MessageDialogStyle.Affirmative, sets);
                return;
            }
            var ex = OpenedPage.LastError;
            this.ShowMessageAsync($"最近的一次出错 {ex.GetType().FullName}",
                ex.ToString(), MessageDialogStyle.Affirmative, sets);
        }

        #endregion

        #region 关于
        private void BtnViewSource_Click(object sender, EventArgs e)
        {
            Utils.OpenProcess("https://github.com/chenbuyi2019/Buyi-Tools");
        }

        private void BtnViewWebsite_Click(object sender, EventArgs e)
        {
            Utils.OpenProcess("https://buyi.dev/");
        }

        private async void CheckUpdate()
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/chenbuyi2019/Buyi-Tools/master/"),
                Timeout = new TimeSpan(0, 0, 6)
            };
            try
            {
                var text = await client.GetStringAsync("ReleaseVersion.txt");
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new Exception("获取到的版本号是空白");
                }
                if (!Version.TryParse(text, out Version? newVersion))
                {
                    throw new Exception($"获取到的版本号无法被识别: {text}");
                }
                if (currentVersion < newVersion)
                {
                    Log($"发现新版本: {newVersion}");
                    Log($"https://github.com/chenbuyi2019/Buyi-Tools/releases");
                }
                else
                {
                    Log($"没有发现新版本 {newVersion}");
                }
            }
            catch (Exception ex)
            {
                Log($"检测更新出错: {ex.Message}");
            }
        }
        #endregion

        #region 日志
        private string _log = string.Empty;

        public string TotalLog
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
                CallPropertyChanged(nameof(TotalLog));
            }
        }

        private void Log(string? msg)
        {
            msg ??= "null";
            var str = TotalLog + "\n" + msg;
            var len = str.Length;
            if (len > 99999)
            {
                str = str.Substring(len - 10000);
            }
            TotalLog = str;
        }

        private void Page_LogEvent(object sender, LogEventArgs e)
        {
            Log(e.Message);
        }

        private void TxtLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtLog.ScrollToEnd();
        }
        #endregion

    }
}
