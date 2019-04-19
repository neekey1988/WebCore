using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Middlewares;

namespace WebCore.Component.Extensions
{
    public static class MiddlewaresDownloadExtensions
    {
        /// <summary>
        /// 使用下载中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDownload
          (this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareDownload>();
        }
    }
}
