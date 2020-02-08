using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Models;

namespace RS1_PrakticniDioIspita_2017_01_24.Helpers
{
    public class AuthorizationNastavnik : TypeFilterAttribute
    {
        public AuthorizationNastavnik() : base(typeof(CustomAuthorizationImpl))
        {
        }
    }


    public class CustomAuthorizationImpl : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            User user = await context.HttpContext.GetLoggedInUser();

            if (user == null)
            {
                if (context.Controller is Controller controller)
                {
                    controller.TempData["error_message"] = "Niste logirani.";
                }
                context.Result = new RedirectToActionResult("Index", "Login", new { @area = "" });

                return;
            }

            var _dbContext = context.HttpContext.RequestServices.GetService<MojContext>();

            if (await _dbContext.Nastavnici.AnyAsync(x => x.UserId == user.Id))
            {
                await next();
                return;
            }


            if (context.Controller is Controller c)
            {
                c.ViewData["error_message"] = "Nemate pravo pristupa.";
            }

            context.Result=new RedirectToActionResult("Index","Login",new {@area=""});
        }
    }

}