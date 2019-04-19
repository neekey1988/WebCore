using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using WebCore.Component.Options;
using WebCore.Common.Share;
using WebCore.Component.Providers.Upload;

namespace WebCore.Component.Builders
{
    public interface IUploadBuilder
    {
        IUploadProvider Build(string router);
    }

    public class UploadBuilder : IUploadBuilder
    {
        private IOptions<List<OptionsUpload>> options { get; set; }
        public UploadBuilder(IOptions<List<OptionsUpload>> _options)
        {
            this.options = _options;
        }


        public IUploadProvider Build(string router) {
            var opts = options.Value.SingleOrDefault(s=>s.Router.ToLower().TrimEnd(new char[] { '/', '\\' }) == router.ToLower().TrimEnd(new char[] { '/', '\\' }));
            if (opts==null)
                return null;
            AssemblyHelper ass = new AssemblyHelper();
            var provider=(IUploadProvider)ass.CreateInstance(opts.AssemblyName,opts.ClassName, opts);
            return provider;
        }
    }
}
