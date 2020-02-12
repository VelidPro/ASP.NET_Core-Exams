using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_02_15.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ispit_2017_02_15.Helpers
{
    public class AuthorizeNastavnik: TypeFilterAttribute
    {
        public AuthorizeNastavnik() : base(typeof(CustomAuthorization))
        {
        }
    }

    public class CustomAuthorization : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = await context.HttpContext.GetLoggedInUser();

            if (currentUser == null)
            {
                if (context.Controller is Controller c1)
                {
                    c1.TempData["error_messsage"] = "You are not logged in.";
                }

                context.Result = new RedirectToActionResult("Login","Authentication",new{@area=""});
                return;
            }

            var _dbContext = context.HttpContext.RequestServices.GetService<MojContext>();

            if (await _dbContext.Nastavnik.AnyAsync(x => x.UserId == currentUser.Id))
            {
                await next();
                return;
            }

            if (context.Controller is Controller c2)
            {
                c2.ViewData["error_message"] = "You don't have permissions";
            }

            context.Result = new RedirectToActionResult("Login", "Authentication", new { @area = "" });
        }
    }
}
