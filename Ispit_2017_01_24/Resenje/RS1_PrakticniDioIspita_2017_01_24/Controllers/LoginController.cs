using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Helpers;
using RS1_PrakticniDioIspita_2017_01_24.ViewModels;

namespace RS1_PrakticniDioIspita_2017_01_24.Controllers
{
    public class LoginController : Controller
    {
        private readonly MojContext _context;

        public LoginController(MojContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.Username == model.Username && x.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("Password","Pogresan username ili password.");
                return View(model);
            }

            await HttpContext.SetLoggedInUser(user, model.RememberMe);

            return RedirectToAction("All", "OdrzaniCas");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.RemoveAuthTokensCurrentUser(Response);

            return RedirectToAction("Index","Login");
        }
    }
}