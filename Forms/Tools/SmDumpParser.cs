using Microsoft.VisualBasic.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuyiTools.Forms.Tools
{
    public partial class SmDumpParser : ToolBase
    {
        public SmDumpParser()
        {
            InitializeComponent();
        }

        private void SmDumpParser_Load(object sender, EventArgs e)
        {
            SourceModHandleDump.BuildListViewHeader(ListDumpDetail);
            SourceModHandleDumpCounter.BuildListViewHeader(ListDumpCount);
            SourceModHandleDumpCounterCompareResult.BuildListViewHeader(ListDumpCompare);
        }

        private void ButReadClipboard_Click(object sender, EventArgs e)
        {
            DoWork(() =>
            {
                if (Clipboard.ContainsText())
                {
                    ParseSourceModDumpLog(Clipboard.GetText());
                    return;
                }
                Log("剪贴板内没有文本");
            });
        }

        private void ButReadFile_Click(object sender, EventArgs e)
        {
            DoWork(() =>
            {
                using var dialog = new OpenFileDialog()
                {
                    CheckPathExists = true,
                    AddExtension = true,
                    Multiselect = false,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Title = "选择 sm_dump_handles 导出的文件"
                };
                var r = dialog.ShowDialog();
                if (r != DialogResult.OK) { return; }
                var file = dialog.FileName;
                Log($"读取 {file}");
                string text = File.ReadAllText(file);
                ParseSourceModDumpLog(text);
            });
        }

        private Dictionary<string, SourceModHandleDumpCounter>? lastCounterDict;

        private void ParseSourceModDumpLog(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("输入的文本是空白的");
            }
            ListDumpCount.Items.Clear();
            ListDumpCompare.Items.Clear();
            ListDumpDetail.Items.Clear();
            var lines = content.Split('\r', '\n');
            var indexOfAddress = -1;
            var indexOfOwner = -1;
            var indexOfType = -1;
            var indexOfSize = -1;
            var indexOfTime = -1;
            var allLogs = new List<SourceModHandleDump>();
            var cur = CultureInfo.InvariantCulture;
            var totalSize = 0;
            var errorCount = 0;
            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                if (errorCount > 5)
                {
                    Log("无法识别的内容过多，放弃");
                    return;
                }
                var line = lines[lineIndex];
                if (string.IsNullOrWhiteSpace(line)) { continue; }
                var words = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length < 3) { continue; }
                var isHeaderLine = indexOfAddress < 0;
                if (isHeaderLine)
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        var word = words[i].Trim();
                        switch (word.ToLower())
                        {
                            case "handle":
                                indexOfAddress = i;
                                break;
                            case "owner":
                                indexOfOwner = i;
                                break;
                            case "type":
                                indexOfType = i;
                                break;
                            case "memory":
                                indexOfSize = i;
                                break;
                            case "time created":
                                indexOfTime = i;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    var log = new SourceModHandleDump
                    {
                        Address = words[indexOfAddress].Trim(),
                        Owner = words[indexOfOwner].Trim(),
                        Type = words[indexOfType].Trim()
                    };
                    var str = words[indexOfSize].Trim();
                    if (!int.TryParse(str, out int num))
                    {
                        errorCount += 1;
                        Log($"无法解析第 {lineIndex} 行的数字 {str}");
                        continue;
                    }
                    log.Size = num;
                    if (num > 0) { totalSize += num; }
                    if (indexOfTime >= 0)
                    {
                        str = words[indexOfTime].Trim();
                        if (!DateTime.TryParseExact(str, "MM/dd/yyyy - HH:mm:ss", cur, DateTimeStyles.None, out DateTime dt))
                        {
                            errorCount += 1;
                            Log($"无法解析第 {lineIndex} 行的日期 {str}");
                            continue;
                        }
                        log.CreateTime = dt;
                    }
                    allLogs.Add(log);
                }
            }
            Log($"解析得到句柄 {allLogs.Count} 条，总大小 {Utils.FormatBytesLength(totalSize)} ({totalSize})");
            if (allLogs.Count < 1) { return; }
            allLogs.Sort();
            var dict = new Dictionary<string, SourceModHandleDumpCounter>();
            ListDumpDetail.BeginUpdate();
            foreach (var item in allLogs)
            {
                ListDumpDetail.Items.Add(item.ToListViewItem());
                var tp = $"{item.Owner}{item.Type}";
                dict.TryGetValue(tp, out SourceModHandleDumpCounter? counter);
                if (counter == null)
                {
                    counter = new SourceModHandleDumpCounter();
                    dict.Add(tp, counter);
                }
                counter.AddLog(item);
            }
            ListDumpDetail.EndUpdate();
            ListDumpDetail.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListDumpCount.BeginUpdate();
            foreach (var counter in dict.Values)
            {
                ListDumpCount.Items.Add(counter.ToListViewItem());
            }
            ListDumpCount.EndUpdate();
            ListDumpCount.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            if (lastCounterDict != null)
            {
                var compareResults = new List<SourceModHandleDumpCounterCompareResult>();
                var usedKeys = new List<string>();
                foreach (var item in lastCounterDict)
                {
                    var old = item.Value;
                    usedKeys.Add(item.Key);
                    dict.TryGetValue(item.Key, out SourceModHandleDumpCounter? newOne);
                    compareResults.Add(SourceModHandleDumpCounterCompareResult.DoCompare(old, newOne));
                }
                foreach (var item in dict)
                {
                    if (usedKeys.Contains(item.Key)) { continue; }
                    var newOne = item.Value;
                    compareResults.Add(SourceModHandleDumpCounterCompareResult.DoCompare(null, newOne));
                }
                ListDumpCompare.BeginUpdate();
                foreach (var item in compareResults)
                {
                    if (item.HandleCount == 0) { continue; }
                    ListDumpCompare.Items.Add(item.ToListViewItem());
                }
                ListDumpCompare.EndUpdate();
                ListDumpCompare.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                var count = ListDumpCompare.Items.Count;
                if (count > 0)
                {
                    Log($"和上次输入的内容对比，句柄个数有 {count} 条变化");
                }
                else
                {
                    Log($"和上次输入的内容对比，句柄个数没有变化");
                }
            }
            lastCounterDict = dict;
        }

    }

    internal class SourceModHandleDump : IComparable
    {
        public SourceModHandleDump()
        {
        }

        public string Owner { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Size { get; set; } = 0;
        public DateTime CreateTime { get; set; } = DateTime.MinValue;

        public int CompareTo(object? obj)
        {
            if (obj == null) { return -1; }
            var item = obj as SourceModHandleDump;
            if (item == null) { return -1; }
            var n = this.Owner.CompareTo(item.Owner);
            if (n != 0) { return n; }
            n = this.Type.CompareTo(item.Type);
            if (n != 0) { return n; }
            n = this.Size.CompareTo(item.Size);
            if (n != 0) { return n; }
            n = this.CreateTime.CompareTo(item.CreateTime);
            if (n != 0) { return n; }
            return this.Address.CompareTo(item.Address);
        }

        public static void BuildListViewHeader(ListView view)
        {
            view.Items.Clear();
            view.Columns.Add("地址");
            view.Columns.Add("来源");
            view.Columns.Add("类型");
            view.Columns.Add("大小");
            view.Columns.Add("时间");
        }

        public ListViewItem ToListViewItem()
        {
            var item = new ListViewItem();
            item.SubItems.Add(this.Owner);
            item.SubItems.Add(this.Type);
            item.SubItems.Add(this.Size.ToString());
            item.SubItems.Add(this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            item.Text = this.Address;
            return item;
        }

        public override string ToString()
        {
            return $"[{Address} {Owner} {Type}]";
        }
    }

    internal class SourceModHandleDumpCounter
    {
        public SourceModHandleDumpCounter()
        {
        }

        private readonly List<SourceModHandleDump> logs = new();

        public void AddLog(SourceModHandleDump log)
        {
            this.logs.Add(log);
        }

        public string GetHandleOwner()
        {
            if (this.logs.Count < 1) { return string.Empty; }
            return this.logs.First().Owner;
        }

        public string GetHandleType()
        {
            if (this.logs.Count < 1) { return string.Empty; }
            return this.logs.First().Type;
        }

        public int GetHandleCount()
        {
            return this.logs.Count;
        }

        public int GetHandleSize()
        {
            var v = 0;
            foreach (var item in this.logs)
            {
                if (item.Size < 1) { continue; }
                v += item.Size;
            }
            return v;
        }

        public double GetAverageCreateIntervalSeconds()
        {
            if (this.logs.Count < 2) { return 0.0; }
            DateTime max = this.logs.First().CreateTime;
            DateTime min = max;
            foreach (var item in this.logs)
            {
                if (item.CreateTime > max)
                {
                    max = item.CreateTime;
                }
                else if (item.CreateTime < min)
                {
                    min = item.CreateTime;
                }
            }
            var diff = max - min;
            return diff.TotalSeconds / Convert.ToDouble(this.logs.Count);
        }

        public static void BuildListViewHeader(ListView view)
        {
            view.Items.Clear();
            view.Columns.Add("来源");
            view.Columns.Add("类型");
            view.Columns.Add("句柄个数");
            view.Columns.Add("总大小(KiB)");
            view.Columns.Add("平均生成时间(秒)");
        }

        public ListViewItem ToListViewItem()
        {
            var item = new ListViewItem();
            item.SubItems.Add(this.GetHandleType());
            item.SubItems.Add(this.GetHandleCount().ToString());
            item.SubItems.Add((this.GetHandleSize() / 1024).ToString());
            item.SubItems.Add(this.GetAverageCreateIntervalSeconds().ToString("#0.0"));
            item.Text = this.GetHandleOwner();
            return item;
        }
    }

    internal class SourceModHandleDumpCounterCompareResult
    {
        public SourceModHandleDumpCounterCompareResult()
        {
        }

        public static SourceModHandleDumpCounterCompareResult DoCompare(
            SourceModHandleDumpCounter? old,
            SourceModHandleDumpCounter? newOne)
        {

            var result = new SourceModHandleDumpCounterCompareResult();
            if (old == null && newOne == null) { return result; }
            var good = old == null ? newOne : old;
            if (good == null) { return result; }
            old ??= new SourceModHandleDumpCounter();
            newOne ??= new SourceModHandleDumpCounter();
            result.Owner = good.GetHandleOwner();
            result.Type = good.GetHandleType();
            result.HandleCount = newOne.GetHandleCount() - old.GetHandleCount();
            return result;
        }

        public string Owner { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int HandleCount { get; set; } = 0;

        public static void BuildListViewHeader(ListView view)
        {
            view.Items.Clear();
            view.Columns.Add("来源");
            view.Columns.Add("类型");
            view.Columns.Add("句柄个数");
        }

        public ListViewItem ToListViewItem()
        {
            var item = new ListViewItem();
            item.SubItems.Add(this.Type);
            var str = this.HandleCount.ToString();
            if (this.HandleCount > 0)
            {
                str = "+" + str;
            }
            item.SubItems.Add(str);
            item.Text = this.Owner;
            return item;
        }
    }
}
