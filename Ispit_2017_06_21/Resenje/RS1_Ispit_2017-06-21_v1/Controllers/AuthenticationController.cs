using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_2017_06_21_v1.EF;
using RS1_Ispit_2017_06_21_v1.Helpers;
using RS1_Ispit_2017_06_21_v1.ViewModels;

namespace RS1_Ispit_2017_06_21_v1.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly MojContext _dbContext;

        public AuthenticationController(MojContext context)
        {
            _dbContext= context;
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
                ModelState.AddModelError(nameof(model.Password),"Username or password is incorrect");
                return View(model);
            }

            await HttpContext.SetLoggedInUser(user, model.RememberMe);

            return RedirectToAction("Index", "MaturskiIspit");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.LogoutUser();
            return RedirectToAction(nameof(Login));
        }
    }
}