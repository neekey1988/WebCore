using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.IO;
using System.Text;
using WebCore.Component.Extensions;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;
using WebCore.Common.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.AspNetCore.Mvc;
using WebCore.Component.Options;
using System.Reflection;
using WebCore.Common.Attributes;
using AspectCore.Injector;
using AspectCore.Extensions.DependencyInjection;

namespace WebCore
{
    public class Startup
    {

        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        //public IConfiguration Configuration { get; }

        private IConfiguration Configuration;


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //添加mvc组件
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);//
            var config = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                             .Build();
            Configuration = config;

            //启用IOptions
            services.AddOptions();

            //通用键值对配置文件
            services.Configure<OptionsAppSetting>(config.GetSection("OptionsAppSetting"));
            //上传下载删除配置文件
            services.Configure<List<OptionsUpload>>(config.GetSection("OptionsUpload"));
            services.Configure<List<OptionsDownload>>(config.GetSection("OptionsDownload"));
            services.Configure<List<OptionsRemoveFile>>(config.GetSection("OptionsRemoveFile"));
            //心跳包配置文件
            services.Configure<OptionsHealth>(config.GetSection("OptionsHealth"));

            //添加自定义的Repository服务
            services.AddRepositorys(options => config.GetSection("OptionsRepository").Bind(options), config);
            services.AddMIME(options => config.GetSection("OptionsMIME").Bind(options));

            //添加Swagger服务
            services.AddSwagger(options=> config.GetSection("OptionsSwagger").Bind(options));

            //添加缓存服务
            services.AddCache(options => config.GetSection("OptionsCache").Bind(options));
            //services.AddDynamicProxy();
            return services.BuildAspectCoreServiceProvider();

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifeTime)
        {
            //if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            //else
            //    //使用全局异常捕获，并在error页面呈现
            //    app.UseExceptionHandler("/message/error");

            //使访问支持静态内容返回，不加这句话图片等静态资源都返回不了
            app.UseStaticFiles();
            //启用上传下载删除文件功能
            app.UseUpload();
            app.UseDownload();
            app.UseRemoveFile();
            //启用心跳包功能
            app.UseHealth();
            //把appsetting的配置加载到context.items["AppSetting"]中
            app.UseAppSetting();


            //var log=app.ApplicationServices.GetRequiredService<ILoggerFactory>().AddConsole().CreateLogger("aa");
            app.UseMvc(routes => routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"));

            //启用swagger
            var swaggerdoc = new OptionsSwagger();
            Configuration.GetSection("OptionsSwagger").Bind(swaggerdoc);
            app.UseSwagger(swaggerdoc);


            //发现服务（consul等）的配置文件
            var serviceInfo = new OptionsServiceInfo();
            Configuration.GetSection("OptionsServiceInfo").Bind(serviceInfo);
            //心跳包的配置文件
            var health = new OptionsHealth();
            Configuration.GetSection("OptionsHealth").Bind(health);
            //执行服务注册到服务发现中心（consul等）
            app.ExecServiceRegister(appLifeTime, serviceInfo, health);
        }
    }
}
