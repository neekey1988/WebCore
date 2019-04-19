using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCore.Domain;
using WebCore.Domain.EntityIRepositorys;
using System.Threading.Tasks;
using WebCore.Common.Logging;
using WebCore.Common.Extensions;
using Newtonsoft.Json;
using WebCore.Common.Share;
using Microsoft.Extensions.Options;
using WebCore.Component.Options;
using WebCore.Common.Attributes;

namespace WebCore.Controllers
{
    public class DemoController : BaseController
    {
        public DemoController(IStudentRepository db, IOptions<List<OptionsDownload>> dlsetting)
        {
        }
        [HttpGet]
        [Route("api/rmfile")]
        public IActionResult Upload() {
            //throw new Exception("333");
            return View();
        }

        public string UploadFile(IFormFile form) {
            var files = Request.Form.Files;
            return JsonConvert.SerializeObject(new {
                status=true,url= "https://upload.jianshu.io/users/upload_avatars/10758270/16afc0b7-a2f8-4ac9-82fa-5a1e233badfe?imageMogr2/auto-orient/strip|imageView2/1/w/96/h/96"
            });
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <returns></returns>
        public string GetFiles()
        {
            var list = new List<Object>();
            list.Add(new { dbid = 1,  filename = "1.jpg", path = "/test/" });
            list.Add(new { dbid = 2,  filename = "2.png", path = "/test/" });
            list.Add(new { dbid = 3,  filename = "3.jpg", path = "/test/" });
            list.Add(new { dbid = 4,  filename = "4.jpg", path = "/test/" });
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="dbid">数据库中的文件id</param>
        /// <param name="path">文件路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public string RemoveFile(string dbid, string path, string filename)
        {
            return JsonConvert.SerializeObject(new MessageData()
            {
                code = 0,
                msg = "删除文件成功",
                status = true,
                data = null
            });
        }
    }
}
