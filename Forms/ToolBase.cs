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

        /// <summary>
        /// 输出一条日志
        /// </summary>
        protected void Log(string msg)
        {
            if (LogSent == null) { return; }
            LogSent(this, msg);
        }

        private string GetControlSaveKeyName(Control ct)
        {
            if (ct.IsDisposed) { return string.Empty; }
            var parent = ct.Parent;
            if (parent == null || parent.IsDisposed) { return string.Empty; }
            return $"{this.Name}-{ct.Name}".ToLower();
        }

        private readonly List<Control> RegisteredContorls = new();

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
                if (ok) { RegisteredContorls.Add(ct); }
            }
        }

        protected void SaveControlsDataToFile()
        {
            if (RegisteredContorls.Count < 1) { return; }
            try
            {
                InputData.ReadFromFile();
                foreach (var ct in RegisteredContorls)
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

        /// <summary>
        /// 执行操作，返回出错的信息
        /// </summary>
        protected Exception? DoWork(Action action)
        {
            this.Enabled = false;
            SaveControlsDataToFile();
            Exception? error = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                error = ex;
                Log($"出错 {ex.Message}");
            }
            Utils.CooldownControl(this);
            return error;
        }

    }
}
