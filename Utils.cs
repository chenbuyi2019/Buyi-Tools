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

        /// <summary>
        /// 最简单的忽略大小写比较
        /// </summary>
        public static StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// 把字符串转化为采用 linux 风格的/斜杠，并去掉头尾的双引号、空格的路径
        /// </summary>
        public static string MakeCleanPath(string str)
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
                UseShellExecute = false
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
        public static void MakeControlCooldown(Control ct, int timeoutMs = 1500)
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
                    ct.Enabled = true;
                });
            });
            t1.Start();
            form.Invoke(() =>
            {
                ct.Enabled = false;
            });
        }
    }
}
