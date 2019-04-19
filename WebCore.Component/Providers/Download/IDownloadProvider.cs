using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.Download
{
    public interface IDownloadProvider
    {
        OptionsDownload options { get; set; }
        /// <summary>
        /// 请求下载
        /// </summary>
        /// <param name="hostingEnv">环境变量</param>
        /// <param name="filename">给客户端显示的文件名</param>
        /// <param name="path">数据库中保存的文件路径，含文件名</param>
        /// <returns></returns>
        void Download(IHostingEnvironment hostingEnv, string filename, string path);
    }
}
