using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.RemoveFile
{
    public class RemoveFileRemoteServiceProvider : RemoveFileBaseProvider, IRemoveFileProvider
    {
        public OptionsRemoveFile options { get; set; }
        private HttpContext context { get; set; }

        public RemoveFileRemoteServiceProvider(HttpContext _context, OptionsRemoveFile _options)
        {
            options = _options;
            context = _context;
        }
        public void Remove(IHostingEnvironment hostingEnv, string filename, string path)
        {
        }
    }
}
