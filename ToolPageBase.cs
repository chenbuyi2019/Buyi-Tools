
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BuyiTools
{
    public class ToolPageBase : Page, INotifyPropertyChanged
    {
        public ToolPageBase() : base()
        {
        }

        public MetroWindow? Window { get; set; } = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void CallPropertyChanged(string name)
        {
            this.Dispatcher.Invoke(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            });
        }

        public event LogEventHandler? LogEvent;

        public void Log(string? msg)
        {
            LogEvent?.Invoke(this, new LogEventArgs(msg));
        }

        public static void RegisterTextboxDropFilePath(params TextBox[] textbox)
        {
            static void OnDragOver(object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.All;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
                e.Handled = false;
            }
            static void OnDragDrop(object sender, DragEventArgs e)
            {
                if (sender == null || sender is not TextBox txt) { return; }
                var data = e.Data?.GetData(DataFormats.FileDrop);
                if (data == null || data is not IEnumerable<string> array) { return; }
                var sb = new StringBuilder();
                foreach (var v in array)
                {
                    if (string.IsNullOrEmpty(v)) { continue; }
                    if (sb.Length > 0) { sb.AppendLine(); }
                    sb.Append(v);
                    if (!txt.AcceptsReturn) { break; }
                }
                if (sb.Length > 0) { txt.Text = sb.ToString(); }
            }
            foreach (var item in textbox)
            {
                item.AllowDrop = true;
                item.AddHandler(TextBox.DragOverEvent, new DragEventHandler(OnDragOver), true);
                item.AddHandler(TextBox.DropEvent, new DragEventHandler(OnDragDrop), true);
            }
        }

        #region 数据保存

        private readonly List<Control> RegisteredAutoSaveContorls = new();

        private string GetControlSaveKeyName(Control ct)
        {
            return $"{this.GetType().Name}-{ct.Name}".ToLower();
        }

        /// <summary>
        /// 注册控件，注册时会读取上次保存的信息，然后在操作工具的按钮时自动保存信息
        /// </summary>
        protected void RegisterContorlSaveData(params Control[] controls)
        {
            SavedData.ReadFromFile();
            foreach (var ct in controls)
            {
                var ok = true;
                var key = GetControlSaveKeyName(ct);
                if (ct is TextBox textbox)
                {
                    textbox.Text = SavedData.GetString(key, textbox.Text);
                }
                else if (ct is NumericUpDown n)
                {
                    var old = Convert.ToDecimal(n.Value ?? 0);
                    n.Value = Math.Clamp(Convert.ToDouble(SavedData.GetNumber(key, old)), n.Minimum, n.Maximum);
                }
                else if (ct is ComboBox cbox)
                {
                    var target = SavedData.GetString(key, string.Empty);
                    if (!string.IsNullOrEmpty(target))
                    {
                        for (int i = 0; i < cbox.Items.Count; i++)
                        {
                            var txt = cbox.Items[i].ToString();
                            if (target.Equals(txt))
                            {
                                cbox.SelectedIndex = i;
                                break;
                            }
                        }
                    }
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
                SavedData.ReadFromFile();
                foreach (var ct in RegisteredAutoSaveContorls)
                {
                    var key = GetControlSaveKeyName(ct);
                    if (ct is TextBox textbox)
                    {
                        SavedData.SetString(key, textbox.Text);
                    }
                    else if (ct is NumericUpDown n)
                    {
                        SavedData.SetNumber(key, Convert.ToDecimal(n.Value ?? 0));
                    }
                    else if (ct is ComboBox cbox)
                    {
                        var obj = cbox.SelectedItem;
                        var s = string.Empty;
                        if (obj != null)
                        {
                            var tmp = obj.ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                s = tmp;
                            }
                        }
                        SavedData.SetString(key, s);
                    }
                }
                SavedData.SaveToFile();
            }
            catch (Exception ex)
            {
                Log($"无法保存操作信息 {ex.Message}");
            }
        }
        #endregion

        #region 工作
        public Exception? LastError = null;

        /// <summary>
        /// 执行操作，返回出错的信息
        /// </summary>
        protected Exception? DoWork(Action action)
        {
            this.Invoke(() =>
            {
                this.IsEnabled = false;
                SaveControlsDataToFile();
            });
            Exception? error = null;
            try
            {
                Log("\n================ 开始");
                action();
            }
            catch (Exception ex)
            {
                error = ex;
                Log($"出错 {ex.Message}");
                LastError = ex;
            }
            Log("\n=== 工作结束");
            Utils.MakeUICoolDown(this);
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
    }
}
