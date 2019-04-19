using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Middlewares;

namespace WebCore.Component.Extensions
{
    public static class MiddlewaresRemoveFileExtensions
    {
        /// <summary>
        /// 使用删除文件中间件，该中间件会对物理文件进行删除，但不涉及到数据库操作
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRemoveFile
          (this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareRemoveFile>();
        }
    }
}
