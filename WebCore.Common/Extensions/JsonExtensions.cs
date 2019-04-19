using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Common.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// 转换源对象为json，并格式化里面的时间格式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format">时间格式，默认为yyyy-MM-dd</param>
        /// <returns></returns>
        public static string ToJson(this object data,string format= "yyyy-MM-dd")
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = format;
            return JsonConvert.SerializeObject(data, Formatting.Indented, timeFormat);
        }
    }
}
