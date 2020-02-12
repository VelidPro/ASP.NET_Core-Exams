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

        private const string NOT_FOUND = "N/A";

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


        public async Task<IActionResult> Detalji(int Id)
        {
            var maturskiIspit = await _dbContext.MaturskiIspiti
                .Include(x=>x.Nastavnik)
                .Include(x=>x.Odjeljenje)
                .FirstOrDefaultAsync(x=>x.Id==Id);

            if (maturskiIspit == null)
                return NotFound("Maturski ispit nije pronadjen.");

            var user = await HttpContext.GetLoggedInUser();
            var currentNastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.Id == user.Id);

            if(maturskiIspit.NastavnikId!= currentNastavnik.Id)
            {
                return Unauthorized();
            }

            var vModel = await BuildMaturskiIspitDetaljiVM(maturskiIspit);

            return View(vModel);

        }


        public async Task<IActionResult> Uredi(int Id)
        {
            var polaganje = await _dbContext.MaturskiIspitStavke
                .Include(x => x.UpisUOdjeljenje)
                .ThenInclude(x => x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (polaganje == null)
                return NotFound();

            if (polaganje.Oslobodjen)
                return BadRequest();

            var vModel = await BuildMaturskiIspitStavkaInputVM(polaganje);

            return PartialView("_RezultatPolaganja", vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Uredi(MaturskiIspitStavkaInputVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            var polaganje = await _dbContext.MaturskiIspitStavke
                .Include(x=>x.UpisUOdjeljenje)
                .ThenInclude(x=>x.Ucenik)
                .FirstOrDefaultAsync(x=>x.Id==model.Id);

            if (polaganje == null)
                return NotFound();

            if (polaganje.Oslobodjen)
                return BadRequest();

            polaganje.Bodovi = model.Bodovi;

            _dbContext.Update(polaganje);
            await _dbContext.SaveChangesAsync();

            return PartialView("_MaturskiIspitStavkaRow", await BuildMaturskiIspitStavkaVM(polaganje));
        }


        [Route("/Polaganje")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bodovi([FromForm]int Id, [FromForm]float bodovi)
        {
            if (bodovi < 0 || bodovi > 100)
                return BadRequest("Bodovi moraju biti izmedju 0 i 100");

            var polaganje = await _dbContext.MaturskiIspitStavke.FindAsync(Id);

            if (polaganje == null)
                return NotFound();

            polaganje.Bodovi = bodovi;

            _dbContext.Update(polaganje);
            await _dbContext.SaveChangesAsync();

            return Ok("Uspjesno evidentirani bodovi.");
        }


        public async Task<IActionResult> OslobodjenToggle(int maturskiIspitStavkaId)
        {
            var polaganje = await _dbContext.MaturskiIspitStavke
                .Include(x => x.UpisUOdjeljenje)
                .ThenInclude(x => x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == maturskiIspitStavkaId);

            if (polaganje == null)
                return NotFound();

            polaganje.Oslobodjen = !polaganje.Oslobodjen;
            _dbContext.Update(polaganje);
            await _dbContext.SaveChangesAsync();

            return PartialView("_MaturskiIspitStavkaRow",await BuildMaturskiIspitStavkaVM(polaganje));
        }


        private async Task<MaturskiIspitStavkaInputVM> BuildMaturskiIspitStavkaInputVM(MaturskiIspitStavka polaganje)
        {
            if (polaganje == null)
                return new MaturskiIspitStavkaInputVM();

            return new MaturskiIspitStavkaInputVM
            {
                Id=polaganje.Id,
                Ucenik=polaganje.UpisUOdjeljenje.Ucenik.ImePrezime
            };
        }

        private async Task<MaturskiIspitStavkaVM> BuildMaturskiIspitStavkaVM(MaturskiIspitStavka polaganje)
        {
            return new MaturskiIspitStavkaVM
            {
                Id = polaganje.Id,
                Bodovi = polaganje.Bodovi,
                OpstiUspjeh = polaganje.UpisUOdjeljenje?.OpciUspjeh??0,
                Oslobodjen = polaganje.Oslobodjen,
                Ucenik = polaganje.UpisUOdjeljenje?.Ucenik?.ImePrezime??NOT_FOUND
            };
        }
        private async Task<ListaMaturskihIspitaVM> BuildListaMaturskihIspitaVM()
        {
            var user = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.Id == user.Id);

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
                        IspitivacId = x.NastavnikId,
                        Odjeljenje = x.Odjeljenje.Naziv,
                        ProsjecniBodovi = _maturskiIspitService.GetProsjekBodova(x.Id)

                    });
                }
                
            }

            return new ListaMaturskihIspitaVM
            {
                IspitivacId = nastavnik.Id,
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

        private async Task<MaturskiIspitDetaljiVM> BuildMaturskiIspitDetaljiVM(MaturskiIspit ispit)
        {
            var polaganja = _dbContext.MaturskiIspitStavke
                .Include(x=>x.UpisUOdjeljenje)
                .ThenInclude(x=>x.Ucenik)
                .Where(x => x.MaturskiIspitId == ispit.Id);

            var polaganjaVM = new List<MaturskiIspitStavkaVM>();

            if(await polaganja.AnyAsync())
            {
                polaganjaVM = await polaganja.Select(x => new MaturskiIspitStavkaVM
                {
                    Id=x.Id,
                    Bodovi = x.Bodovi,
                    OpstiUspjeh = x.UpisUOdjeljenje.OpciUspjeh,
                    Oslobodjen = x.Oslobodjen,
                    Ucenik = x.UpisUOdjeljenje.Ucenik.ImePrezime
                }).ToListAsync();
            }


            return new MaturskiIspitDetaljiVM{
                Datum = ispit.Datum,
                Id=ispit.Id,
                Ispitivac = ispit.Nastavnik.ImePrezime,
                Odjeljenje = ispit.Odjeljenje.Naziv,
                Polaganja = polaganjaVM
            };
        }

    }
}