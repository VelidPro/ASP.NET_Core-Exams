using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Helpers;
using RS1_PrakticniDioIspita_2017_01_24.Models;
using RS1_PrakticniDioIspita_2017_01_24.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS1_PrakticniDioIspita_2017_01_24.Interfaces;

namespace RS1_PrakticniDioIspita_2017_01_24.Controllers
{
    [AuthorizationNastavnik]
    public class OdrzaniCasController : Controller
    {
        private readonly MojContext _context;
        private readonly IOdrzaniCasService _odrzaniCasService;

        public OdrzaniCasController(MojContext context, IOdrzaniCasService odrzaniCasService)
        {
            _context = context;
            _odrzaniCasService = odrzaniCasService;
        }

        [Route("/OdrzaniCas")]
        public async Task<IActionResult> All()
        {
            var user = await HttpContext.GetLoggedInUser();


            var nastavnik = await _context.Nastavnici
                .FirstOrDefaultAsync( x=> x.UserId== user.Id);


            if (nastavnik == null)
                return NotFound("Nastavnik nije pronadjen.");

            var vModel = await BuildOdrzaniCasoviVM(nastavnik);

            return View(vModel);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var odrzaniCas = await _context.OdrzaniCasovi
                .Include(x => x.Angazovan)
                .ThenInclude(x => x.Nastavnik)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (odrzaniCas == null)
                return NotFound("Odrzani cas nije pronadjen.");

            var vModel = await BuildOdrzaniCasInputVM(odrzaniCas: odrzaniCas);

            return PartialView("_OdrzaniCasForm", vModel);
        }


        public async Task<IActionResult> Snimi(OdrzaniCasInputVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new {ModelStateErrors = ModelState.Values.SelectMany(x => x.Errors)});
            }

            if (model.Id.HasValue)
            {
                var odrzaniCasFromDb = await _context.OdrzaniCasovi.FindAsync(model.Id.Value);

                if (odrzaniCasFromDb == null)
                    return NotFound("Odrzani cas nije pronadjen.");

                odrzaniCasFromDb.datum = model.DatumOdrzanogCasa;
                odrzaniCasFromDb.AngazovanId = model.AngazovanId;

                _context.Update(odrzaniCasFromDb);
            }
            else
            {
                var noviOdrzaniCas = new OdrzaniCas
                {
                    datum = model.DatumOdrzanogCasa,
                    AngazovanId = model.AngazovanId
                };

                var dodavanjeCasaResult = await _odrzaniCasService.DodajCas(noviOdrzaniCas);

                if (!dodavanjeCasaResult.Success)
                    return BadRequest(new
                    {
                        Error=dodavanjeCasaResult.Message,
                        Message="Dodavanje casa neuspjesno."
                    });
            }

            await _context.SaveChangesAsync();

