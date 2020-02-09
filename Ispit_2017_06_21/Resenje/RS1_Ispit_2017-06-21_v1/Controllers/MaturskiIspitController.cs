using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_2017_06_21_v1.EF;
using RS1_Ispit_2017_06_21_v1.Helpers;

namespace RS1_Ispit_2017_06_21_v1.Controllers
{
    [AuthorizationNastavnik]
    public class MaturskiIspitController : Controller
    {
        private readonly MojContext _context;

        public MaturskiIspitController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
    }
}