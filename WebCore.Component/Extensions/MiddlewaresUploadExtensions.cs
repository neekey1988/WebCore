using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Middlewares;

namespace WebCore.Component.Extensions
{
    public static class MiddlewaresUploadExtensions
    {
        /// <summary>
        /// 使用上传中间件，对于提交上传文件，该中间件会捕获到上传文件，并进行保存
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUpload
          (this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareUpload>();
        }
    }
}
