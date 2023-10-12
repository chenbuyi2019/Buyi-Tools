using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyiTools
{
    public class ToolInfo
    {
        public ToolInfo(string title, string xamlName)
        {
            this.Title = title;
            this.PageUri = new Uri($"Tools/{xamlName}.xaml", UriKind.Relative);
        }

        public string Title { get; }
        public Uri PageUri { get; }

        public override string ToString()
        {
            return Title;
        }
    }
}
