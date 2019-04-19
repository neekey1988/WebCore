using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.Upload
{
    public interface IUploadProvider
    {
        OptionsUpload options { get; set; }
        bool Upload(IHostingEnvironment hostingEnv,IFormFile file, out dynamic result);
    }
}
