using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebCore.Common.Attributes;
using WebCore.Common.Enums;
using WebCore.Common.Share;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{
    /// <summary>
    /// 文件服务
    /// </summary>
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <returns></returns>
        [ApiMethodGroup(E_ApiGroup.File, "[action]", E_HttpVerbs.GET, E_HttpVerbs.POST)]
        public string GetFiles()
        {
            var list = new List<Object>();
            list.Add(new { dbid = 1, filename = "1.jpg", path = "/test/" });
            list.Add(new { dbid = 2, filename = "2.png", path = "/test/" });
            list.Add(new { dbid = 3, filename = "3.jpg", path = "/test/" });
            list.Add(new { dbid = 4, filename = "4.jpg", path = "/test/" });
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="dbid">数据库中的文件id</param>
        /// <param name="path">文件路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveFile")]
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
