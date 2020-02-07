using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Helpers;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class MaturskiIspitController : Controller
    {
        private const string NOT_FOUND = "N/A";
        private readonly MojContext _context;
        private readonly IMaturskiIspitService _maturskiIspitService;
        public MaturskiIspitController(MojContext context, IMaturskiIspitService maturskiIspitService)
        {
            _context = context;
            _maturskiIspitService = maturskiIspitService;
        }


        public async Task<IActionResult> Index()
        {
            var nastavniciViewModel = await BuildNastavniciViewModel();

            return View(nastavniciViewModel);
        }

        public async Task<IActionResult> GetTakmicenja(int nastavnikId)
        {

            if (!await _context.Nastavnik.AnyAsync(x=>x.Id==nastavnikId))
                return NotFound("Nastavnik nije pronadjen.");

            var vmModel = await BuildMaturskiIspitiVM(nastavnikId);

            return View("All",vmModel);
        }


        public async Task<IActionResult> Dodaj(int nastavnikId)
        {
            var vModel = await BuildMaturskiIspitInputVM(nastavnikId);

            return PartialView("_Dodaj",vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(MaturskiIspitInputVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new{ModelState=ModelState.Values.SelectMany(x=>x.Errors)});

            var noviMaturskiIspit = new MaturskiIspit
            {
                DatumOdrzavanja = model.DatumIspita,
                Napomena = string.Empty,
                NastavnikId = model.NastavnikId,
                PredmetId = model.PredmetId,
                SkolaId = model.SkolaId,
                SkolskaGodinaId = model.SkolskaGodinaId
            };

            var dodavanjeMaturskogIspitaResult = await _maturskiIspitService.DodajNovi(noviMaturskiIspit);

            if (dodavanjeMaturskogIspitaResult.Success)
                return Ok(dodavanjeMaturskogIspitaResult.Message);


            return BadRequest(dodavanjeMaturskogIspitaResult.Message);
        }


        public async Task<IActionResult> Detalji(int Id)
        {
            if (!await _context.MaturskiIspiti.AnyAsync(x=>x.Id==Id))
                return NotFound("Maturski ispit nije pronadjen.");

            var vModel = await BuildDetaljiMaturskiIspitVM(Id);

            return View(vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Uredi([FromForm]int maturskiIspitId,[FromForm]string napomena)
        {
            var maturskiIspit = await _context.MaturskiIspiti.FindAsync(maturskiIspitId);

            if (maturskiIspit == null)
                return NotFound("Maturski ispit nije pronadjen.");

            if (string.IsNullOrEmpty(napomena) || napomena.Length > 150)
                return BadRequest("Napomena moze sadrzati izmedju 1 i 100 karaktera");


            maturskiIspit.Napomena = napomena;

            _context.Update(maturskiIspit);
            await _context.SaveChangesAsync();

            return Ok("Uspjesno evidentirana napomena.");
        }


        public async Task<IActionResult> PrisustvoToggler(int maturskiIspitStavkaId, int rowNumber)
        {
            var maturskiIspitStavka = await _context.MaturskiIspitStavke.FindAsync(maturskiIspitStavkaId);

            if (maturskiIspitStavka == null)
                return NotFound("Polaganje nije pronadjeno.");

            maturskiIspitStavka.IsPristupio = !maturskiIspitStavka.IsPristupio;
            _context.Update(maturskiIspitStavka);
            await _context.SaveChangesAsync();

            return ViewComponent("SinglePolaganje", new {maturskiIspitStavkaId = maturskiIspitStavkaId,rowNumber=rowNumber});
        }


        public async Task<IActionResult> UrediPolaganje(int maturskiIspitStavkaId)
        {
            var maturskiIspitStavka = await _context.MaturskiIspitStavke
                .Include(x=>x.Ucenik)
                .FirstOrDefaultAsync(x=>x.Id==maturskiIspitStavkaId);

            if (maturskiIspitStavka == null)
                return NotFound("Polaganje nije pronadjeno.");

            var vModel = await BuildMaturskiIspitStavkaInputVM(maturskiIspitStavka);

            return PartialView("_PolaganjeEdit", vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediPolaganje(MaturskiIspitStavkaInputVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new {ModelStateErrors = ModelState.Values.SelectMany(x => x.Errors)});

            var maturskiIspitStavka = await _context.MaturskiIspitStavke.FindAsync(model.Id);

            maturskiIspitStavka.OsvojeniBodovi = model.Bodovi;

            _context.Update(maturskiIspitStavka);
            await _context.SaveChangesAsync();

            return ViewComponent("SinglePolaganje", new {maturskiIspitStavkaId = model.Id});


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EvidencijaBodova([FromForm] int brojBodova,
            [FromForm] int maturskiIspitStavkaId)
        {
            if (brojBodova < 0 || brojBodova > 100)
                return BadRequest("Broj bodova mora biti izmedju 0 i 100");

            var polaganje = await _context.MaturskiIspitStavke.FindAsync(maturskiIspitStavkaId);

            if (polaganje == null)
                return NotFound("Maturski ispit nije pronadjen.");

            polaganje.OsvojeniBodovi = brojBodova;
            _context.Update(polaganje);

            await _context.SaveChangesAsync();

            return Ok("Uspjesno evidentirani bodovi.");
        }


        private async Task<NastavniciVM> BuildNastavniciViewModel()
        {
            var nastavnici = _context.PredajePredmet
                .Include(x => x.Nastavnik)
                .Include(x => x.Odjeljenje)
                .ThenInclude(x => x.Skola);

            var nastavniciVM = new List<NastavnikVM>();

            if (await nastavnici.AnyAsync())
                nastavniciVM =   nastavnici.Select(x => new NastavnikVM
                {
                    Id=x.NastavnikID,
                    ImePrezime = x.Nastavnik.ImePrezime(),
                    Skola=x.Odjeljenje.Skola.Naziv
                })
                    .AsEnumerable()
                    .DistinctBy(x=>x.Id)
                    .ToList();

            return new NastavniciVM{Nastavnici = nastavniciVM};
        }

        private async Task<MaturskiIspitiVM> BuildMaturskiIspitiVM(int nastavnikId)
        {
            var maturskiIspiti = _context.MaturskiIspiti
                .Include(x=>x.Skola)
                .Include(x=>x.Predmet)
                .Where(x => x.NastavnikId == nastavnikId);

            var nastavnik = await _context.Nastavnik.FindAsync(nastavnikId);


            var maturskiIspitiVM = new List<MaturskiIspitVM>();

            if (await maturskiIspiti.AnyAsync())
            {
                foreach (var x in maturskiIspiti)
                {
                    maturskiIspitiVM.Add(new MaturskiIspitVM
                    {
                        Id = x.Id,
                        Datum = x.DatumOdrzavanja,
                        Predmet = x.Predmet.Naziv,
                        Skola = x.Skola.Naziv,
                        UceniciNisuPristupili = x.DatumOdrzavanja.Date<DateTime.Now.Date?
                            (await _maturskiIspitService.UceniciNisuPristupili(x.Id)).Select(z=>z.ImePrezime).ToList()
                            :new List<string>()
                    });
                }
            }

            return new MaturskiIspitiVM
            {
                NastavnikId = nastavnikId,
                Nastavnik = nastavnik?.ImePrezime()??NOT_FOUND,
                MaturskiIspiti = maturskiIspitiVM
            };

        }

        private async Task<MaturskiIspitInputVM> BuildMaturskiIspitInputVM(int nastavnikId)
        {
            var nastavnik = await _context.Nastavnik.FindAsync(nastavnikId);

            var skolskaGodina = await _context.SkolskaGodina.FirstOrDefaultAsync(x=>x.Aktuelna);

            return new MaturskiIspitInputVM
            {
                SkolskaGodinaId = skolskaGodina.Id,
                SkolskaGodina = skolskaGodina.Naziv,
                DatumIspita = DateTime.Now.AddDays(1).Date,
                Nastavnik = nastavnik?.ImePrezime()??NOT_FOUND,
                NastavnikId = nastavnikId,
                Predmeti = _context.PredajePredmet.Where(x=>x.NastavnikID==nastavnikId)
                    .Select(x=>x.Predmet)
                    .AsEnumerable()
                    .ToSelectList(x=>x.Id.ToString(),x=>x.Naziv,"Odaberite predmet"),
                Skole=_context.Skola
                    .AsEnumerable()
                    .ToSelectList(x=>x.Id.ToString(),x=>x.Naziv,"Odaberite skolu")
            };
        }

        private async Task<MaturskiIspitDetaljiVM> BuildDetaljiMaturskiIspitVM(int maturskiIspitId)
        {
            var polaganja = _context.MaturskiIspitStavke
                .Include(x=>x.Ucenik)
                .Where(x => x.MaturskiIspitId == maturskiIspitId);

            var maturskiIspit = await _context.MaturskiIspiti
                .Include(x=>x.Predmet)
                .FirstOrDefaultAsync(x=>x.Id==maturskiIspitId);

            var polaganjaVM = new List<MaturskiIspitStavkaVM>();

            if (await polaganja.AnyAsync())
            {
                foreach (var x in polaganja)
                {
                    polaganjaVM.Add(await BuildMaturskiIspitStavkaVM(x));
                }
            }

            return new MaturskiIspitDetaljiVM
            {
                Datum = maturskiIspit.DatumOdrzavanja,
                Id=maturskiIspitId,
                Napomena = maturskiIspit.Napomena,
                Predmet = maturskiIspit.Predmet.Naziv,
                PrijavljeniUcenici = polaganjaVM
            };
        }


        private async Task<MaturskiIspitStavkaVM> BuildMaturskiIspitStavkaVM(MaturskiIspitStavka stavka)
        {
            return new MaturskiIspitStavkaVM
            {
                Id = stavka.Id,
                IsPristupio = stavka.IsPristupio,
                ProsjekOcjena = await _maturskiIspitService.GetProsjekUcenika(stavka.UcenikId),
                OsvojioBodova = stavka.OsvojeniBodovi,
                Ucenik = stavka.Ucenik.ImePrezime
            };
        }

        private async Task<MaturskiIspitStavkaInputVM> BuildMaturskiIspitStavkaInputVM(MaturskiIspitStavka stavka)
        {
            if(stavka==null)
                return new MaturskiIspitStavkaInputVM();

            return new MaturskiIspitStavkaInputVM
            {
                Id=stavka.Id,
                Bodovi = 0,
                Ucenik=stavka.Ucenik?.ImePrezime??NOT_FOUND
            };
        }
    }
}