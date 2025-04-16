using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Middlewares
{
    public class SessionCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("UserSessionId"))
            {
                context.Items["UserId"] = context.Request.Cookies["UserSessionId"];
                context.Items["UserRole"] = context.Request.Cookies["UserSessionRole"];
                context.Items["IsUserLoggedIn"] = true;
            }
            else
            {
                context.Items["IsUserLoggedIn"] = false;
            }

            await _next(context);
        }
    }
}
