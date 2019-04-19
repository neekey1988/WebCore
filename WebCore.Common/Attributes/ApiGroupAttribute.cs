using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebCore.Common.Enums;

namespace WebCore.Common.Attributes
{
    /// <summary>
    /// 如果启用swagger，并在class上添加此特性，那么该class下的所有方法都会生成api文档
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
    public class ApiClassGroupAttribute: RouteAttribute
    {
        public string GroupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="template">定义controller模板</param>
        public ApiClassGroupAttribute(E_ApiGroup groupName,string template= "apixx/[controller]")
            :base(template)
        {
            this.GroupName = groupName.ToString();
        }
    }

    /// <summary>
    /// 如果启用swagger，并在method上添加此特性，那么该method方法会生成api文档
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ApiMethodGroupAttribute : HttpMethodAttribute
    {
        public string GroupName { get; set; }
        /// <summary>
        /// 如果用swagger，必填httpVerbs，不然会报错
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="template">如果controller定义了模板，那么action不需要定义完整的模板，只要定义action名，调用的时候会自动把controller和action的模板进行组合</param>
        /// <param name="httpVerbs"></param>
        public ApiMethodGroupAttribute(E_ApiGroup groupName, string template = "[action]", params E_HttpVerbs[] httpVerbs)
            : base(httpVerbs.Select(s => s.ToString()), template)
        {
            GroupName = groupName.ToString();
        }
    }
}
