using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCore.Component.Builders;
using WebCore.Component.Options;
using WebCore.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using WebCore.Component.Providers.RemoveFile;

namespace WebCore.Component.Middlewares
{
    public class MiddlewareRemoveFile
    {
        private RequestDelegate next;
        private IOptions<List<OptionsRemoveFile>> options;
        private IHostingEnvironment hostingEnv;

        public MiddlewareRemoveFile(RequestDelegate _next, IHostingEnvironment _hostingEnv,IOptions<List<OptionsRemoveFile>> _options)
        {
            this.next = _next;
            this.options = _options;
            this.hostingEnv = _hostingEnv;
        }

        public async Task Invoke(HttpContext context)
        {
            IRemoveFileProvider removeFileProvider= new RemoveFileBuilder(this.options).Build(context);
            if (removeFileProvider == null)
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
            removeFileProvider.Remove(this.hostingEnv,filename,path);
            return;
        }
    }
}
