using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Middlewares;

namespace WebCore.Component.Extensions
{
    public static class MiddlewaresHealthExtensions
    {
        /// <summary>
        /// 使用心跳包功能，用于consul等工具的心跳监听
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHealth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareHealth>();
        }
    }
}
