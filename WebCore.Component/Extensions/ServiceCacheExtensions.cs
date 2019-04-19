using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Builders;
using WebCore.Component.Options;
using WebCore.Component.Services;

namespace WebCore.Component.Extensions
{
    public static class ServiceCacheExtensions
    {
        /// <summary>
        /// 添加自定义的Repository服务，完成dbcontext的注入，Repository的注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddCache(this IServiceCollection services, Action<OptionsCache> action)
        {
            OptionsCache options = new OptionsCache();
            action.Invoke(options);
            ServiceCache srv = new ServiceCache(services, options);
            srv.LoadCache();
        }

    }

}
