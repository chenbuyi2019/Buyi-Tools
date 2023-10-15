using Okolni.Source.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace BuyiTools.Tools
{
    public partial class A2SViewer : ToolPageBase
    {
        public A2SViewer()
        {
            InitializeComponent();
        }

        public bool EnableAutoRefresh { get; set; } = true;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.RegisterContorlSaveData(txtInputUrl);
            var t1 = new Task(Work);
            t1.Start();
        }

        private string inputUrl = string.Empty;

        private void TxtInputUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputUrl = txtInputUrl.Text.Trim();
        }

        private string _result = string.Empty;

        public string ResultText
        {
            get { return _result; }
            set
            {
                _result = value;
                CallPropertyChanged(nameof(ResultText));
            }
        }

        private void Work()
        {
            var lastUrl = string.Empty;
            var nextFetch = DateTime.MinValue;
            while (true)
            {
                Thread.Sleep(1000);
                if (!EnableAutoRefresh) { continue; }
                var now = DateTime.Now;
                if (now < nextFetch) { continue; }
                if (!inputUrl.Equals(lastUrl))
                {
                    lastUrl = inputUrl;
                    Utils.SimpleUITimer(() => { SaveControlsDataToFile(); }, 1);
                    Log($"开始分析  {inputUrl}");
                }
                nextFetch = now.AddSeconds(2);
                IQueryConnection? conn = null;
                var sb = new StringBuilder();
                sb.AppendLine($"时间： {now:HH:mm:ss}");
                try
                {
                    if (lastUrl.Length < 1) { throw new Exception("空白的输入"); }
                    var sw = new Stopwatch();
                    conn = new QueryConnection();
                    var indexOfSp = lastUrl.IndexOf(':');
                    if (indexOfSp > 0)
                    {
                        conn.Host = lastUrl.Substring(0, indexOfSp);
                        var portStr = lastUrl.Substring(indexOfSp + 1);
                        if (!int.TryParse(portStr, out var port))
                        {
                            throw new Exception($"无法识别为数字 {portStr}");
                        }
                        if (port < 1)
                        {
                            throw new Exception($"非正整数不能作为端口数字 {port}");
                        }
                        conn.Port = port;
                    }
                    else
                    {
                        conn.Host = lastUrl;
                        conn.Port = 27015;
                    }
                    sb.AppendLine($"{conn.Host}:{conn.Port}");
                    sw.Restart();
                    conn.Connect(5000);
                    var info = conn.GetInfo();
                    sw.Stop();
                    sb.AppendLine($"PING: {sw.ElapsedMilliseconds}ms");
                    sb.AppendLine($"名字: {info.Name}");
                    sb.AppendLine($"游戏: {info.Game}");
                    sb.AppendLine($"协议版本: {info.Protocol}");
                    sb.AppendLine($"文件夹: {info.Folder}");
                    sb.AppendLine($"AppID: {info.ID}");
                    sb.AppendLine($"GameID:{info.GameID}");
                    sb.AppendLine($"玩家数: {info.Players} / {info.MaxPlayers} (Bot: {info.Bots})");
                    sb.AppendLine($"地图: {info.Map}");
                    sb.AppendLine($"系统: {info.Environment}");
                    sb.AppendLine($"VAC启用: {(info.VAC ? '是' : '否')}");
                    sb.AppendLine($"游戏版本: {info.Version}");
                    sb.AppendLine($"类型: {info.ServerType}");
                    sb.AppendLine($"密码: {(info.Visibility == Okolni.Source.Common.Enums.Visibility.Public ? '无' : '有')}");
                    var players = conn.GetPlayers();
                    if (players.Players.Any())
                    {
                        sb.AppendLine("");
                        sb.AppendLine($"玩家列表: ");
                        foreach (var ply in players.Players)
                        {
                            sb.AppendLine($"{ply.Name}    {ply.Duration.TotalMinutes:0.0}分钟");
                        }
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"出错: {ex.Message}");
                    LastError = ex;
                }
                conn?.Disconnect();
                var result = sb.ToString().Trim();
                ResultText = result;
            }
        }

    }
}
