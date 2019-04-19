using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebCore.Component.Middlewares
{
    public class MiddlewareRepository
    {
        private RequestDelegate next;

        public MiddlewareRepository(RequestDelegate _next)
        {
            this.next = _next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Items.Add("middlewareID",
                         "ID of your middleware");
            await this.next(context);
        }
    }
}
