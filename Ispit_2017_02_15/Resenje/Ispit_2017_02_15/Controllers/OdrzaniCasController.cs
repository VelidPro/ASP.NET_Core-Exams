using Ispit_2017_02_15.EF;
using Ispit_2017_02_15.Helpers;
using Ispit_2017_02_15.Models;
using Ispit_2017_02_15.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_02_15.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;

namespace Ispit_2017_02_15.Controllers
{
    [AuthorizeNastavnik]
    public class OdrzaniCasController : Controller
    {
        private const string NOT_FOUND = "N/A";
        private readonly MojContext _dbContext;
        private readonly IOdrzaniCasService _odrzaniCasService;

        public OdrzaniCasController(MojContext dbContext, IOdrzaniCasService odrzaniCasService)
        {
            _dbContext = dbContext;
            _odrzaniCasService = odrzaniCasService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            return View(await BuildOdrzaniCasoviListVM(nastavnik));
        }

        public async Task<IActionResult> Dodaj()
        {
            var currentUser = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            return View("OdrzaniCasForm",await BuildOdrzaniCasInputVM(nastavnik));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(OdrzaniCasInputVM model)
        {
            var currentUser = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.AkademskeGodinePredmeti = GetListPredmeta(nastavnik.Id);
                return View("OdrzaniCasForm", model);
            }

            var noviCas = new OdrzaniCas
            {
                AngazovanId = model.AngazujeId,
                Datum = model.Datum
            };

            if (await _odrzaniCasService.Dodaj(noviCas))
            {
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();


        }

        public async Task<IActionResult> Uredi(int Id)
        {
            var currentUser = await HttpContext.GetLoggedInUser();

            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            var odrzaniCas = await _dbContext.OdrzaniCasovi
                .Include(x=>x.Angazovan)
                .ThenInclude(x=>x.AkademskaGodina)
                .Include(x=>x.Angazovan)
                .ThenInclude(x=>x.Predmet)
                .FirstOrDefaultAsync(x=>x.Id==Id);

            if (odrzaniCas == null)
                return NotFound();

            var vModel = await BuildOdrzaniCasDetaljiVM(odrzaniCas,nastavnik);

            return View("Detalji", vModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OdrzaniCasDetaljiVM model)
        {

            var currentUser = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            var casFromDb = await _dbContext.OdrzaniCasovi
                .Include(x => x.Angazovan)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (casFromDb == null)
                return NotFound();

            //Provera da li je trenutno prijavljeni nastavnik kreirao cas koji se namerava izmeniti
            if (casFromDb.Angazovan.NastavnikId != nastavnik.Id)
            {
                return Unauthorized();
            }

            casFromDb.Datum = model.Datum;
            _dbContext.Update(casFromDb);
            await _dbContext.SaveChangesAsync();

            return Ok("Uspjesno evidentiran datum odrzanog casa.");
        }


        [Route("/Prisustvo/Prisutan/{odrzaniCasDetaljId}")]
        public async Task<IActionResult> Prisutan(int odrzaniCasDetaljId)
        {
            var currentUser = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            var prisustvo = await _dbContext.OdrzaniCasDetalji
                .Include(x=>x.OdrzaniCas)
                .ThenInclude(x=>x.Angazovan)
                .Include(x=>x.SlusaPredmet)
                .ThenInclude(x=>x.UpisGodine)
                .ThenInclude(x=>x.Student)
                .FirstOrDefaultAsync(x=>x.Id==odrzaniCasDetaljId);

            if (prisustvo == null)
                return NotFound();

            if (prisustvo.OdrzaniCas.Angazovan.NastavnikId != nastavnik.Id)
            {
                return Unauthorized();
            }

            prisustvo.Prisutan = !prisustvo.Prisutan;
            _dbContext.Update(prisustvo);
            await _dbContext.SaveChangesAsync();

            return PartialView("_OdrzaniCasDetaljRow",await BuildOdrzaniCasDetaljVM(prisustvo));
        }


        [Route("/Prisustvo/Edit/{odrzaniCasDetaljId}")]
        public async Task<IActionResult> UrediPrisustvo(int odrzaniCasDetaljId)
        {
            var currentUser = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            var prisustvo = await _dbContext.OdrzaniCasDetalji
                .Include(x => x.OdrzaniCas)
                .ThenInclude(x => x.Angazovan)
                .Include(x => x.SlusaPredmet)
                .ThenInclude(x => x.UpisGodine)
                .ThenInclude(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == odrzaniCasDetaljId);

            if (prisustvo == null)
                return NotFound();

            if (prisustvo.OdrzaniCas.Angazovan.NastavnikId != nastavnik.Id)
            {
                return Unauthorized();
            }

            var vModel = new PrisustvoInputVM
            {
                Id=prisustvo.Id,
                Student=prisustvo.SlusaPredmet?.UpisGodine?.Student?.ImePrezime()??NOT_FOUND
            };

            return PartialView("_PrisustvoEdit", vModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SnimiPrisustvo(PrisustvoInputVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Podaci nisu validni.");
            }

            var currentUser = await HttpContext.GetLoggedInUser();
            var nastavnik = await _dbContext.Nastavnik.FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

            if (nastavnik == null)
                return NotFound();

            var prisustvo = await _dbContext.OdrzaniCasDetalji
                .Include(x => x.OdrzaniCas)
                .ThenInclude(x => x.Angazovan)
                .Include(x=>x.SlusaPredmet)
                .ThenInclude(x=>x.UpisGodine)
                .ThenInclude(x=>x.Student)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (prisustvo == null)
                return NotFound();

            if (prisustvo.OdrzaniCas.Angazovan.NastavnikId != nastavnik.Id)
            {
                return Unauthorized();
            }

            prisustvo.BodoviNaCasu = model.Bodovi;
            _dbContext.Update(prisustvo);
            await _dbContext.SaveChangesAsync();


            return PartialView("_OdrzaniCasDetaljRow", await BuildOdrzaniCasDetaljVM(prisustvo));
        }






        //VM Builders

        private async Task<OdrzaniCasDetaljiVM> BuildOdrzaniCasDetaljiVM(OdrzaniCas cas, Nastavnik nastavnik)
        {
            var prisustva = _dbContext.OdrzaniCasDetalji
                .Include(x=>x.SlusaPredmet)
                .ThenInclude(x=>x.UpisGodine)
                .ThenInclude(x=>x.Student)
                .Where(x => x.OdrzaniCasId == cas.Id);


            var prisustvaVM = new List<OdrzaniCasDetaljVM>();

            if (await prisustva.AnyAsync())
            {
                prisustvaVM = await prisustva.Select(x => new OdrzaniCasDetaljVM
                {
                    Id=x.Id,
                    Bodovi = x.BodoviNaCasu,
                    IsPrisutan = x.Prisutan,
                    Student = x.SlusaPredmet.UpisGodine.Student.ImePrezime()
                }).ToListAsync();
            }
            return new OdrzaniCasDetaljiVM
            {
                Nastavnik = nastavnik.ImePrezime(),
                AkademskaGodinaPredmet = cas.Angazovan?.AkademskaGodina?.Opis??NOT_FOUND,
                Datum = cas.Datum,
                Id = cas.Id,
                Prisustva = prisustvaVM
            };
        }

        private async Task<OdrzaniCasDetaljVM> BuildOdrzaniCasDetaljVM(OdrzaniCasDetalji prisustvo)
        {
            if(prisustvo==null)
                return new OdrzaniCasDetaljVM();

            return new OdrzaniCasDetaljVM
            {
                Id = prisustvo.Id,
                Bodovi = prisustvo.BodoviNaCasu,
                IsPrisutan = prisustvo.Prisutan,
                Student = prisustvo.SlusaPredmet.UpisGodine.Student.ImePrezime()
            };
        }
        private async Task<OdrzaniCasoviListVM> BuildOdrzaniCasoviListVM(Nastavnik nastavnik)
        {
            if (nastavnik == null)
                return new OdrzaniCasoviListVM();

            var odrzaniCasovi = _dbContext.OdrzaniCasovi
                .Include(x => x.Angazovan)
                .ThenInclude(x => x.Predmet)
                .Include(x => x.Angazovan)
                .ThenInclude(x => x.AkademskaGodina)
                .Where(x => x.Angazovan.NastavnikId == nastavnik.Id);

            var odrzaniCasoviVM = new List<OdrzaniCasVM>();

            if (await odrzaniCasovi.AnyAsync())
            {
                foreach (var x in odrzaniCasovi)
                {
                    odrzaniCasoviVM.Add(new OdrzaniCasVM
                    {
                        Id = x.Id,
                        AkademskaGodina = x.Angazovan.AkademskaGodina.Opis,
                        BrojPrisutnih = _dbContext.OdrzaniCasDetalji.Count(z => z.OdrzaniCasId == z.Id),
                        ProsjecnaOcjena = _odrzaniCasService.GetProsjecnuOcjenu(x.AngazovanId), 
                        Datum = x.Datum,
                        Predmet = x.Angazovan.Predmet.Naziv
                    });
                }
               
            }

            return new OdrzaniCasoviListVM
            {
                Nastavnik = nastavnik.ImePrezime(),
                OdrzaniCasovi = odrzaniCasoviVM
            };
        }

        private async Task<OdrzaniCasInputVM> BuildOdrzaniCasInputVM(Nastavnik nastavnik)
        {
            if (nastavnik == null)
                return new OdrzaniCasInputVM();

                return new OdrzaniCasInputVM
                {
                    Nastavnik = nastavnik.ImePrezime(),
                    AkademskeGodinePredmeti = GetListPredmeta(nastavnik.Id),
                    Datum = DateTime.Now.AddDays(1)
                };
        }

        private List<SelectListItem> GetListPredmeta(int nastavnikId)
        {
            return _dbContext.Angazovan
                .Include(x => x.AkademskaGodina)
                .Include(x => x.Predmet)
                .Where(x => x.NastavnikId == nastavnikId)
                .AsEnumerable()
                .ToSelectList(x => x.Id.ToString(), x => x.AkademskaGodina.Opis + " / "
                                                                                + x.Predmet.Naziv,
                    "Odaberite ak. godinu / predmet");
        }
    }
}