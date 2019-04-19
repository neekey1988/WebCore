using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Middlewares;

namespace WebCore.Component.Extensions
{
    public static class MiddlewaresAppSettingExtensions
    {
        /// <summary>
        /// 使用AppSetting功能，把appsetting加载到context上下文，通过context.items获取
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppSetting(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareAppSetting>();
        }
    }
}
