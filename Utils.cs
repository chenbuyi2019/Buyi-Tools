using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyiTools
{
    internal abstract class Utils
    {

        public static readonly UTF8Encoding UTF8noBom = new(false);

        /// <summary>
        /// 最简单的忽略大小写比较
        /// </summary>
        public static readonly StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// 把字符串转化为采用 linux 风格的/斜杠，并去掉头尾的双引号、空格的路径
        /// </summary>
        public static string CleanPath(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) { return string.Empty; }
            return str.Trim().Replace("\\", "/").Replace("\"", "").Trim('/');
        }

        /// <summary>
        /// 运行命令行程序然后返回输出的结果。输出数组的第一个是 stdout ，第二个是 stderr
        /// </summary>
        public static string[] RunCmd(string program,
            IEnumerable<string>? args = null,
            string? workingDir = null,
            int timeoutMs = 5000)
        {
            var info = new ProcessStartInfo()
            {
                FileName = program,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            if (!string.IsNullOrWhiteSpace(workingDir))
            {
                info.WorkingDirectory = workingDir;
            }
            if (args != null)
            {
                foreach (var arg in args)
                {
                    info.ArgumentList.Add(arg);
                }
            }
            using var proc = Process.Start(info);
            if (proc == null) { throw new Exception($"无法启动 {program}"); }
            var finished = false;
            var t1 = new Task(() =>
            {
                Thread.Sleep(timeoutMs);
                if (finished) { return; }
                if (!proc.HasExited) { proc.Kill(); }
            });
            t1.Start();
            proc.WaitForExit(timeoutMs);
            finished = true;
            var stderr = proc.StandardError.ReadToEnd();
            var stdout = proc.StandardOutput.ReadToEnd();
            return new string[] { stdout, stderr };
        }

        /// <summary>
        /// 把控件 Enabled 设为 false ，然后在几秒后恢复为 true
        /// </summary>
        public static void CooldownControl(Control ct, int timeoutMs = 600)
        {
            if (timeoutMs < 1) { return; }
            var form = ct.FindForm();
            if (form == null || form.IsDisposed) { return; }
            var t1 = new Task(() =>
            {
                Thread.Sleep(timeoutMs);
                if (form.IsDisposed) { return; }
                form.Invoke(() =>
                {
                    if (ct.IsDisposed) { return; }
                    ct.Enabled = true;
                });
            });
            t1.Start();
            form.Invoke(() =>
            {
                ct.Enabled = false;
            });
        }

        /// <summary>
        /// 美化输出字节长度，比如 0B 15.1KB 50.1MB
        /// </summary>
        public static string FormatBytesLength(long len)
        {
            if (len < 0) { len = -len; }
            if (len < 800) { return $"{len}B"; }
            double v = Convert.ToDouble(len);
            v /= 1024;
            if (v < 800) { return $"{v:0.0}KB"; }
            v /= 1024;
            if (v < 1600) { return $"{v:0.0}MB"; }
            v /= 1024;
            return $"{v:0.0}GB";
        }

        /// <summary>
        /// 直接获取一个文件的大小，如果不存在或无法读取文件会返回-1
        /// </summary>
        public static long GetFileLength(string filename)
        {
            try
            {
                var info = new FileInfo(filename);
                return info.Length;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 读取流里的内容，直到遇到一个 NULL 字符
        /// </summary>
        public static string ReadNullTerminatedStr(Stream stream)
        {
            var ls = new List<byte>();
            while (true)
            {
                var v = stream.ReadByte();
                if (v < 1) { break; }
                ls.Add(Convert.ToByte(v));
            }
            if (ls.Count < 1) { return string.Empty; }
            return UTF8noBom.GetString(ls.ToArray());
        }

        /// <summary>
        /// 在数组里寻找 target 字符串，返回一致的字符串（忽略大小写）
        /// </summary>
        public static int FindStrInListIgnoreCase(IList<string> list, string target)
        {
            int ct = list.Count;
            for (int i = 0; i < ct; i++)
            {
                var str = list[i];
                if (target.Equals(str, IgnoreCase)) { return i; }
            }
            return -1;
        }

        /// <summary>
        /// 如果 str 是以 prefix 作为开头，就把 prefix 去掉。
        /// 默认不区分大小写。
        /// </summary>
        public static string StrTrimPrefix(string str,
            string prefix,
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(prefix)) { return str; }
            if (str.StartsWith(prefix, compare)) { return str.Substring(prefix.Length); }
            return str;
        }

        /// <summary>
        /// 如果 str 是以 suffix 作为结尾，就把 suffix 去掉。
        /// 默认不区分大小写。
        /// </summary>
        public static string StrTrimSuffix(string str,
            string suffix,
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(suffix)) { return str; }
            if (str.EndsWith(suffix, compare)) { return str.Substring(0, str.Length - suffix.Length); }
            return str;
        }

        private static readonly string p7zPath = Path.Combine(AppContext.BaseDirectory, "lib", "7z", "7z.exe");

        /// <summary>
        /// 执行 7z.exe ，如果有错误会抛出error
        /// </summary>
        public static void Run7zCmd(params string[] args)
        {
            var result = RunCmd(p7zPath, args, null, 1000 * 600);
            var err = result[1].Trim();
            if (err.Length >= 1)
            {
                throw new Exception(err);
            }
        }
    }
}
