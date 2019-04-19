using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsMIME : IOptions<OptionsMIME>
    {
        public Dictionary<string,string> DictMIME { get; set; }
        public OptionsMIME Value => this;
    }
}
