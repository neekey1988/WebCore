using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Common.Share;
using WebCore.Component.Builders;
using WebCore.Component.Options;
using WebCore.Component.Providers.Download;
using WebCore.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace WebCore.Component.Middlewares
{
    public class MiddlewareDownload
    {
        private RequestDelegate next;
        private IOptions<List<OptionsDownload>> options;
        private IHostingEnvironment hostingEnv;

        public MiddlewareDownload(RequestDelegate _next, IHostingEnvironment _hostingEnv,IOptions<List<OptionsDownload>> _options)
        {
            this.next = _next;
            this.options = _options;
            this.hostingEnv = _hostingEnv;
        }

        public async Task Invoke(HttpContext context, FileExtensionContentTypeProvider _mimes)
        {
            IDownloadProvider downloadProvider= new DownloadBuilder(this.options).Build(context,_mimes);
            if (downloadProvider == null)
            {
                await this.next(context);
                return;
            }
            if (!context.Request.HasFormContentType && context.Request.Query.Count==0)
            {
                context.Result404();
                return;
            }
            string filename = "";
            string path = "";
            if (context.Request.Query["path"].Count > 0)
                path = context.Request.Query["path"][0];
            if (context.Request.Query["filename"].Count > 0)
                filename = context.Request.Query["filename"][0];

            if (context.Request.HasFormContentType && context.Request.Form["path"].Count > 0)
                path = context.Request.Form["path"][0];
            if (context.Request.HasFormContentType && context.Request.Form["filename"].Count > 0)
                filename = context.Request.Form["filename"][0];
            if (string.IsNullOrEmpty(path))
            {
                context.Result404();
                return;
            }
            if (string.IsNullOrEmpty(filename))
                filename = System.IO.Path.GetFileName(path);
            path = System.Web.HttpUtility.UrlDecode(path);
            filename = System.Web.HttpUtility.UrlDecode(filename);
            downloadProvider.Download(this.hostingEnv,filename,path);
            return;
        }
    }
}
