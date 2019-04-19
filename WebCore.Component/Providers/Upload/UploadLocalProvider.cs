using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.Upload
{
    public class UploadLocalProvider : UploadBaseProvider, IUploadProvider
    {
        public OptionsUpload options { get;set;}

        public UploadLocalProvider(OptionsUpload _options) {
            options = _options;
        }
        public bool Upload(IHostingEnvironment hostingEnv,IFormFile file, out dynamic result)
        {
            FileStream fs=null;
            try
            {
                var path = BuildPath(file.FileName, options, hostingEnv, options.DirectoryName);
                CreateDir(path);
                using (fs = System.IO.File.Create(path))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                //path返回的是要保存在数据库中的相对路径
                path = path.Replace(hostingEnv.ContentRootPath, "");
                if (!string.IsNullOrEmpty(options.Root))
                    path = path.Replace(options.Root, "");
                result = new { filename=file.FileName,path=path};
            }
            catch (Exception ex)
            {
                log.InfoAsync($"上传{file.FileName}文件失败：{ex.Message}");
                result = $"上传文件失败：{ex.Message}";
                if (fs != null)
                    fs.Dispose();
                return false;
            }
            return true;
        }
    }
}
