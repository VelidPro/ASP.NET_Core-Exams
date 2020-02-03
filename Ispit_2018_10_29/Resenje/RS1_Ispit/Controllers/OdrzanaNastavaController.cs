using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Helpers;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class OdrzanaNastavaController : Controller
    {
        private readonly MojContext _context;
        private readonly IOdrzanaNastavaService _nastavaService;

        public OdrzanaNastavaController(MojContext context,
            IOdrzanaNastavaService nastavaService)
        {
            _context = context;
            _nastavaService = nastavaService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await BuildNastavniciPredavaciVM();
            return View(model);
        }

        public async Task<IActionResult> GetAll(int nastavnikId)
        {
            var model = await BuildOdrzaniCasoviVM(nastavnikId);

            return View("All", model);
        }

        public async Task<IActionResult> DodajNoviCas(int nastavnikId)
        {
            var model = await BuildDodavanjeCasaVM(nastavnikId);

            return PartialView("_DodavanjeOdrzanogCasa", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajOdrzaniCas(DodavanjeCasaVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            var predajePredmet = await _context.PredajePredmet.FindAsync(model.PredajePredmetId);

            if (predajePredmet == null)
                return BadRequest("Predmet i odjeljenje nisu pronadjeni.");

            var noviCas = new OdrzaniCas
            {
                Datum = model.DatumOdrzavanja,
                OdjeljenjeId = predajePredmet.OdjeljenjeID,
                PredajePredmetId = predajePredmet.Id,
                Napomena = string.Empty
            };

            var dodavanjeResult = await _nastavaService.DodajOdrzaniCas(noviCas);

            if (dodavanjeResult.Success)
                return ViewComponent("SingleOdrzaniCas", new { odrzaniCasId = noviCas.Id });

            return BadRequest(dodavanjeResult.Message);
        }

        public async Task<IActionResult> Obrisi(int odrzaniCasId)
        {
            var brisanjeResult = await _nastavaService.ObrisiOdrzaniCas(odrzaniCasId);

            if (brisanjeResult.Success)
                return Ok(brisanjeResult.Message);
            return BadRequest(brisanjeResult.Message);
        }

        public async Task<IActionResult> Detalji(int odrzaniCasId)
        {
            var model = await BuildOdrzaniCasDetaljiVM(odrzaniCasId);

            return View(model);
        }

        public async Task<IActionResult> UrediPrisustvo(int odrzaniCasStavkaId)
        {
            var model = await BuildUredjivanjaPrisustvaVM(odrzaniCasStavkaId);

            return PartialView("_UredjivanjePrisustva", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediPrisustvo(UredjivanjePrisustvaVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");
            try
            {
                var odrzaniCasStavka = await _context.OdrzaniCasStavke.FindAsync(model.OdrzaniCasStavkaId);

                if (odrzaniCasStavka == null)
                    return BadRequest("Prisustvo nije pronadjeno.");

                if (model.IsPrisutan)
                {
                    odrzaniCasStavka.Ocjena = model.Ocjena;
                }
                else
                {
                    odrzaniCasStavka.OpravdanoOdsustvo = model.OpravdanoOdsutan;
                    odrzaniCasStavka.Napomena = model.Napomena;
                }

                _context.Update(odrzaniCasStavka);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return ViewComponent("SinglePrisustvo", new { odrzaniCasStavkaId = model.OdrzaniCasStavkaId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOdrzaniCas([FromForm]string napomena, [FromForm] int odrzaniCasId)
        {
            if (string.IsNullOrEmpty(napomena))
                return BadRequest("Napomena ne moze biti prazna.");

            var odrzaniCas = await _context.OdrzaniCasovi.FindAsync(odrzaniCasId);

            if (odrzaniCas == null)
                return BadRequest("Odrzani cas nije pronadjen.");

            odrzaniCas.Napomena = napomena;

            _context.Update(odrzaniCas);

            await _context.SaveChangesAsync();

            return Ok("Uspjesna izmjena napomene.");
        }

        public async Task<IActionResult> PrisutanToggle(int odrzaniCasStavkaId)
        {
            try
            {
                var stavka = await _context.OdrzaniCasStavke.FindAsync(odrzaniCasStavkaId);

                if (stavka == null)
                    return BadRequest("Prisustvo nije pronadjeno.");

                stavka.IsPrisutan = !stavka.IsPrisutan;

                _context.Update(stavka);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return ViewComponent("SinglePrisustvo", new {odrzaniCasStavkaId = odrzaniCasStavkaId});
        }


        private async Task<UredjivanjePrisustvaVM> BuildUredjivanjaPrisustvaVM(int odrzaniCasStavkaId)
        {
            var odrzaniCasStavka = await _context.OdrzaniCasStavke
                .Include(x => x.OdjeljenjeStavka)
                .ThenInclude(x => x.Odjeljenje)
                .ThenInclude(x => x.SkolskaGodina)
                .Include(x => x.OdjeljenjeStavka)
                .ThenInclude(x => x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == odrzaniCasStavkaId);

            if (odrzaniCasStavka == null)
                return new UredjivanjePrisustvaVM();

            return new UredjivanjePrisustvaVM
            {
                OdrzaniCasStavkaId = odrzaniCasStavka.Id,
                Napomena = odrzaniCasStavka.Napomena,
                Ocjena = odrzaniCasStavka.Ocjena,
                IsPrisutan = odrzaniCasStavka.IsPrisutan,
                OpravdanoOdsutan = odrzaniCasStavka.OpravdanoOdsustvo,
                UcenikGodina = string.Concat(odrzaniCasStavka.OdjeljenjeStavka.Ucenik.ImePrezime, " (", odrzaniCasStavka.OdjeljenjeStavka.Odjeljenje.SkolskaGodina.Naziv, ")")
            };
        }

        private async Task<OdrzaniCasDetaljiVM> BuildOdrzaniCasDetaljiVM(int odrzaniCasId)
        {
            var odrzaniCas = await _context.OdrzaniCasovi
                .Include(x => x.PredajePredmet)
                .ThenInclude(x => x.Predmet)
                .Include(x => x.PredajePredmet)
                .ThenInclude(x => x.Odjeljenje)
                .FirstOrDefaultAsync(x => x.Id == odrzaniCasId);

            if (odrzaniCas == null)
                return new OdrzaniCasDetaljiVM { Prisustva = new List<OdrzaniCasStavkaVM>() };

            var prisustva = _context.OdrzaniCasStavke
                .Include(x => x.OdjeljenjeStavka)
                .ThenInclude(x => x.Ucenik)
                .Where(x => x.OdrzaniCasId == odrzaniCasId);
            var prisustvaVM = new List<OdrzaniCasStavkaVM>();

            if (await prisustva.AnyAsync())
            {
                prisustvaVM = await prisustva.Select(x => new OdrzaniCasStavkaVM
                {
                    Id = x.Id,
                    Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                    Ocjena = x.Ocjena,
                    IsPrisutan = x.IsPrisutan,
                    OpravdanoOdsutan = x.OpravdanoOdsustvo
                }).ToListAsync();
            }

            return new OdrzaniCasDetaljiVM
            {
                OdrzaniCasId = odrzaniCas.Id,
                Datum = odrzaniCas.Datum,
                Napomena = odrzaniCas.Napomena,
                Odjeljenje = string.Concat(odrzaniCas.Odjeljenje?.Oznaka ?? "", " / ", odrzaniCas.PredajePredmet?.Predmet?.Naziv ?? ""),
                Prisustva = prisustvaVM
            };
        }

        private async Task<NastavniciPredavaciVM> BuildNastavniciPredavaciVM()
        {
            var nastavnici = await _nastavaService.GetNastavnikePredavace();

            var nastavniciVM = new List<NastavnikVM>();

            if (nastavnici.Any())
                nastavniciVM = nastavnici.Select(x => new NastavnikVM
                {
                    Id = x.Id,
                    ImePrezime = string.Concat(x.Ime, " ", x.Prezime),
                    Skola = x.Skola.Naziv
                }).ToList();

            return new NastavniciPredavaciVM
            {
                Nastavnici = nastavniciVM
            };
        }

        private async Task<OdrzaniCasoviVM> BuildOdrzaniCasoviVM(int nastavnikId)
        {
            var odrzaniCasoviVM = new List<OdrzaniCasVM>();

            var nastavnik = await _context.Nastavnik.FindAsync(nastavnikId);

            if (nastavnik == null)
                return new OdrzaniCasoviVM
                {
                    OdrzaniCasovi = odrzaniCasoviVM
                };

            var odrzaniCasovi = _context.OdrzaniCasovi
                .Include(x => x.PredajePredmet)
                .ThenInclude(x => x.Predmet)
                .Include(x => x.Odjeljenje)
                .ThenInclude(x => x.SkolskaGodina)
                .Where(x => x.PredajePredmet.NastavnikID == nastavnikId);

            if (await odrzaniCasovi.AnyAsync())
            {
                foreach (var x in odrzaniCasovi)
                {
                    odrzaniCasoviVM.Add(new OdrzaniCasVM
                    {
                        Id = x.Id,
                        Datum = x.Datum,
                        Predmet = x.PredajePredmet.Predmet.Naziv,
                        OdsutniUcenici = await _nastavaService.GetOdsutniUceniciString(x.Id),
                        SkGodinaOdjeljenje = string.Concat(x.Odjeljenje.SkolskaGodina.Naziv, " ", x.Odjeljenje.Oznaka)
                    });
                }
            }

            return new OdrzaniCasoviVM
            {
                NastavnikId = nastavnikId,
                OdrzaniCasovi = odrzaniCasoviVM
            };
        }

        private async Task<DodavanjeCasaVM> BuildDodavanjeCasaVM(int nastavnikId)
        {
            var nastavnik = await _context.Nastavnik.FindAsync(nastavnikId);
            var predmeti = await _nastavaService.GetPredmetePredaje(nastavnikId);

            return new DodavanjeCasaVM
            {
                Nastavnik = string.Concat(nastavnik?.Ime ?? "", " ", nastavnik?.Prezime ?? ""),
                NastavnikId = nastavnikId,
                DatumOdrzavanja = DateTime.Now.AddDays(1).Date,
                OdjeljenjaPredmeti = predmeti.ToSelectList(x => x.Id.ToString(),
                    x => string.Concat(x.Odjeljenje.Oznaka, " / ", x.Predmet.Naziv), defaultOption: "Odaberite odjeljenje/predmet")
            };
        }
    }
}