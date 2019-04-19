using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.Download
{
    public class DownloadRemoteServiceProvider : DownloadBaseProvider, IDownloadProvider
    {
        public OptionsDownload options { get; set; }
        private HttpContext context { get; set; }
        private FileExtensionContentTypeProvider mimes { get; set; }


        public DownloadRemoteServiceProvider(HttpContext _context,OptionsDownload _options, FileExtensionContentTypeProvider _mimes)
        {
            options = _options;
            context = _context;
            mimes = _mimes;
        }
        public void Download(IHostingEnvironment hostingEnv, string filename, string path)
        {
        }
    }
}
