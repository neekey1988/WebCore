using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebCore.Common.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //添加监视，变更配置文件会自动重新加载
            Common.Logging.Log4NetHelper.ConfigureAndWatch("Log4net.config");
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .Build();

            var wh= WebHost.CreateDefaultBuilder(args).UseContentRoot(Directory.GetCurrentDirectory()).UseStartup<Startup>();
            wh.SetupBaseData(config);
            wh.SetupLogging();
            wh.SetupUrlListen(config);
            return wh;
        }
    }
}
