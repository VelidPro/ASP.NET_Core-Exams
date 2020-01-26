using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.Constants;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.Helpers;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Ispit_2017_09_11_DotnetCore.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ispit_2017_09_11_DotnetCore.Controllers
{
    public class OdjeljenjeController : Controller
    {
        private readonly MojContext _context;
        private readonly IDataProtector _protector;
        private readonly IOdjeljenjeService _odjeljenjeService;
        private readonly IUcenikService _ucenikService;

        public OdjeljenjeController(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants,
            IOdjeljenjeService odjeljenjeService, 
            IUcenikService ucenikService)
        {
            _protector = protectionProvider.CreateProtector(securityConstants.DisplayingProtectorPurpose);
            _context = context;
            _odjeljenjeService = odjeljenjeService;
            _ucenikService = ucenikService;
        }

        public async Task<IActionResult> Index()
        {
            var odjeljenjeViewModel = await BuildOdjeljenjaViewModel();

            return View(odjeljenjeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Novi()
        {
            var model = await BuildNovoOdjeljenjeViewModel();
            return PartialView("_NovoPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novi(NovoOdjeljenjeVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Podaci nisu validni");
            }

            Odjeljenje nizeOdjeljenje = null;

            if (!string.IsNullOrEmpty(model.OdjeljenjeId))
            {
                nizeOdjeljenje = _context.Odjeljenje.Find(int.Parse(_protector.Unprotect(model.OdjeljenjeId)));

                if (nizeOdjeljenje != null && nizeOdjeljenje.Razred >= model.Razred)
                    return BadRequest(
                        "Razred u koji se prebacuje staro odjeljenje mora biti veci od razreda starod odjeljenja!");
            }


            var novoOdjeljenje = new Odjeljenje
            {
                IsPrebacenuViseOdjeljenje = false,
                NastavnikID = int.Parse(_protector.Unprotect(model.RazrednikId)),
                Oznaka = model.Oznaka,
                SkolskaGodina = model.SkolskaGodina,
                Razred = model.Razred
            };
            _context.Add(novoOdjeljenje);

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(model.OdjeljenjeId))
            {
                if (nizeOdjeljenje == null)
                    nizeOdjeljenje = _context.Odjeljenje.Find(int.Parse(_protector.Unprotect(model.OdjeljenjeId)));

                await _odjeljenjeService.PrebaciUViseOdjeljenje(nizeOdjeljenje, novoOdjeljenje);
            }

            return Ok("Uspjesno dodavanje odjeljenja");
        }


        public async Task<IActionResult> Obrisi(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            if (decryptedId <= 0)
                return BadRequest("Invalid Odjeljenje ID");

            await _odjeljenjeService.IzbrisiOdjeljenjeAndStavke(decryptedId);

            return Ok("Uspjesno izbrisano odjeljenje");
        }




        private async Task<OdjeljenjaViewModel> BuildOdjeljenjaViewModel()
        {
            if (!await _context.Odjeljenje.AnyAsync())
                return new OdjeljenjaViewModel{Odjeljenja = new List<OdjeljenjeViewModel>()};

            var odjeljenja = new List<OdjeljenjeViewModel>();

            foreach (var o in await _context.Odjeljenje.Include(x => x.Nastavnik).ToListAsync())
            {

                odjeljenja.Add(new OdjeljenjeViewModel
                {
                    Id = _protector.Protect(o.Id.ToString()),
                    SkolskaGodina = o.SkolskaGodina,
                    Oznaka = o.Oznaka,
                    Razred = o.Razred,
                    Nastavnik = o.Nastavnik.ImePrezime,
                    PrebaceniUViseOdjeljenje = o.IsPrebacenuViseOdjeljenje,
                    ProsjekOcjena = await _odjeljenjeService.GetProsjekOcjena(o.Id),
                    NajboljiUcenik = (await _ucenikService.GetNajboljiUcenik(o.Id))?.ImePrezime ?? ""
                });
            }

            return new OdjeljenjaViewModel{Odjeljenja = odjeljenja};
        }


        private async Task<NovoOdjeljenjeVM> BuildNovoOdjeljenjeViewModel()
        {
            var odjeljenja = _context.Odjeljenje.Where(x => !x.IsPrebacenuViseOdjeljenje);

            var selectListOdjeljenja = odjeljenja.ToSelectList(x => _protector.Protect(x.Id.ToString()),
                x => x.SkolskaGodina + ", " + x.Oznaka, "Bez preuzimanja");

            return new NovoOdjeljenjeVM
            {
                Razred=1,
                OdjeljenjeId = null,
                Odjeljenja =selectListOdjeljenja ,
                Razrednici = _context.Nastavnik
                    .ToSelectList(x =>_protector.Protect(x.NastavnikID.ToString()),x => x.ImePrezime)
            };
        }

 
    }
}