using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Common.Share;
using WebCore.Component.Builders;
using WebCore.Component.Options;
using WebCore.Component.Providers.Upload;
using WebCore.Common.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace WebCore.Component.Middlewares
{
    public class MiddlewareUpload
    {
        private RequestDelegate next;
        private IHostingEnvironment hostingEnv;

        public MiddlewareUpload(RequestDelegate _next, IHostingEnvironment _hostingEnv)
        {
            this.next = _next;
            this.hostingEnv = _hostingEnv;
        }

        public async Task Invoke(HttpContext context,IOptionsSnapshot<List<OptionsUpload>> options)
        {
            IUploadProvider uploadProvider= new UploadBuilder(options).Build(context.Request.Path.Value);
            if (uploadProvider == null)
            {
                await this.next(context);
                return;
            }
            //如果提交的数据不含有表单，context.Request.Form会异常，所以此处判断下长度
            if (!context.Request.HasFormContentType)
            {
                context.Result404();
                return;
            }
            var files =context.Request.Form.Files;
            //经过上面判断，说明匹配到了路由，但是附件是空的，那么直接返回，不用执行next了，因为路由是匹配的，但是没上传文件
            if (files.Count == 0)
            {
                context.Result404();
                return;
            }
            //理论上，上传都是通过post，应该不会出现get方式
            if (context.Request.Query["directoryname"].Count > 0)
                uploadProvider.options.DirectoryName = context.Request.Query["directoryname"][0];
            if (context.Request.Form["directoryname"].Count > 0)
                uploadProvider.options.DirectoryName = context.Request.Form["directoryname"][0];

            foreach (var item in files)
            {
                if (item.Length>uploadProvider.options.MaxLength)
                {
                    await context.Response.WriteAsync(new MessageData() {  status=false, msg="上传的文件中，有文件超过了系统限制大小"}.ToJson());
                    return;
                }
                if (!uploadProvider.options.Extension.Contains( System.IO.Path.GetExtension(item.FileName).Replace(".","")))
                {
                    await context.Response.WriteAsync(new MessageData() { status = false, msg = "上传的文件中，有文件的格式不符合要求" }.ToJson());
                    return;
                }
            }
            List<string> faileds = new List<string>();
            List<dynamic> success = new List<dynamic>();
            foreach (var item in files)
            {
                if (!uploadProvider.Upload(hostingEnv, item, out dynamic result))
                {
                    faileds.Add(item.FileName);
                    continue;
                }
                success.Add(result);
            }
            var resule = new MessageData()
            {
                status = faileds.Count==0? true:false,
                msg = faileds.Count > 0? "文件上传完毕,其中以下文件失败(" + string.Join(',', faileds.ToArray()) + ")" : "文件上传完毕",
                data=success
            };
            await context.Response.WriteAsync(resule.ToJson());
            return;
        }
    }
}
