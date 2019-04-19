using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;
using WebCore.Component.Services;

namespace WebCore.Component.Extensions
{
    public static class ServiceMIMEExtensions
    {
        /// <summary>
        /// 添加自定义的MIME服务，完成新的MIME的添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddMIME(this IServiceCollection services, Action<OptionsMIME> action)
        {
            OptionsMIME options = new OptionsMIME();
            action.Invoke(options);
            ServiceMIME srv = new ServiceMIME(services, options);
            srv.AddMIME();
        }

    }

}
