using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;

namespace WebCore.Common.Extensions
{
    public static class WebHostExtensions
    {
        /// <summary>
        /// 设置网站的基础信息
        /// </summary>
        /// <param name="wh"></param>
        /// <param name="config"></param>
        public static void SetupBaseData(this IWebHostBuilder wh, IConfigurationRoot config)
        {
            //配置上传大小
            int maxRequestBodySize = 30000000;//28.60mb
            if (config.GetSection("GlobalSys:MaxRequestBodySize").Value != null)
                maxRequestBodySize = int.Parse(config.GetSection("GlobalSys:MaxRequestBodySize").Value);

            //设置启动方式
            string ServerMode = "";
            if (config.GetSection("GlobalSys:ServerMode").Value != null)
                ServerMode = config.GetSection("GlobalSys:ServerMode").Value;
            switch (ServerMode.ToLower())
            {
                case "kestrel":
                    wh.UseKestrel(options => options.Limits.MaxRequestBodySize = maxRequestBodySize);
                    break;
                case "httpsys":
                    wh.UseHttpSys(options => options.MaxRequestBodySize = maxRequestBodySize);
                    break;
                default:
                    wh.UseKestrel(options => options.Limits.MaxRequestBodySize = maxRequestBodySize);
                    break;
            }
        }

        /// <summary>
        /// 设置要监听的ip和端口
        /// </summary>
        /// <param name="wh"></param>
        /// <param name="config"></param>
        public static void SetupUrlListen(this IWebHostBuilder wh, IConfigurationRoot config)
        {
            List<string> urls = new List<string>();
            config.GetSection("GlobalSys:AppUrl").Bind(urls);

            if (urls.Count != 0)
                wh.UseUrls(urls.ToArray());
        }

        /// <summary>
        /// 设置系统自带的log插件相关的配置
        /// </summary>
        /// <param name="wh"></param>
        /// <param name="config"></param>
        public static void SetupLogging(this IWebHostBuilder wh)
        {
            wh.ConfigureLogging((hostContext, logging) =>
            {
                //aspnetcore的log组件是在build中添加的，添加之后会输出很多无用信息，所以在build之前把这些信息过滤掉
                //logging.AddFilter("System", LogLevel.Warning);
                //logging.AddFilter("Microsoft", LogLevel.Warning);
                //logging.ClearProviders();
            });
        }
    }
}
