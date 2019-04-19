using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsRepository : IOptions<OptionsRepository>
    {
        public Dictionary<string, string> DbContexts { get; set; }
        public OptionsRepository Value => this;

    }
}
