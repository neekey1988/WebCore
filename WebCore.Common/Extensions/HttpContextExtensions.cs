using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebCore.Common.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 转换源对象为json，并格式化里面的时间格式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format">时间格式，默认为yyyy-MM-dd</param>
        /// <returns></returns>
        public static void Result404(this HttpContext context)
        {
            context.Response.StatusCode = 404;
        }
    }
}
