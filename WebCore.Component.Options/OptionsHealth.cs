using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsHealth : IOptions<OptionsHealth>
    {
        /// <summary>
        /// 匹配路由
        /// </summary>
        public string Router { get; set; }
        /// <summary>
        /// 要返回的值
        /// </summary>
        public string ResultValue { get; set; }
        /// <summary>
        /// 要返回的格式 xml json 空为字符串
        /// </summary>
        public string ResultType { get; set; }

        public OptionsHealth Value => this;
    }
}
