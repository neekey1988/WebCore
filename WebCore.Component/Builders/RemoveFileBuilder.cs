using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using WebCore.Component.Options;
using WebCore.Common.Share;
using WebCore.Component.Providers.RemoveFile;

namespace WebCore.Component.Builders
{
    public interface IRemoveFileBuilder
    {
        IRemoveFileProvider Build(HttpContext context);
    }

    public class RemoveFileBuilder : IRemoveFileBuilder
    {
        private IOptions<List<OptionsRemoveFile>> options { get; set; }
        public RemoveFileBuilder(IOptions<List<OptionsRemoveFile>> _options)
        {
            this.options = _options;
        }


        public IRemoveFileProvider Build(HttpContext _context) {
            string _router = _context.Request.Path.Value;
            var opts = options.Value.SingleOrDefault(s=>s.Router.ToLower().TrimEnd(new char[] { '/','\\'})==_router.ToLower().TrimEnd(new char[] { '/', '\\' }));
            if (opts==null)
                return null;
            AssemblyHelper ass = new AssemblyHelper();
            var provider=(IRemoveFileProvider)ass.CreateInstance(opts.AssemblyName,opts.ClassName, _context, opts);
            return provider;
        }
    }
}
