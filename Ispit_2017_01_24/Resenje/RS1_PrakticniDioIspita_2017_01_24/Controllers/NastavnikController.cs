using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Helpers;

namespace RS1_PrakticniDioIspita_2017_01_24.Controllers
{
    [AuthorizationNastavnik]
    public class NastavnikController : Controller
    {
        private readonly MojContext _context;

        public NastavnikController(MojContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {

            return View();
        }


    }
}