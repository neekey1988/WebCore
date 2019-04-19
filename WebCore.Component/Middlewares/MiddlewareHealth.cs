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
    public class MiddlewareHealth
    {
        private RequestDelegate next;
        private IHostingEnvironment hostingEnv;

        public MiddlewareHealth(RequestDelegate _next, IHostingEnvironment _hostingEnv)
        {
            this.next = _next;
            this.hostingEnv = _hostingEnv;
        }

        public async Task Invoke(HttpContext context, IOptions<OptionsHealth> _options)
        {
            if (_options == null)
            {
                await this.next(context);
                return;
            }
            if (_options.Value.Router.ToLower().TrimEnd(new char[] { '/', '\\' }) != context.Request.Path.Value.ToLower().TrimEnd(new char[] { '/', '\\' }))
            {
                await this.next(context);
                return;
            }

            switch (_options.Value.ResultType)
            {
                case "xml":
                    context.Response.ContentType = "text/xml";
                    break;
                case "json":
                    context.Response.ContentType = "application/Json";
                    break;
                default:
                    break;
            }
            await context.Response.WriteAsync(_options.Value.ResultValue);
        }
    }
}