            return Ok("Uspjesno evidentiran odrzani cas.");
        }


        public async Task<IActionResult> Dodaj(int nastavnikId)
        {
            var nastavnik = await _context.Nastavnici.FindAsync(nastavnikId);

            if (nastavnik == null)
                return NotFound("Nastavnik nije pronadjen.");

            var vModel = await BuildOdrzaniCasInputVM(nastavnik: nastavnik);

            ViewData["formType"] = "dodavanje";
            return PartialView("_OdrzaniCasForm", vModel);
        }


        public async Task<IActionResult> Detalji()
        {
            var user = await HttpContext.GetLoggedInUser();
            var nastavnik = await _context.Nastavnici
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (nastavnik == null)
                return NotFound("Nastavnik nije pronadjen.");

            var vModel = await BuildOdrzaniCasoviDetaljiVM(nastavnik);

            return View("AllDetaljiCasa",vModel);
        }


        public async Task<IActionResult> EditAll(int odrzaniCasId)
        {
            var odrzaniCas = await _context.OdrzaniCasovi
                .Include(x => x.Angazovan)
                .FirstOrDefaultAsync(x => x.Id == odrzaniCasId);

            if (odrzaniCas == null)
                return NotFound("Odrzani cas nije pronadjen");

            var vModel = await BuildOdrzaniCasEditVM(odrzaniCas);

            return View("Edit",vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(OdrzaniCasEditVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new {Errors = ModelState.Values.SelectMany(x => x.Errors)});

            var cas = await _context.OdrzaniCasovi.FindAsync(model.Id);

            if (cas == null)
                return NotFound("Odrzani cas nije pronadjen.");

            cas.datum = model.DatumOdrzavanja;
            cas.AngazovanId = model.AngazovanId;

            _context.Update(cas);
            await _context.SaveChangesAsync();

            return Ok("Uspjesno evidentirane izmjene za odrzani cas.");
        }



        private async Task<OdrzaniCasEditVM> BuildOdrzaniCasEditVM(OdrzaniCas odrzaniCas)
        {
            if (odrzaniCas == null)
                return null;

            var prisustva = _context.OdrzaniCasDetalji
                .Include(x => x.UpisUOdjeljenje)
                .ThenInclude(x => x.Ucenik)
                .Where(x => x.OdrzaniCasId == odrzaniCas.Id);

            var prisustvaVM = new List<OdrzaniCasDetaljEditVM>();

            if (await prisustva.AnyAsync())
            {
                prisustvaVM = await prisustva.Select(x => new OdrzaniCasDetaljEditVM
                {
                    Id = x.Id,
                    Odsutan = x.Odsutan,
                    OpravdanoOdsutan = x.OpravdanoOdsutan,
                    Ocjena = x.Ocjena,
                    Ucenik = x.UpisUOdjeljenje.Ucenik.Ime
                }).ToListAsync();
            }

            return new OdrzaniCasEditVM
            {
                AngazovanId = odrzaniCas.AngazovanId,
                DatumOdrzavanja = odrzaniCas.datum,
                Id = odrzaniCas.Id,
                Prisustva = prisustvaVM,
                OdjeljenjaPredmeti = await _odrzaniCasService.GetOdjeljenjaPredmeti(odrzaniCas.Angazovan.NastavnikId, odrzaniCas.AngazovanId)
            };
        }


        private async Task<OdrzaniCasoviDetaljiVM> BuildOdrzaniCasoviDetaljiVM(Nastavnik nastavnik)
        {
            var odrzaniCasovi = _context.OdrzaniCasovi
                .Include(x => x.Angazovan)
                .ThenInclude(x => x.Odjeljenje)
                .Where(x => x.Angazovan.NastavnikId == nastavnik.Id);

            var odrzaniCasoviDetaljiVM = new List<OdrzaniCasDetaljiOdrzavanjaVM>();

            if (await odrzaniCasovi.AnyAsync())
            {
                foreach (var x in odrzaniCasovi)
                {
                    odrzaniCasoviDetaljiVM.Add(new OdrzaniCasDetaljiOdrzavanjaVM
                    {
                        Id = x.Id,
                        Datum = x.datum,
                        Odjeljenje = x.Angazovan.Odjeljenje.Oznaka,
                        Predmet = (await _context.Predmeti.FindAsync(x.Angazovan.PredmetId))?.Naziv ?? "",
                        BrojPrisutnih = await _odrzaniCasService.BrojPrisutnih(x.Id),
                        NajboljiUcenik = (await _odrzaniCasService
                                             .NajboljiUcenik(x.Angazovan.PredmetId, x.Angazovan.OdjeljenjeId))?.Ime ?? string.Empty
                    });
                }
            }

            return new OdrzaniCasoviDetaljiVM
            {
                Nastavnik = nastavnik?.Ime ?? "",
                OdrzaniCasoviDetalji = odrzaniCasoviDetaljiVM
            };
        }


        private async Task<OdrzaniCasoviVM> BuildOdrzaniCasoviVM(Nastavnik nastavnik)
        {
            var odrzaniCasovi = _context.OdrzaniCasovi
                .Include(x => x.Angazovan)
                .ThenInclude(x => x.Predmet)
                .Include(x => x.Angazovan)
                .ThenInclude(x => x.Odjeljenje)
                .Where(x => x.Angazovan.NastavnikId == nastavnik.Id);

            var odrzaniCasoviVM = new List<OdrzaniCasVM>();

            if (await odrzaniCasovi.AnyAsync())
            {
                odrzaniCasoviVM = await odrzaniCasovi.Select(x => new OdrzaniCasVM
                {
                    Id = x.Id,
                    Datum = x.datum,
                    Odjeljenje = x.Angazovan.Odjeljenje.Oznaka,
                    Predmet = x.Angazovan.Predmet.Naziv
                }).ToListAsync();
            }

            return new OdrzaniCasoviVM
            {
                NastavnikId = nastavnik?.Id ?? 0,
                Nastavnik = nastavnik?.Ime ?? "",
                OdrzaniCasovi = odrzaniCasoviVM
            };
        }

        private async Task<OdrzaniCasInputVM> BuildOdrzaniCasInputVM(Nastavnik nastavnik = null, OdrzaniCas odrzaniCas = null)
        {
            var nastavnikId = nastavnik?.Id ?? odrzaniCas.Angazovan.NastavnikId;

            return new OdrzaniCasInputVM
            {
                AngazovanId = odrzaniCas?.AngazovanId??0,
                DatumOdrzanogCasa = odrzaniCas?.datum??DateTime.Now.AddDays(1),
                OdjeljenjaPredmeti = await _odrzaniCasService.GetOdjeljenjaPredmeti(nastavnikId, odrzaniCas?.AngazovanId),
                Nastavnik = odrzaniCas?.Angazovan.Nastavnik.Ime ?? (nastavnik!=null?nastavnik.Ime:string.Empty)
            };
        }
    }
}