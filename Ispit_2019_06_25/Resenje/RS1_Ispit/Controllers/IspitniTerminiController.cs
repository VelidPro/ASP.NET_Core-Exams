using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.Constants;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS1_Ispit_asp.net_core.EntityModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class IspitniTerminiController : Controller
    {
        private readonly MojContext _context;
        private readonly IPredmetService _predmetService;
        private readonly IDataProtector _protector;
        public IspitniTerminiController(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants, IPredmetService predmetService)
        {
            _context = context;
            _predmetService = predmetService;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }

        public async Task<IActionResult> GetAll(string angazmanId)
        {
            int decryptedAngazmanId = int.Parse(_protector.Unprotect(angazmanId));

            var model = await BuildIspitniTerminiViewModel(decryptedAngazmanId);
            return View("PredmetIspitniTermini",model);
        }

        [HttpGet]
        public async Task<IActionResult> Novi(string angazmanId)
        {
            int decryptedAngazmanId = int.Parse(_protector.Unprotect(angazmanId));

            var model = await BuildNoviIspitniTerminViewModel(decryptedAngazmanId);

            if (model == null)
                return Redirect("/");

            return PartialView("_Novi", model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novi(NoviIspitniTerminVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            int decryptedAngazmanId = int.Parse(_protector.Unprotect(model.AngazmanId));

            var angazman = _context.Angazovan.Find(decryptedAngazmanId);

            if (angazman == null)
                return BadRequest("Ne mozete dodati ispitne termine za ovaj predmet.");


            var noviTermin = new IspitniTermin
            {
                AngazovanId = decryptedAngazmanId,
                BrojPrijavljenihStudenata = 0,
                BrojNepolozenih = 0,
                DatumIspita = model.Datum,
                EvidentiraniRezultati = false
            };

            await _context.AddAsync(noviTermin);
            await _context.SaveChangesAsync();

            return Ok("Uspjesno dodat ispitni termin");

        }


        [HttpGet]
        public async Task<IActionResult> Detalji(string Id)
        {
            var ispitniTerminId = int.Parse(_protector.Unprotect(Id));

            var ispitniTermin = _context.IspitniTermini.Find(ispitniTerminId);

            if (ispitniTermin == null)
            {
                TempData["error"] = "Ispitni termin nije pronadjen.";
                return Redirect("/");

            }

            var model = await BuildIspitniTerminDetaljiViewModel(ispitniTerminId);


            return View(model);
        }


        private async Task<IspitniTerminiVM> BuildIspitniTerminiViewModel(int angazmanId)
        {
            var angazman = await _context.Angazovan
                .Include(x => x.Predmet)
                .Include(x => x.AkademskaGodina)
                .Include(x => x.Nastavnik)
                .FirstOrDefaultAsync(x => x.Id == angazmanId);

            if (angazman == null)
                return null;

            var ispitniTermini = _context.IspitniTermini.Where(x => x.AngazovanId == angazmanId);

            var model = new IspitniTerminiVM
            {
                AngazmanId = _protector.Protect(angazman.Id.ToString()),
                AkademskaGodina = angazman.AkademskaGodina.Opis,
                Predmet = angazman.Predmet.Naziv,
                Nastavnik = angazman.Nastavnik.Ime + " " + angazman.Nastavnik.Prezime,
                IspitniTermini = new List<IspitniTerminVM>()
            };

            if (!await ispitniTermini.AnyAsync())
                return model;

            model.IspitniTermini = await ispitniTermini
                   .Select(t => new IspitniTerminVM
                   {
                       Id = _protector.Protect(t.Id.ToString()),
                       DatumIspita = t.DatumIspita,
                       BrojStudenataNepolozeno = t.BrojNepolozenih,
                       BrojPrijavljenihStudenata = t.BrojPrijavljenihStudenata,
                       EvidentiraniRazultati = t.EvidentiraniRezultati
                   }).ToListAsync();

            return model;
        }

        private async Task<NoviIspitniTerminVM> BuildNoviIspitniTerminViewModel(int angazmanId)
        {
            var angazman = await _context.Angazovan
                .Include(x => x.Predmet)
                .Include(x => x.AkademskaGodina)
                .Include(x => x.Nastavnik)
                .FirstOrDefaultAsync(x => x.Id == angazmanId);

            if (angazman == null)
                return null;

            return new NoviIspitniTerminVM{
                AngazmanId = _protector.Protect(angazman.Id.ToString()),
                Datum = DateTime.Now.Date.AddDays(1),
                Napomena = string.Empty,
                Nastavnik = angazman.Nastavnik.Ime+" "+angazman.Nastavnik.Prezime,
                Predmet = angazman.Predmet.Naziv,
                SkolskaGodina = angazman.AkademskaGodina.Opis
            };
        }

        private async Task<IspitniTerminDetaljiVM> BuildIspitniTerminDetaljiViewModel(int ispitniTerminId)
        {
            var ispitniTermin = await _context.IspitniTermini
                .Include(x => x.Angazovan)
                .FirstOrDefaultAsync(x => x.Id == ispitniTerminId);

            var angazman = await _context.Angazovan
                .Include(x => x.Predmet)
                .Include(x => x.AkademskaGodina)
                .Include(x => x.Nastavnik)
                .FirstOrDefaultAsync( x => x.Id == ispitniTermin.AngazovanId);


            if (ispitniTermin == null || angazman==null)
                return null;

                await _context.Entry(ispitniTermin)
                .Collection(s => s.Polaganja).LoadAsync();


            var model = new IspitniTerminDetaljiVM
            {
                Id=_protector.Protect(ispitniTermin.Id.ToString()),
                Predmet = angazman.Predmet.Naziv,
                Nastavnik = angazman.Nastavnik.Ime+" "+angazman.Nastavnik.Prezime,
                SkolskaGodina = angazman.AkademskaGodina.Opis,
                Napomena = ispitniTermin.Napomena,
                Zakljucan = ispitniTermin.EvidentiraniRezultati,
                  Datum = ispitniTermin.DatumIspita,
                Polaganja = ispitniTermin.Polaganja.Any() ? ispitniTermin.Polaganja.Select(x => new PolaganjeIspitaVM{
                    Id = _protector.Protect(x.Id.ToString()),
                    Student = _context.UpisGodine
                         .Where(u => u.Id == x.UpisGodineId)
                        .Select(u => u .Student)
                        .FirstOrDefault()
                        ?.GetImePrezime() ?? "",
                    PristupioIspitu = x.PristupioIspitu,
                    Ocjena = x.Ocjena

                }).ToList() : new List<PolaganjeIspitaVM>()
            };

            return model;
        }
       
    }
}