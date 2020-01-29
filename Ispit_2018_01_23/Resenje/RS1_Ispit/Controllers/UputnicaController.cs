using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.Constants;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.Helpers;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Ispit_2017_09_11_DotnetCore.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1.Ispit.Web.Models;
using SQLitePCL;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class UputnicaController : Controller
    {
        private readonly MojContext _context;
        private readonly IDataProtector _protector;
        private readonly IUputnicaService _uputnicaService;

        public UputnicaController(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants,
            IUputnicaService uputnicaService)
        {
            _context = context;
            _uputnicaService = uputnicaService;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }


        public async Task<IActionResult> Index()
        {
            var model = await BuildUputniceViewModel();

            return View(model);
        }

        public IActionResult Dodaj() => PartialView("_NovaPartial", BuildUputnicaInputViewModel());



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(UputnicaInputVM model)
        {
            if (ModelState.IsValid)
            {
                var novaUputnica = new Uputnica
                { 
                    DatumUputnice = model.DatumUputnice,
                    DatumRezultata = null,
                    IsGotovNalaz = false,
                    UputioLjekarId = int.Parse(_protector.Unprotect(model.LjekarUputioId)),
                    PacijentId = int.Parse(_protector.Unprotect(model.PacijentId)),
                    VrstaPretrageId = model.VrstaPretrageId,
                    LaboratorijLjekarId=null
                };

                var rezultatKreiranja = await _uputnicaService.DodajAsync(novaUputnica);

                if (rezultatKreiranja.Success)
                    return Ok(rezultatKreiranja.Message);

                return BadRequest(rezultatKreiranja.Message);
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            return BadRequest(errors.Any()? errors.First().ToString() : "Greska");
        }



        public async Task<IActionResult> Detalji(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            var uputnica = await _context.Uputnica
                .Include(x => x.Pacijent)
                .FirstOrDefaultAsync(x => x.Id == decryptedId);

            if (uputnica == null)
            {
                TempData["error"] = "Uputnica nije pronadjena.";
                return RedirectToAction(nameof(Index));
            }


            var model = await BuildDetaljiUputniceViewModel(uputnica);

            return View(model);
        }

        public async Task<IActionResult> Zakljucaj(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            var uputnica = await _context.Uputnica.FindAsync(decryptedId);

            if (uputnica == null)
                return BadRequest("Uputnica nije pronadjena.");

            uputnica.IsGotovNalaz = true;
            uputnica.DatumRezultata = DateTime.Now;

            _context.Update(uputnica);

            await _context.SaveChangesAsync();

            return Ok(uputnica.DatumRezultata.Value.ToString("g"));
        }





        //ViewModel Builders
        private async Task<UputniceVM> BuildUputniceViewModel()
        {
            var uputnice = new List<UputnicaVM>();

            if(!await _context.Uputnica.AnyAsync())
                return new UputniceVM{Uputnice = uputnice};

            uputnice = await _context.Uputnica
                .Include(x => x.VrstaPretrage)
                .Include(x => x.UputioLjekar)
                .Include(x=>x.Pacijent)
                .Select(x => new UputnicaVM
                {
                    Id=_protector.Protect(x.Id.ToString()),
                    DatumEvidentiranjaRezultataPretrage = x.DatumRezultata,
                    DatumUputnice = x.DatumUputnice,
                    Pacijent = x.Pacijent.Ime,
                    UputioLjekar = x.UputioLjekar.Ime,
                    VrstaPretraga = x.VrstaPretrage.Naziv
                }).ToListAsync();

            return new UputniceVM
            {
                Uputnice = uputnice
            };
        }

        private UputnicaInputVM BuildUputnicaInputViewModel()
        {
            return new UputnicaInputVM
            {
                DatumUputnice = DateTime.Now.Date,
                Ljekari= _context.Ljekar.ToSelectList(x => _protector.Protect(x.Id.ToString()),
                    x => x.Ime, "Odaberite ljekara"),
                Pacijenti= _context.Pacijent.ToSelectList(x => _protector.Protect(x.Id.ToString()),
                    x => x.Ime, "Odaberite pacijenta"),
                VrstePretrage = _context.VrstaPretrage.ToSelectList(x => x.Id.ToString(),
                    x => x.Naziv, "Odaberite vrstu pretrage")
            };
        }

        private async Task<DetaljiUputniceVM> BuildDetaljiUputniceViewModel(Uputnica uputnica)
        {
            var rezultatiPretragaVM = new List<RezultatPretrageVM>();

            if (uputnica == null)
                return new DetaljiUputniceVM
                {
                    RezultatiPretraga = rezultatiPretragaVM
                };

             var rezultatiPretraga = _context.RezultatPretrage
                 .Include(x=>x.LabPretraga)
                 .Where(x => x.UputnicaId == uputnica.Id);


            if(!await rezultatiPretraga.AnyAsync())
                return new DetaljiUputniceVM
                {
                    RezultatiPretraga = rezultatiPretragaVM
                };

           var model = new DetaljiUputniceVM
            {
                Id=_protector.Protect(uputnica.Id.ToString()),
                DatumUputnice = uputnica.DatumUputnice,
                DatumRezultata = uputnica.DatumRezultata,
                IsZavrsenUnos = uputnica.IsGotovNalaz,
                Pacijent = uputnica.Pacijent.Ime,
                RezultatiPretraga = new List<RezultatPretrageVM>()
            };

           foreach (var x in rezultatiPretraga)
           {
               if(x.ModalitetId.HasValue)
                   x.Modalitet=await _context.Modalitet.FindAsync(x.ModalitetId);
               model.RezultatiPretraga.Add(await BuildRezultatPretrageVM(x));
           }



           return model;
        }

        private async Task<RezultatPretrageVM> BuildRezultatPretrageVM(RezultatPretrage x)
        {
            return new RezultatPretrageVM
            {
                Id = _protector.Protect(x.Id.ToString()),
                IzmjerenaVrijednost =
                    x.NumerickaVrijednost == null ? x.Modalitet?.Opis??"" : x.NumerickaVrijednost?.ToString()??"",
                JMJ = x.ModalitetId == null ? x.LabPretraga.MjernaJedinica : string.Empty,
                Pretraga = x.LabPretraga.Naziv,
                ReferentnaVrijednost = await _uputnicaService.GetReferentneVrijednosti(x.LabPretragaId),
                VrstaVrijednosti = x.LabPretraga.VrstaVr,
                IsReferentnaVrijednost = x.ModalitetId.HasValue
                    ? x.Modalitet?.IsReferentnaVrijednost??true
                    : !x.NumerickaVrijednost.HasValue || (x.NumerickaVrijednost >= x.LabPretraga.ReferentnaVrijednostMin &&
                                                          x.NumerickaVrijednost <= x.LabPretraga.ReferentnaVrijednostMax),
                ModalitetId = x.ModalitetId,
                Modaliteti = _context.Modalitet
                                 .Where(z => z.LabPretragaId == x.LabPretragaId)?
                                 .ToSelectList(z => z.Id.ToString(),
                                                  z => z.Opis, defaultId: (await _context.Modalitet.FindAsync(x.ModalitetId ?? 0))?.Id ?? 0)
                                                      ?? new List<SelectListItem>()
        };
        }
    }
}