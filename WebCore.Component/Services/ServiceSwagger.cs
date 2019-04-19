using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebCore.Domain;
using WebCore.Common.Logging;
using WebCore.Component.Options;
using Microsoft.AspNetCore.StaticFiles;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebCore.Common.Attributes;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebCore.Component.Services
{
    public class ServiceSwagger
    {
        private OptionsSwagger options;
        private IServiceCollection services;
        public ServiceSwagger(IServiceCollection _services, OptionsSwagger _options) {
            options = _options;
            services = _services;
        }

        /// <summary>
        /// 添加SwaggerDoc
        /// </summary>
        public void AddSwaggerDocs() {
            if (this.options.SwaggerDocs == null || this.options.SwaggerDocs.Count == 0)
                return;
            services.AddSwaggerGen(c =>{
                //添加SwaggerDocs
                foreach (var item in this.options.SwaggerDocs)
                    c.SwaggerDoc(item.Key, item.Value);
                //设置xml文档文件地址，这个是项目属性——生成——xml文档文件中的文件名
                var basePath = AppContext.BaseDirectory;
                var xmlPath = System.IO.Path.Combine(basePath, this.options.XmlCommentsName);
                c.IncludeXmlComments(xmlPath);
                //自定义生成文档的条件，
                //controller添加了特性ApiClassGroupAttribute，那么该控制器下的所有action，只要含有httpverbs和route，就都自动生成文档
                //action添加了特性ApiMethodGroupAttribute，那么只把当前action生成文档
                //docName为服务名，apiDesc为swagger对action的封装类
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    string classGroup = "";
                    //获取action的信息失败，就不生成这个action的文档
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    //查看action是否定义了httpverbs，没有定义的跳过
                    //if (!methodInfo.GetCustomAttributes(false).Any(a => typeof(IActionHttpMethodProvider).IsAssignableFrom(a.GetType()))) return false;
                    if (string.IsNullOrEmpty(apiDesc.HttpMethod)) return false;
                    //查看action上的ApiMethodGroupAttribute特性的GroupName是否和docName服务吗一致，一致就返回true，生成文档
                    var methodGroup = methodInfo.GetCustomAttributes<ApiMethodGroupAttribute>().Select(s=>s.GroupName);
                    if (methodGroup.Any(a => a == docName))
                        return true;
                    else
                    {
                        //查询当前action的controller是否添加了ApiClassGroup特性，添加了就比对ApiClassGroup的GroupName和docName服务吗一致，一致就返回true，生成文档
                        var attr = apiDesc.ControllerAttributes().Where(s=>s.ToString().Contains(nameof(ApiClassGroupAttribute)));
                        classGroup = attr.Count() == 0 ? "" : (attr.First() as ApiClassGroupAttribute).GroupName;
                        if (classGroup == docName)
                            return true;
                    }
                    return false;
                });
            });
        }
    }


}
