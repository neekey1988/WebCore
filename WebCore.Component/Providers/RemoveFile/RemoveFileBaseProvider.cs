using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebCore.Common.Logging;
using WebCore.Common.Share;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.RemoveFile
{
    public class RemoveFileBaseProvider
    {
        public ILogHelper log = new Log4NetHelper();
        /// <summary>
        /// 生成文件路径，其中配置文件的path可设置参数
        /// </summary>
        /// <param name="filename">文件名，带后缀</param>
        /// <param name="path">配置文件重的path</param>
        /// <param name="hostingEnv">环境变量</param>
        /// <param name="dirname">参数dirname</param>
        /// <returns></returns>
        public string BuildPath(string filename, OptionsRemoveFile opts, IHostingEnvironment hostingEnv,string directoryname) {
            string path = opts.Root;// + opts.Path;
            path= PathHelper.BuildPath(path, hostingEnv.ContentRootPath, filename, directoryname);
            //转换相对路径为绝对路径
            if (opts.Root=="")
                path = hostingEnv.ContentRootPath + path;
            return path;
        }

    }
}
