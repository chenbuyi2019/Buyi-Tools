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
        #region 日志
        public event EventHandler<string>? LogSent;

        /// <summary>
        /// 输出一条日志
        /// </summary>
        protected void Log(string msg)
        {
            LogSent?.Invoke(this, msg);
        }
        #endregion

        #region 数据保存
        private readonly List<Control> RegisteredAutoSaveContorls = new();

        private string GetControlSaveKeyName(Control ct)
        {
            if (ct.IsDisposed) { return string.Empty; }
            var parent = ct.Parent;
            if (parent == null || parent.IsDisposed) { return string.Empty; }
            return $"{this.Name}-{ct.Name}".ToLower();
        }

        /// <summary>
        /// 注册控件，注册时会读取上次保存的信息，然后在操作工具的按钮时自动保存信息
        /// </summary>
        protected void RegisterContorlSaveData(params Control[] controls)
        {
            foreach (var ct in controls)
            {
                var ok = true;
                var key = GetControlSaveKeyName(ct);
                if (ct is TextBoxBase)
                {
                    ct.Text = InputData.GetString(key, ct.Text);
                }
                else if (ct is NumericUpDown)
                {
                    var n = (NumericUpDown)ct;
                    n.Value = Math.Clamp(InputData.GetNumber(key, n.Value), n.Minimum, n.Maximum);
                }
                else
                {
                    ok = false;
                    Log($"无法注册控件自动保存 {ct.Name} {ct.GetType().FullName}");
                }
                if (ok) { RegisteredAutoSaveContorls.Add(ct); }
            }
        }

        protected void SaveControlsDataToFile()
        {
            if (RegisteredAutoSaveContorls.Count < 1) { return; }
            try
            {
                InputData.ReadFromFile();
                foreach (var ct in RegisteredAutoSaveContorls)
                {
                    var key = GetControlSaveKeyName(ct);
                    if (ct is TextBoxBase)
                    {
                        InputData.SetString(key, ct.Text);
                    }
                    else if (ct is NumericUpDown)
                    {
                        var n = (NumericUpDown)ct;
                        InputData.SetNumber(key, n.Value);
                    }
                }
                InputData.SaveToFile();
            }
            catch (Exception ex)
            {
                Log($"无法保存操作信息 {ex.Message}");
            }
        }
        #endregion

        #region 工作
        private Exception? _lastError = null;
        public Exception? LastError
        {
            get
            {
                return _lastError;
            }
        }

        /// <summary>
        /// 执行操作，返回出错的信息
        /// </summary>
        protected Exception? DoWork(Action action)
        {
            SetFullProgress(0);
            this.Invoke(() =>
            {
                this.Enabled = false;
            });
            SaveControlsDataToFile();
            Exception? error = null;
            try
            {
                action();
                Log($"工作完成");
            }
            catch (Exception ex)
            {
                error = ex;
                Log($"出错 {ex.Message}");
                _lastError = ex;
            }
            Utils.CooldownControl(this);
            return error;
        }

        protected Task<Exception?> DoWorkAsync(Action action)
        {
            var t = new Task<Exception?>(() =>
            {
                return DoWork(action);
            });
            t.Start();
            return t;
        }
        #endregion

        #region 工作进度 
        private long fullProgress = 0;
        private long currentProgress = 0;

        /// <summary>
        /// 工作的进度，是 0.00 - 1.00 之间的一个小数
        /// </summary>
        public double WorkProgress
        {
            get
            {
                if (fullProgress < 1) { return 0; }
                if (currentProgress > fullProgress) { return 1; }
                return Convert.ToDouble(currentProgress) / Convert.ToDouble(fullProgress);
            }
        }

        protected void SetFullProgress(long num)
        {
            currentProgress = 0;
            fullProgress = num < 1 ? 0 : num;
        }

        protected void AddProgress(long add = 1)
        {
            if (add < 1) { return; }
            currentProgress = Math.Min(add + currentProgress, fullProgress);
        }

        #endregion

        #region 控件辅助
        /// <summary>
        /// 注册文本框控件，允许他们可以被拖拽放置文件、文件夹列表
        /// </summary>
        protected static void RegisterTextboxDropFilePath(params TextBoxBase[] controls)
        {
            void OnDragOver(object? sender, DragEventArgs e)
            {
                e.Effect = DragDropEffects.Copy;
            }
            void OnDragDrop(object? sender, DragEventArgs e)
            {
                if (sender == null || sender is not TextBoxBase) { return; }
                var txt = (TextBoxBase)sender;
                var data = e.Data?.GetData(DataFormats.FileDrop);
                if (data == null || data is not IEnumerable<string>) { return; }
                var array = (IEnumerable<string>)data;
                var sb = new StringBuilder();
                foreach (var v in array)
                {
                    if (string.IsNullOrEmpty(v)) { continue; }
                    if (sb.Length > 0) { sb.AppendLine(); }
                    sb.Append(v);
                    if (!txt.Multiline) { break; }
                }
                if (sb.Length > 0) { txt.Text = sb.ToString(); }
            }
            foreach (var ct in controls)
            {
                ct.AllowDrop = true;
                ct.DragOver += OnDragOver;
                ct.DragDrop += OnDragDrop;
            }
        }

        private void CheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = MenuCheckBoxList.Parent;
            Log($"parent: {p}");
        }
        #endregion
    }
}
