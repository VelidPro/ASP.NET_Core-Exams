using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Helpers;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewComponents;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class PopravniIspitController : Controller
    {
        private readonly MojContext _context;
        private readonly IPopravniIspitService _popravniIspitService;

        public PopravniIspitController(MojContext context
            , IPopravniIspitService popravniIspitService)
        {
            _context = context;
            _popravniIspitService = popravniIspitService;
        }


        public async Task<IActionResult> Index()
        {
            var model = await BuildParametriPretragePopravnihVM();

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pretraga(ParametriPretragePopravnihVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Parametri pretrage nisu validni.");

            var modelVM = await BuildRezultatPretragePopravnihVM(model.SkolskaGodinaId, model.PredmetId, model.SkolaId);

            return View("RezultatPretrage", modelVM);
        }

        public async Task<IActionResult> Dodaj(int predmetId, int skolaId, int skolskaGodinaId)
        {
            if (!await _context.Predmet.AnyAsync(x => x.Id == predmetId) ||
                !await _context.Skola.AnyAsync(x => x.Id == skolaId)
                || !await _context.SkolskaGodina.AnyAsync(x => x.Id == skolskaGodinaId))
                return BadRequest("Podaci nisu validni.");

            var model = await BuildDodavanjePopravnogIspitaVM(skolaId, predmetId, skolskaGodinaId);

            return PartialView("_Novi", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(DodavanjePopravnogIspitaVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podacni nisu validni.");

            var novi = new PopravniIspit
            {
                DatumOdrzavanja = model.DatumIspita,
                PredmetId = model.PredmetId,
                SkolaId = model.SkolaId,
                SkolskaGodinaId = model.SkolskaGodinaId,
            };

            var dodavanjeResult = await _popravniIspitService.Dodaj(novi,model.ClanoviKomisijaIds);

            if (dodavanjeResult.Success)
                return ViewComponent("SinglePopravniIspit",new{popravniIspitId=novi.Id});

            return BadRequest(dodavanjeResult.Message);
        }


        public async Task<IActionResult> Detalji(int popravniIspitId)
        {
            var popravni = await _context.PopravniIspiti
                .Include(x=>x.Skola)
                .Include(x=>x.SkolskaGodina)
                .Include(x=>x.Predmet)
                .FirstOrDefaultAsync(x=>x.Id==popravniIspitId);

            if (popravni == null)
                return BadRequest("Popravni ispit ne postoji.");

            var model = await BuildDetaljiPopravniIspitVM(popravni);

            return View(model);
        }

        public async Task<IActionResult> UrediPolaganje(int popravniIspitStavkaId, int popravniIspitId)
        {
            var model = await BuildPopravniIspitInputVM(popravniIspitStavkaId, popravniIspitId);

            TempData["formType"] = "Izmjena";
            return PartialView("_FormaPolaganje", model);
        }

        public async Task<IActionResult> DodajPolaganje(int popravniIspitId)
        {
            var model = await BuildPopravniIspitInputVM(popravniIspitId: popravniIspitId);

            TempData["formType"] = "Dodavanje";
            return PartialView("_FormaPolaganje", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SnimiPolaganje(PopravniIspitStavkaInputVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            PopravniIspitStavka stavka;
            if (model.Id.HasValue)
            {
                stavka = await _context.PopravniIspitStavke.FindAsync(model.Id);

                if (stavka == null)
                    return BadRequest("Polaganje nije pronadjena.");

                stavka.OsvojeniBodovi = model.Bodovi;
                _context.Update(stavka);
            }
            else
            {
                stavka = new PopravniIspitStavka
                {
                    ImaPravoNaIzlazask = true,
                    IsPrisutupio = false,
                    PopravniIspitId = model.PopravniIspitId,
                    OsvojeniBodovi = model.Bodovi,
                    UcenikId = model.UcenikId
                };
                await _context.AddAsync(stavka);
            }

            await _context.SaveChangesAsync();
            return ViewComponent("SinglePolaganjePopravnog", new {popravniIspitStavkaId = stavka.Id});

        }


        public async Task<IActionResult> PolaganjePristupioToggle(int Id)
        {
            var polaganje = await _context.PopravniIspitStavke.FindAsync(Id);

            if (polaganje == null)
                return BadRequest("Polaganje nije pronadjeno.");

            polaganje.IsPrisutupio = !polaganje.IsPrisutupio;

            _context.Update(polaganje);
            await _context.SaveChangesAsync();

            return ViewComponent("SinglePolaganjePopravnog", new {popravniIspitStavkaId = Id});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EvidencijaBodova([FromForm] int bodovi, [FromForm] int popravniIspitStavkaId)
        {
            var popravniIspitStavka = await _context.PopravniIspitStavke.FindAsync(popravniIspitStavkaId);

            if (popravniIspitStavka == null)
                return BadRequest("Polaganje nije pronadjeno.");

            if (bodovi < 0 || bodovi > 100)
                return BadRequest("Broj bodova mora biti izmedju 0 i 100");

            popravniIspitStavka.OsvojeniBodovi = bodovi;

            _context.Update(popravniIspitStavka);

            await _context.SaveChangesAsync();

            return Ok("Uspjesno evidentirani bodovi.");
        }




        private async Task<ParametriPretragePopravnihVM> BuildParametriPretragePopravnihVM()
        {
            var skole = _context.Skola
                .AsEnumerable()
                .ToSelectList(x => x.Id.ToString(), x => x.Naziv, 0, "Odaberite skolu");

            var skolskeGodine = _context.SkolskaGodina
                .AsEnumerable()
                .ToSelectList(x => x.Id.ToString(), x => x.Naziv, 0, "Odaberite skolsku godinu");

            var predmeti = _context.Predmet
                .AsEnumerable()
                .ToSelectList(x => x.Id.ToString(), x => x.Naziv, 0, "Odaberite predmet");

            return new ParametriPretragePopravnihVM{
                Predmeti=predmeti,
                SkolskeGodine=skolskeGodine,
                Skole=skole
            };
        }

        private async Task<RezultatPretragePopravnihVM> BuildRezultatPretragePopravnihVM(int skolskaGodinaId,int predmetId,int skolaId)
        {
            var popravniIspiti = _context.PopravniIspiti
                .Where(x => x.SkolskaGodinaId == skolskaGodinaId && x.PredmetId == predmetId && x.SkolaId == skolaId);

            var popravniIspitiVM = new List<PopravniIspitVM>();

            if (await popravniIspiti.AnyAsync())
            {
                foreach (var p in popravniIspiti)
                {
                    popravniIspitiVM.Add(new PopravniIspitVM
                    {
                        Id=p.Id,
                        BrojKojiNisuPolozili = await _popravniIspitService.BrojUcenikaNisuPolozili(p.Id),
                        BrojUcenika = await _context.PopravniIspitStavke
                            .CountAsync(x=>x.PopravniIspitId==p.Id),
                        Datum=p.DatumOdrzavanja,
                        Komisija = string.Join(", ", await _context.PopravniIspitKomisija
                            .Include(x => x.Nastavnik)
                            .Where(x => x.PopravniIspitId == p.Id)
                            .Select(x=>x.Nastavnik.ImePrezime()).ToListAsync() ?? new List<string>())
                    });
                }
            }

            return new RezultatPretragePopravnihVM
            {
                SkolaId=skolaId,
                Skola=(await _context.Skola.FindAsync(skolaId))?.Naziv??"",
                PredmetId = predmetId,
                Predmet=(await _context.Predmet.FindAsync(predmetId))?.Naziv??"",
                SkolskaGodinaId = skolskaGodinaId,
                SkolskaGodina = (await _context.SkolskaGodina.FindAsync(skolskaGodinaId))?.Naziv??"",
                PopravniIspiti = popravniIspitiVM
            };
        }

        private async Task<DodavanjePopravnogIspitaVM> BuildDodavanjePopravnogIspitaVM(int skolaId,int predmetId, int skolskaGodinaId)
        {
            var potencijalniClanoviKomisije = _context.PredajePredmet
                .Include(x=>x.Nastavnik)
                .Where(x => x.PredmetID == predmetId)
                .ToSelectList(x=>x.Nastavnik.Id.ToString(),x=>x.Nastavnik.ImePrezime(),
                    defaultOption:"Odaborite clana komisije");

            return new DodavanjePopravnogIspitaVM{
                SkolaId = skolaId,
                Skola = (await _context.Skola.FindAsync(skolaId))?.Naziv ?? "",
                PredmetId = predmetId,
                Predmet = (await _context.Predmet.FindAsync(predmetId))?.Naziv ?? "",
                SkolskaGodinaId = skolskaGodinaId,
                SkolskaGodina = (await _context.SkolskaGodina.FindAsync(skolskaGodinaId))?.Naziv ?? "",
                DatumIspita = DateTime.Now.AddDays(1).Date,
                ClanoviKomisije = potencijalniClanoviKomisije
            };
        }

        private async Task<DetaljiPopravniIspitVM> BuildDetaljiPopravniIspitVM(PopravniIspit popravni)
        {
            var komisija = await _context.PopravniIspitKomisija
                .Include(x => x.Nastavnik)
                .Where(x => x.PopravniIspitId == popravni.Id)
                .Select(x => x.Nastavnik.ImePrezime())
                .ToListAsync();

            var polaganja = _context.PopravniIspitStavke
                .Include(x=>x.Ucenik)
                .Where(x => x.PopravniIspitId == popravni.Id);
            var polaganjaVM = new List<PopravniIspitStavkaVM>();

            if (await polaganja.AnyAsync())
            {
                foreach(var p in polaganja)
                {
                    var tempOdjeljenjeStavka = await _context.OdjeljenjeStavka
                        .Include(x=>x.Odjeljenje)
                        .FirstOrDefaultAsync(x => x.UcenikId == p.UcenikId);

                    polaganjaVM.Add(new PopravniIspitStavkaVM{
                        Id=p.Id,
                        BrojUDnevniku = tempOdjeljenjeStavka?.BrojUDnevniku??0,
                        ImaPravoIzlaska = p.ImaPravoNaIzlazask,
                        IsPristupio = p.IsPrisutupio,
                        Odjeljenje = tempOdjeljenjeStavka?.Odjeljenje?.Oznaka??"",
                        OsvojenoBodova = p.OsvojeniBodovi ?? 0,
                        Ucenik = p.Ucenik.ImePrezime
                    });
                }
            }

            return new DetaljiPopravniIspitVM{
                ClanoviKomisije = komisija,
                DatumIspita = popravni.DatumOdrzavanja,
                Id=popravni.Id,
                Polaganja = polaganjaVM,
                Predmet = popravni.Predmet.Naziv,
                PredmetId = popravni.PredmetId,
                Skola = popravni.Skola.Naziv,
                SkolaId = popravni.SkolaId,
                SkolskaGodinaId = popravni.SkolskaGodinaId,
                SkolskaGodina = popravni.SkolskaGodina.Naziv
            };
        }

        private async Task<PopravniIspitStavkaInputVM> BuildPopravniIspitInputVM(int popravniIspitStavkaId = 0,int popravniIspitId=0)
        {

            if (popravniIspitStavkaId == 0)
            {
                var popravniIspit = await _context.PopravniIspiti.FindAsync(popravniIspitId);
                if(popravniIspit==null)
                    return new PopravniIspitStavkaInputVM();

                var uceniciKojiSlusajuPredmet = _context.DodjeljenPredmet
                    .Include(x => x.OdjeljenjeStavka)
                    .ThenInclude(x => x.Ucenik)
                    .Where(x => x.PredmetId == popravniIspit.PredmetId)
                    .Select(x => x.OdjeljenjeStavka.Ucenik);

                    var uceniciVecPrijavljeni= _context.PopravniIspitStavke
                        .Where(x=>x.PopravniIspitId==popravniIspitId)
                        .Select(x=>x.UcenikId);

                return new PopravniIspitStavkaInputVM
                {
                    

                    PopravniIspitId = popravniIspitId,
                    Ucenici= uceniciKojiSlusajuPredmet.Where(x=>!uceniciVecPrijavljeni.Contains(x.Id))
                        .ToSelectList(x => x.Id.ToString(), x => x.ImePrezime, defaultOption: "Odaberite ucenika")
            };
            }

            var popravniIspitStavka = await _context.PopravniIspitStavke
                .Include(x=>x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == popravniIspitStavkaId);

            return new PopravniIspitStavkaInputVM{
                PopravniIspitId = popravniIspitId,
                Id=popravniIspitStavka.Id,
                Bodovi = popravniIspitStavka.OsvojeniBodovi??0,
                Ucenik=popravniIspitStavka.Ucenik.ImePrezime,
                UcenikId=popravniIspitStavka.UcenikId

            };
        }
    }
}