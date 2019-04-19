using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsServiceInfo : IOptions<OptionsServiceInfo>
    {
        /// <summary>
        /// 当前服务对外的ip，从当前主机所有网卡里面选取，为空默认匹配第一个，也支持模糊匹配，会从当前主机网卡的所有ip中模糊匹配“ServiceIP%”
        /// </summary>
        public string ServiceIP { get; set; }
        public int ServicePort { get; set; }
        /// <summary>
        /// 服务名，通过该名称获取提供该服务的所有节点ip
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 要注册到服务发现的ip
        /// </summary>
        public string RegisterIP { get; set; }
        public int RegisterPort { get; set; }
        /// <summary>
        /// 心跳检测时间间隔
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        public OptionsServiceInfo Value => this;
    }
}
