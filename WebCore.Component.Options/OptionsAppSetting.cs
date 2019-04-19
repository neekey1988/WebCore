using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsAppSetting : IOptions<OptionsAppSetting>
    {
        /// <summary>
        /// 获取配置文件项
        /// </summary>
        public Dictionary<string,string> Items { get; set; }

        public string this[string key] {
            get {
                return this.Items[key];
            }
            set {
                this.Items[key] = value; 
            }
        }
        public OptionsAppSetting Value => this;
    }
}
