using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebCore.Controllers
{
    public class MessageController : BaseController
    {
        public MessageController()
        {
    
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var eh = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception ex = eh?.Error;
            log.ErrorAsync(ex.Message,ex);
            return View(ex);
        }
    }
}