using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsCache : IOptions<OptionsCache>
    {
        public bool Enable { get; set; }
        public double Expiration { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public OptionsCache Value => this;
    }
}
