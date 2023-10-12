using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyiTools
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string? message)
        {
            message ??= "null";
            this.Message = message;
        }

        public string Message { get; }
    }

    public delegate void LogEventHandler(object sender, LogEventArgs e);

}
