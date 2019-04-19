using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebCore.Common.Share;
using WebCore.Common.Extensions;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.RemoveFile
{
    public class RemoveFileLocalProvider : RemoveFileBaseProvider, IRemoveFileProvider
    {
        public OptionsRemoveFile options { get;set;}
        private HttpContext context { get; set; }

        public RemoveFileLocalProvider(HttpContext _context, OptionsRemoveFile _options) {
            options = _options;
            context = _context;
        }

        public void Remove(IHostingEnvironment hostingEnv,string filename,string path)
        {
            var filePath =(string.IsNullOrEmpty(options.Root)?hostingEnv.ContentRootPath.TrimEnd(new char[] { '/','\\'})+"/":options.Root)+path.TrimStart(new char[] { '/', '\\' });
            try
            {
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                context.Response.WriteAsync(new MessageData() { code = 0, data = null, msg = "删除成功", status = true }.ToJson());
            }
            catch (Exception)
            {
                context.Response.WriteAsync(new MessageData() { code = 0, data = null, msg = "删除失败", status = false }.ToJson());
            }

        }
    }
}
