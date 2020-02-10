using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_2017_06_21_v1.EF;
using RS1_Ispit_2017_06_21_v1.Helpers;
using RS1_Ispit_2017_06_21_v1.Interfaces;
using RS1_Ispit_2017_06_21_v1.Models;
using RS1_Ispit_2017_06_21_v1.ViewModels;

namespace RS1_Ispit_2017_06_21_v1.Controllers
{
    [AuthorizationNastavnik]
    public class MaturskiIspitController : Controller
    {
        private readonly MojContext _dbContext;
        private readonly IMaturskiIspitService _maturskiIspitService;

        public MaturskiIspitController(MojContext context, IMaturskiIspitService maturskiIspitService)
        {
            _dbContext = context;
            _maturskiIspitService = maturskiIspitService;
        }

        public async Task<IActionResult> Index()
        {
            var vModel = await BuildListaMaturskihIspitaVM();

            return View(vModel);
        }

        public async Task<IActionResult> Dodaj()
        {
            var vModel = await BuildMaturskiIspitInputVM();

            return PartialView("_Novi",vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(MaturskiIspitInputVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new {Errors = ModelState.Values.SelectMany(x => x.Errors)});

            var user = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.Id == user.Id);


            if (user == null || nastavnik == null)
                return RedirectToAction(nameof(Index));

            var noviIspit = new MaturskiIspit
            {
                Datum = model.Datum,
                NastavnikId = nastavnik.Id,
                OdjeljenjeId = model.OdjeljenjeId
            };
            var dodavanjeResult = await _maturskiIspitService.Dodaj(noviIspit);

            if (dodavanjeResult.Success)
                return RedirectToAction(nameof(Index));

            return BadRequest(new{Error=dodavanjeResult.Message});
        }


        private async Task<ListaMaturskihIspitaVM> BuildListaMaturskihIspitaVM()
        {
            var maturskiIspiti = _dbContext.MaturskiIspiti
                .Include(x => x.Nastavnik)
                .Include(x => x.Odjeljenje);

            var maturskiIspitiVM = new List<MaturskiIspitVM>();

            if (await maturskiIspiti.AnyAsync())
            {
                foreach (var x in maturskiIspiti)
                {
                    maturskiIspitiVM.Add(new MaturskiIspitVM
                    {
                        Datum = x.Datum,
                        Id = x.Id,
                        Ispitivac = x.Nastavnik.ImePrezime,
                        Odjeljenje = x.Odjeljenje.Naziv,
                        ProsjecniBodovi = _maturskiIspitService.GetProsjekBodova(x.Id)

                    });
                }
                
            }

            return new ListaMaturskihIspitaVM
            {
                MaturskiIspiti = maturskiIspitiVM
            };
        }

        private async Task<MaturskiIspitInputVM> BuildMaturskiIspitInputVM()
        {
            var user = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x=>x.Id==user.Id);

            if (user == null || nastavnik==null)
                return null;

            return new MaturskiIspitInputVM
            {
                Datum=DateTime.Now.AddDays(1),
                Ispitivac = nastavnik.ImePrezime,
                Odjeljenja = _dbContext.Odjeljenje.Where(x=>x.NastavnikId==nastavnik.Id)
                    .ToSelectList(x=>x.Id.ToString(),x=>x.Naziv,"Odaberite odjeljenje")
            };
        }

    }
}