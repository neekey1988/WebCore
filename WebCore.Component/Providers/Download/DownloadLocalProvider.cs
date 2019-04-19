using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using WebCore.Common.Extensions;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.Download
{
    public class DownloadLocalProvider : DownloadBaseProvider, IDownloadProvider
    {
        public OptionsDownload options { get;set;}
        private HttpContext context { get; set; }
        private FileExtensionContentTypeProvider mimes { get; set; }

        public DownloadLocalProvider(HttpContext _context,OptionsDownload _options, FileExtensionContentTypeProvider _mimes) {
            options = _options;
            context = _context;
            mimes = _mimes;
        }

        public void Download(IHostingEnvironment hostingEnv,string filename,string path)
        {
            //要下载的文件地址，这个文件会被分成片段，通过循环逐步读取到ASP.NET Core中，然后发送给客户端浏览器
            var filePath =(string.IsNullOrEmpty(options.Root)?hostingEnv.ContentRootPath.TrimEnd(new char[] { '/','\\'})+"/":options.Root)+path.TrimStart(new char[] { '/', '\\' });
            if (!System.IO.File.Exists(filePath))
            {
                context.Result404();
                return;
            }


            var fileExt = Path.GetExtension(filePath);
            //这就是ASP.NET Core循环读取下载文件的缓存大小，这里我们设置为了1024字节，也就是说ASP.NET Core每次会从下载文件中读取1024字节的内容到服务器内存中，然后发送到客户端浏览器，这样避免了一次将整个下载文件都加载到服务器内存中，导致服务器崩溃
            int bufferSize = 1024;

            //由于我们下载的是一个Excel文件，所以设置ContentType为application/vnd.ms-excel
            context.Response.ContentType = this.mimes.Mappings.Keys.Contains(fileExt)? this.mimes.Mappings[fileExt]: "application/octet-stream";

            //在Response的Header中设置下载文件的文件名，这样客户端浏览器才能正确显示下载的文件名，注意这里要用HttpUtility.UrlEncode编码文件名，否则有些浏览器可能会显示乱码文件名
            var contentDisposition = "attachment;" + "filename=" + WebUtility.UrlEncode(filename);
            context.Response.Headers.Add("Content-Disposition", new string[] { contentDisposition });

            //使用FileStream开始循环读取要下载文件的内容
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                //调用Response.Body.Dispose()并不会关闭客户端浏览器到ASP.NET Core服务器的连接，之后还可以继续往Response.Body中写入数据
                using (context.Response.Body)
                {
                    //获取下载文件的大小
                    long contentLength = fs.Length;
                    //在Response的Header中设置下载文件的大小，这样客户端浏览器才能正确显示下载的进度
                    context.Response.ContentLength = contentLength;

                    byte[] buffer;
                    long hasRead = 0;//变量hasRead用于记录已经发送了多少字节的数据到客户端浏览器

                    //如果hasRead小于contentLength，说明下载文件还没读取完毕，继续循环读取下载文件的内容，并发送到客户端浏览器
                    while (hasRead < contentLength)
                    {
                        //HttpContext.RequestAborted.IsCancellationRequested可用于检测客户端浏览器和ASP.NET Core服务器之间的连接状态，如果HttpContext.RequestAborted.IsCancellationRequested返回true，说明客户端浏览器中断了连接
                        if (context.RequestAborted.IsCancellationRequested)
                        {
                            //如果客户端浏览器中断了到ASP.NET Core服务器的连接，这里应该立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                            break;
                        }

                        buffer = new byte[bufferSize];

                        int currentRead = fs.Read(buffer, 0, bufferSize);//从下载文件中读取bufferSize(1024字节)大小的内容到服务器内存中

                        context.Response.Body.Write(buffer, 0, currentRead);//发送读取的内容数据到客户端浏览器
                        context.Response.Body.Flush();//注意每次Write后，要及时调用Flush方法，及时释放服务器内存空间

                        hasRead += currentRead;//更新已经发送到客户端浏览器的字节数
                    }
                }
            }

        }
    }
}
