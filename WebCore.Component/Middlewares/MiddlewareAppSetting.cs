using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Component.Builders;
using WebCore.Component.Options;
using WebCore.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using WebCore.Component.Providers.RemoveFile;

namespace WebCore.Component.Middlewares
{
    public class MiddlewareAppSetting
    {
        private RequestDelegate next;
        private IHostingEnvironment hostingEnv;

        public MiddlewareAppSetting(RequestDelegate _next, IHostingEnvironment _hostingEnv)
        {
            this.next = _next;
            this.hostingEnv = _hostingEnv;
        }

        public async Task Invoke(HttpContext context, IOptionsSnapshot<OptionsAppSetting> _options)
        {
            if (_options == null)
            {
                await this.next(context);
                return;
            }
            context.Items["AppSetting"] = _options==null?null:_options.Value;
            await this.next(context);
        }
    }
}
