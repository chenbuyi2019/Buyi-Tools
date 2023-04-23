using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace BuyiTools
{
    internal abstract class InputData
    {
        private static readonly string configPath = Path.Combine(AppContext.BaseDirectory, "data.json");
        private static Dictionary<string, string> dict = new();

        public static void ReadFromFile()
        {
            try
            {
                if (!File.Exists(configPath)) { return; }
                var j = File.ReadAllText(configPath);
                var d = JsonSerializer.Deserialize<Dictionary<string, string>>(j);
                if (d == null) { return; }
                dict = d;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"无法读取配置 {ex}");
            }
        }

        public static string GetValue(string key, string? fallback = null)
        {
            if (fallback == null) { fallback = string.Empty; }
            if (string.IsNullOrWhiteSpace(key)) { return fallback; }
            dict.TryGetValue(key, out string? value);
            if (value == null) { return fallback; }
            return value;
        }

        public static void SetValue(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) { return; }
            dict.Remove(key);
            if (value == null) { return; }
            dict.Add(key, value);
        }

        private static string GetControlKeyName(Control ct)
        {
            if (ct.IsDisposed) { return string.Empty; }
            var parent = ct.Parent;
            if (parent == null || parent.IsDisposed) { return string.Empty; }
            return $"{parent.GetType().Name}-{ct.Name}";
        }

        public static string GetValueToControl(Control ct, string? fallback = null)
        {
            var key = GetControlKeyName(ct);
            var value = GetValue(key, fallback);
            ct.Text = value;
            return value;
        }

        public static void SetValueFromControl(Control ct, string? fallback = null)
        {
            if (ct.IsDisposed) { return; }
            var value = ct.Text;
            var key = GetControlKeyName(ct);
            SetValue(key, value);
        }

        public static void SaveToFile()
        {
            try
            {
                var j = JsonSerializer.Serialize(dict);
                File.WriteAllText(configPath, j);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"无法保存配置 {ex}");
            }
        }

    }
}
