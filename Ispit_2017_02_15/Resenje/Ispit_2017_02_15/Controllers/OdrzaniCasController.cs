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


        public async Task<IActionResult> Edit(int Id)
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

            var vModel = await BuildOdrzaniCasInputVM(nastavnik, odrzaniCas);

            return View("OdrzaniCasForm", vModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Snimi(OdrzaniCasInputVM model)
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

            if (!model.Id.HasValue && !model.AngazujeId.HasValue)
            {
                model.AkademskeGodinePredmeti = GetListPredmeta(nastavnik.Id);

                ModelState.AddModelError(string.Empty,"Morate odabrati skolsku godinu i predmet.");
                return View("OdrzaniCasForm", model);
            }

            if (!model.Id.HasValue && !await _dbContext.Angazovan.AnyAsync(x => x.NastavnikId == nastavnik.Id && model.AngazujeId == x.Id))
            {
                return Unauthorized();
            }

            if (!model.Id.HasValue)
            {
                var noviCas = new OdrzaniCas
                {
                    AngazovanId = model.AngazujeId.Value,
                    Datum = model.Datum
                };

                if (await _odrzaniCasService.Dodaj(noviCas))
                {
                    return RedirectToAction(nameof(Index));
                }

                return BadRequest();
            }

            var casFromDb = await _dbContext.OdrzaniCasovi.FindAsync(model.Id.Value);

            if(casFromDb==null)
                return NotFound();

            casFromDb.Datum = model.Datum;
            _dbContext.Update(casFromDb);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
                odrzaniCasoviVM = await odrzaniCasovi.Select(x => new OdrzaniCasVM
                {
                    Id = x.Id,
                    AkademskaGodina = x.Angazovan.AkademskaGodina.Opis,
                    Datum = x.Datum,
                    Predmet = x.Angazovan.Predmet.Naziv
                }).ToListAsync();
            }

            return new OdrzaniCasoviListVM
            {
                Nastavnik = nastavnik.ImePrezime(),
                OdrzaniCasovi = odrzaniCasoviVM
            };
        }

        private async Task<OdrzaniCasInputVM> BuildOdrzaniCasInputVM(Nastavnik nastavnik, OdrzaniCas odrzaniCas = null)
        {
            if (nastavnik == null)
                return new OdrzaniCasInputVM();

            if (odrzaniCas == null)
            {
                return new OdrzaniCasInputVM
                {
                    Nastavnik = nastavnik.ImePrezime(),
                    AkademskeGodinePredmeti = GetListPredmeta(nastavnik.Id),
                    Datum = DateTime.Now.AddDays(1)
                };
            }

            return new OdrzaniCasInputVM
            {
                Id = odrzaniCas.Id,
                Nastavnik = nastavnik.ImePrezime(),
                Datum = DateTime.Now.AddDays(1),
                Angazman = (odrzaniCas.Angazovan?.AkademskaGodina?.Opis ?? NOT_FOUND) + " / " + (odrzaniCas.Angazovan?.Predmet?.Naziv ?? NOT_FOUND)
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