using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RS1_Ispit_2017_06_21_v1.EF;

namespace RS1_Ispit_2017_06_21_v1.Helpers
{
    public class AuthorizationNastavnik: TypeFilterAttribute
    {
        public AuthorizationNastavnik() : base(typeof(NastavnikCustomAuthorization))
        {
        }
    }

    public class NastavnikCustomAuthorization: IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = await context.HttpContext.GetLoggedInUser();

            if (currentUser == null)
            {
                if (context.Controller is Controller controller)
                {
                    controller.TempData["error_message"] = "You are not logged in.";
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

            if (context.Controller is Controller c)
            {
                c.ViewData["error_message"] = "You don't have permissions";
            }

            context.Result=new RedirectToActionResult("Login","Authentication",new{@area=""});
        }
    }
}
