using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCore.Common.Logging;
using WebCore.Common.Extensions;
using Microsoft.Extensions.Options;
using WebCore.Component.Options;

namespace WebCore.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 日志帮助类
        /// </summary>
        public ILogHelper log= new Log4NetHelper();
    }
}