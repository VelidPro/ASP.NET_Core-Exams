using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Helpers;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class TakmicenjeController : Controller
    {
        private readonly MojContext _context;
        private readonly ITakmicenjeService _takmicenjeService;

        public TakmicenjeController(MojContext context, ITakmicenjeService takmicenjeService)
        {
            _context = context;
            _takmicenjeService = takmicenjeService;
        }


        public async Task<IActionResult> Index()
        {
            var model = await BuildPretragaVM();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pretraga(PretragaTakmicenjeVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Parametri pretrage nisu validni.");
            var skola = await _context.Skola.FindAsync(model.SkolaDomacinId);

            if (skola == null)
                return BadRequest("Skola nije pronadjena.");


            var modelVM = new TakmicenjaVM
            {
                SkolaDomacin=skola.Naziv,
                SkolaDomacinId = model.SkolaDomacinId,
                Razred= model.Razred
            };

            return View("RezultatPretrage", modelVM);
        }

        public async Task<IActionResult> Dodaj(int skolaId)
        {
            var model = await BuilDodavanjeTakmicenjaVM(skolaId);

            return PartialView("_Novo", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(DodavanjeTakmicenjaVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            var predmet = await _context.Predmet.FirstOrDefaultAsync(x => x.Naziv == model.Predmet && x.Razred == model.Razred);
            var novoTakmicenje = new Takmicenje
            {
                SkolaDomacinId = model.SkolaDomacinId,
                BrojKojiNisuPristupili = 0,
                BrojPrijavljenih = 0,
                DatumOdrzavanja = model.DatumOdrzavanja,
                PredmetId=predmet.Id,
                Razred = model.Razred,
                IsEvidentiraniRezultati = false
            };
            var rezultatDodavanja = await _takmicenjeService.DodajTakmicenje(novoTakmicenje);

            if (rezultatDodavanja.Success)
                return ViewComponent("Takmicenje",new {skolaId=novoTakmicenje.SkolaDomacinId,razred=0});

            return BadRequest("Greska");

        }


        public async Task<IActionResult> Rezultati(int takmicenjeId)
        {
            var takmicenje = await _context.Takmicenja
                .Include(x => x.SkolaDomacin)
                .Include(x => x.Predmet)
                .FirstOrDefaultAsync(x => x.Id == takmicenjeId);

            if (takmicenje == null)
                return BadRequest("Takmicenje nije pronadjeno.");

            var model = new RezultatiTakmicenjaVM
            {
                DatumOdrzavanja = takmicenje.DatumOdrzavanja,
                Id=takmicenje.Id,
                IsEvidentiraniRezultati = takmicenje.IsEvidentiraniRezultati,
                Predmet = takmicenje.Predmet.Naziv,
                Razred = takmicenje.Razred,
                SkolaDomacinId = takmicenje.SkolaDomacinId,
                SkolaDomacin = takmicenje.SkolaDomacin.Naziv
            };

            return View(model);
        }



        public async Task<IActionResult> DodajUcesnika(int takmicenjeID)
        {
            var model = await BuildDodavanjeUcesnikaVM(takmicenjeID);

            return View("_DodavanjeUcesnika", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajUcesnika(DodavanjeUcesnikaTakmicenjaVM model)
        {
            if (!ModelState.IsValid || model.OsvojeniBodovi<0 || model.OsvojeniBodovi>100)
                return BadRequest("Podaci nisu validni.");

            var noviUcesnik = new TakmicenjeUcesnik{
                IsPristupio = false,
                OdjeljenjeStavkaId = model.OdjeljenjeStavkaId,
                OsvojeniBodovi = model.OsvojeniBodovi,
                TakmicenjeId = model.TakmicenjeId
            };

            var rezultatiDodavanja = await _takmicenjeService.DodajUcesnika(noviUcesnik);

            if (rezultatiDodavanja.Success)
                return ViewComponent("RezultatiTakmicenja", new {takmicenjeId = model.TakmicenjeId});

            return BadRequest(rezultatiDodavanja.Message);
        }



        public async Task<IActionResult> UcesnikJePristupio(int takmicenjeUcesnikId)
        {
            var takmicenjeUcesnik = await _context.TakmicenjeUcesnici.FindAsync(takmicenjeUcesnikId);

            if (takmicenjeUcesnik == null)
                return BadRequest("Ucesnik nije pronadjen.");

            takmicenjeUcesnik.IsPristupio = true;

            _context.Update(takmicenjeUcesnik);

            await _context.SaveChangesAsync();

            return ViewComponent("SingleRezultatTakmicenja",new{takmicenjeUcesnikId=takmicenjeUcesnikId});
        }

        public async Task<IActionResult> UcesnikNijePristupio(int takmicenjeUcesnikId)
        {
            var takmicenjeUcesnik = await _context.TakmicenjeUcesnici.FindAsync(takmicenjeUcesnikId);

            if (takmicenjeUcesnik == null)
                return BadRequest("Ucesnik nije pronadjen.");

            takmicenjeUcesnik.IsPristupio = false;

            _context.Update(takmicenjeUcesnik);

            await _context.SaveChangesAsync();

            return ViewComponent("SingleRezultatTakmicenja", new { takmicenjeUcesnikId = takmicenjeUcesnikId });
        }


        [HttpGet]
        public async Task<IActionResult> RezultatEditGet(int takmicenjeUcesnikId)
        {
            var takmicenjeUcesnik = await _context.TakmicenjeUcesnici
                .Include(x => x.OdjeljenjeStavka)
                .ThenInclude(x=>x.Odjeljenje)
                .Include(x=>x.OdjeljenjeStavka)
                .ThenInclude(x=>x.Ucenik)
                .FirstOrDefaultAsync(x=>x.Id==takmicenjeUcesnikId);

            if (takmicenjeUcesnik == null)
                return BadRequest("Ucesnik nije pronadjen.");

            var model = new RezultatEditVM
            {
                TakmicenjeUcesnikId = takmicenjeUcesnik.Id,
                Ucesnik = string.Concat(takmicenjeUcesnik.OdjeljenjeStavka.Odjeljenje.Oznaka," - ",takmicenjeUcesnik.OdjeljenjeStavka.Ucenik.ImePrezime),
                OsvojeniBodovi = takmicenjeUcesnik.OsvojeniBodovi
            };

            return PartialView("_RezultatEdit", model);

        }


        [HttpPost]
        public async Task<IActionResult> RezultatEdit(RezultatEditVM model)
        {
            if (!ModelState.IsValid || model.OsvojeniBodovi < 0 || model.OsvojeniBodovi > 100)
                return BadRequest("Podaci nisu validni.");

            var ucesnikTakmicenje = await _context.TakmicenjeUcesnici.FindAsync(model.TakmicenjeUcesnikId);

            if (ucesnikTakmicenje == null)
                return BadRequest("Ucesnik nije pronadjen.");

            ucesnikTakmicenje.OsvojeniBodovi = model.OsvojeniBodovi;
            ucesnikTakmicenje.IsPristupio = true;

            _context.Update(ucesnikTakmicenje);
            await _context.SaveChangesAsync();

            return ViewComponent("RezultatiTakmicenja", new {takmicenjeId = ucesnikTakmicenje.TakmicenjeId});
        }


        public async Task<IActionResult> Zakljucaj(int takmicenjeId)
        {
            var takmicenje = await _context.Takmicenja.FindAsync(takmicenjeId);

            if (takmicenje == null)
                return BadRequest("Takmicenje nije pronadjeno.");

            takmicenje.IsEvidentiraniRezultati = true;

            _context.Update(takmicenje);

            await _context.SaveChangesAsync();

            return ViewComponent("RezultatiTakmicenja", new { takmicenjeId = takmicenjeId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EvidencijaRezultata([FromForm]int takmicenjeUcesnikId, [FromForm]int bodovi)
        {
            var takmicenjeUcesik = await _context.TakmicenjeUcesnici.FindAsync(takmicenjeUcesnikId);

            if (takmicenjeUcesik == null)
                return BadRequest("Ucesnik nije pronadjen.");
            if (bodovi < 0 || bodovi > 100)
                return BadRequest("Broj bodova mora biti izmedju 0 i 100.");

            takmicenjeUcesik.OsvojeniBodovi = bodovi;
            _context.Update(takmicenjeUcesik);

            await _context.SaveChangesAsync();

            return Ok("Uspjesno evidentiran rezultat.");
        }




        private async Task<DodavanjeUcesnikaTakmicenjaVM> BuildDodavanjeUcesnikaVM(int takmicenjeId)
        {
            var takmicenje = await _context.Takmicenja.FindAsync(takmicenjeId);

            if (takmicenje == null)
                return null;

            var vecDodati = _context.TakmicenjeUcesnici
                .Where(x => x.TakmicenjeId == takmicenjeId)
                .Select(x => x.OdjeljenjeStavkaId);

            var ponudjeniUcesnici = _context.DodjeljenPredmet
                .Where(x => x.PredmetId == takmicenje.PredmetId && !vecDodati.Contains(x.OdjeljenjeStavkaId))
                .Select(x => x.OdjeljenjeStavka)
                .Include(x => x.Ucenik)
                .Include(x => x.Odjeljenje)
                .ToSelectList(x => x.Id.ToString(), x => x.Odjeljenje.Oznaka + " - " + x.Ucenik.ImePrezime,
                    defaultOption: "Odaberite ucensnika");

           return new DodavanjeUcesnikaTakmicenjaVM{
               TakmicenjeId = takmicenjeId,
               PonudjeniUcesnici = ponudjeniUcesnici
           };
        }

        private async Task<PretragaTakmicenjeVM> BuildPretragaVM()
        {
      
                var skole = _context.Skola.AsEnumerable().ToSelectList(x => x.Id.ToString(), x => x.Naziv,
                    defaultOption: "Odaberite skolu"); ;


            var razredi = new List<int> {1, 2, 3, 4}.ToSelectList(x => x.ToString(), x => x.ToString(),
                defaultOption: "Odaberite razred");

            return new PretragaTakmicenjeVM{
            Skole=skole,
            Razredi = razredi
            };

        }




        private async Task<DodavanjeTakmicenjaVM> BuilDodavanjeTakmicenjaVM(int skolaId)
        {
            var skola = await _context.Skola.FindAsync(skolaId);

            if (skola == null)
                return null;

            var predmeti = _context.Predmet
                .AsEnumerable()
                .DistinctBy(x=>x.Naziv)
                .ToSelectList(x => x.Naziv, x => x.Naziv,
                defaultOption: "Odaberite predmet");

            var razredi = new List<int> {1, 2, 3, 4}.ToSelectList(x => x.ToString(), x => x.ToString(),
                defaultOption: "Odaberite razred");

            return new DodavanjeTakmicenjaVM
            {
                SkolaDomacinId = skola.Id,
                SkolaDomacin = skola.Naziv,
                DatumOdrzavanja = DateTime.Now.AddDays(1),
                Razredi=razredi,
                Predmeti=predmeti
            };

        }
    }
}