using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;
using WebCore.Component.Services;

namespace WebCore.Component.Extensions
{
    public static class ServiceSwaggerExtensions
    {
        /// <summary>
        /// 添加Swagger服务,用于生成api说明页
        /// swagger必须同时添加服务和中间件，配置swaggerdoc到appsetting中，并且在项目属性的生成中勾选xml文档文件，然后在controller或action上添加特性(ApiMethodGroup\ApiClassGroup)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddSwagger(this IServiceCollection services, Action<OptionsSwagger> action)
        {
            OptionsSwagger options = new OptionsSwagger();
            action.Invoke(options);
            ServiceSwagger srv = new ServiceSwagger(services, options);
            srv.AddSwaggerDocs();
        }

    }

}
