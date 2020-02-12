using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_02_15.EF;
using Ispit_2017_02_15.Helpers;
using Ispit_2017_02_15.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ispit_2017_02_15.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly MojContext _dbContext;

        public AuthenticationController(MojContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Login()
        {
            return View(new LoginInputVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == model.Username
                                                                       && x.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Password), "Username or password is incorrect");
                return View(model);
            }

            await HttpContext.SetLoggedInUser(user, model.RememberMe);

            return RedirectToAction("Index", "OdrzaniCas");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.LogoutUser();
            return RedirectToAction(nameof(Login));
        }
    }
}
