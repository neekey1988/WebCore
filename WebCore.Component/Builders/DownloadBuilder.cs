using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using WebCore.Component.Options;
using WebCore.Common.Share;
using WebCore.Component.Providers.Download;
using Microsoft.AspNetCore.StaticFiles;

namespace WebCore.Component.Builders
{
    public interface IDownloadBuilder
    {
        IDownloadProvider Build(HttpContext context, FileExtensionContentTypeProvider _mimes);
    }

    public class DownloadBuilder : IDownloadBuilder
    {
        private IOptions<List<OptionsDownload>> options { get; set; }
        public DownloadBuilder(IOptions<List<OptionsDownload>> _options)
        {
            this.options = _options;
        }


        public IDownloadProvider Build(HttpContext _context, FileExtensionContentTypeProvider _mimes) {
            string _router = _context.Request.Path.Value;
            var opts = options.Value.SingleOrDefault(s=>s.Router.ToLower().TrimEnd(new char[] { '/','\\'})==_router.ToLower().TrimEnd(new char[] { '/', '\\' }));
            if (opts==null)
                return null;
            AssemblyHelper ass = new AssemblyHelper();
            var provider=(IDownloadProvider)ass.CreateInstance(opts.AssemblyName,opts.ClassName, _context, opts, _mimes);
            return provider;
        }
    }
}
