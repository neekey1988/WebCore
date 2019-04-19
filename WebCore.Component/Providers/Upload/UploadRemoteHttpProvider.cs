using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.Upload
{
    public class UploadRemoteHttpProvider : UploadBaseProvider, IUploadProvider
    {
        public OptionsUpload options { get; set; }

        public UploadRemoteHttpProvider(OptionsUpload _options)
        {
            options = _options;
        }
        public bool Upload(IHostingEnvironment hostingEnv,IFormFile file, out dynamic result)
        {
            result = "123";
            return true;
        }
    }
}
