using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuyiTools
{
    internal static class SavedData
    {
        private static readonly string configPath = Path.Combine(AppContext.BaseDirectory, "data.json");
        private static Dictionary<string, string> dict = new();

        public static void ReadFromFile()
        {
            if (!File.Exists(configPath)) { return; }
            var j = File.ReadAllText(configPath);
            var d = JsonSerializer.Deserialize<Dictionary<string, string>>(j);
            if (d == null) { return; }
            dict = d;
        }

        public static void SaveToFile()
        {
            var j = JsonSerializer.Serialize(dict);
            File.WriteAllText(configPath, j);
        }

        public static string GetString(string key, string? fallback = null)
        {
            if (fallback == null) { fallback = string.Empty; }
            if (string.IsNullOrWhiteSpace(key)) { return fallback; }
            dict.TryGetValue(key, out string? value);
            if (value == null) { return fallback; }
            return value;
        }

        public static void SetString(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) { return; }
            dict.Remove(key);
            if (value == null) { return; }
            dict.Add(key, value);
        }

        public static decimal GetNumber(string key, decimal fallback = 0)
        {
            var str = GetString(key);
            if (str.Length > 0 && decimal.TryParse(str, out decimal num)) { return num; }
            return fallback;
        }

        public static void SetNumber(string key, decimal value)
        {
            SetString(key, value.ToString());
        }

    }
}
