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
using System.IO;
using WebCore.Common.Attributes;

namespace WebCore.Controllers
{
    public class HomeController : BaseController
    {
        private IStudentRepository db { get; set; }
        public HomeController(IStudentRepository _db)
        {
            db = _db;
            //log.Info("linux test");
        }

        public virtual IActionResult Index()
        {
            //throw new Exception("333");
            var b = new Entity.DBTest.Student();
            b.Name = "ppp";
            var a=db.Test2(7,b);
            db.Test();
            var entity=db.Get(p => p.Name == "李四");
            return View(entity);
        }

        public string SetCookies() {
            CookieOptions c = new CookieOptions();
            c.Expires = DateTimeOffset.Now.AddDays(1);
            Response.Cookies.Append("name","99999", c);
            return "0000";
        }

        public string GetCookies()
        {
            return Request.Cookies["name"];
        }


    }
}
