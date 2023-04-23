﻿using BuyiTools.Tools;
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            InputData.ReadFromFile();
            buildTimeToolStripMenuItem.Text = $"编译时间 {Properties.Resources.BuildDate.Trim()}";
            frameworkVersionToolStripMenuItem.Text = $"平台 {AppContext.TargetFrameworkName}";
            RegisterTool<MklinkTool>("量产 Mklink");
            RegisterTool<FileDeleteTool>("删除相对文件");
            var args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
            {
                var toolName = args[1];
                OpenToolByName(toolName);
            }
        }

        private void Log(string str)
        {
            if (string.IsNullOrEmpty(str)) { return; }
            Debug.WriteLine(str);
            TxtLog.AppendText(str + "\r\n");
            if (TxtLog.TextLength > 9999)
            {
                TxtLog.Text = TxtLog.Text.Substring(5000);
            }
            TxtLog.Select(TxtLog.TextLength, 0);
            TxtLog.ScrollToCaret();
        }

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
            if (PnTool.Width < tool.Width) { this.Width = tool.Width + (this.Width - PnTool.Width) + 3; }
            if (PnTool.Height < tool.Width) { this.Height = tool.Height + (this.Height - PnTool.Height) + 3; }
            Log($"启动 {name}");
            tool.LogSent += (object? sender, string msg) =>
            {
                Log(msg);
            };
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

        private void GithubUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var proc = Process.Start("explorer.exe", "https://github.com/chenbuyi2019/Buyi-Tools");
        }
    }
}