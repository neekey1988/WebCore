using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;
using WebCore.Component.Middlewares;
using WebCore.Component.Providers.ServiceRegister;

namespace WebCore.Component.Extensions
{
    public static class MiddlewaresServiceRegisterExtensions
    {
        /// <summary>
        /// 执行服务注册
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void ExecServiceRegister(this IApplicationBuilder app,IApplicationLifetime lifetime, OptionsServiceInfo serviceInfo, OptionsHealth health)
        {
            if (serviceInfo==null || health==null)
                return;
            if (string.IsNullOrEmpty(serviceInfo.ServiceName) || string.IsNullOrEmpty(health.Router))
                return;
            serviceInfo.ServiceIP=Common.Share.IPHelper.GetNetWork(serviceInfo.ServiceIP);

            IServiceRegisterProvider provider = new ConsulProvider();//todo:后续要改成工厂
            provider.Register(lifetime, serviceInfo, health);
        }
    }
}
