using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;
using WebCore.Component.Middlewares;

namespace WebCore.Component.Extensions
{
    public static class MiddlewaresSwaggerExtensions
    {
        /// <summary>
        /// 使用Swagger显示api接口文档，路由/swagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void UseSwagger(this IApplicationBuilder app,OptionsSwagger options)
        {
            if (options==null||options.SwaggerDocs.Count==0)
                return;
            //启用swagger，并设每个服务文档的路由，documentName为服务名
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                //设置不同服务的终结点
                foreach (var item in options.Value.SwaggerDocs)
                {
                    c.SwaggerEndpoint($"/swagger/{item.Key}/swagger.json", item.Value.Title);
                }
                //swagger路由的前缀，这里通过/swagger 访问
                c.RoutePrefix = "swagger";
            });
        }
    }
}
