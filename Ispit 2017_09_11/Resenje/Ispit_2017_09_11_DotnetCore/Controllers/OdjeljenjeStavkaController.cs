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
using Microsoft.EntityFrameworkCore;

namespace Ispit_2017_09_11_DotnetCore.Controllers
{
    public class OdjeljenjeStavkaController : Controller
    {
        private readonly MojContext _context;
        private readonly IDataProtector _protector;
        private readonly IOdjeljenjeService _odjeljenjeService;

        public OdjeljenjeStavkaController(IOdjeljenjeService odjeljenjeService,
            IDataProtectionProvider protectionProvider,
            MojContext context,
            SecurityConstants securityConstants)
        {
            _odjeljenjeService = odjeljenjeService;
            _protector = protectionProvider.CreateProtector(securityConstants.DisplayingProtectorPurpose);
            _context = context;
        }


        public IActionResult Uredi(string Id)
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Dodaj(string odjeljenjeId)
        {
            int decryptedId = int.Parse(_protector.Unprotect(odjeljenjeId));

            var vecDodati = await _context.OdjeljenjeStavka
                .Where(x => x.OdjeljenjeId == decryptedId)
                .Select(x => x.UcenikId)
                .ToListAsync();


            var stavkaViewModel = new DodavanjeUcenikOdjeljenjeVM
            {
                OdjeljenjeId = odjeljenjeId,
                Ucenici = _context.Ucenik
                    .Where(x => !vecDodati.Contains(x.Id))
                    .ToSelectList(x => _protector.Protect(x.Id.ToString()), x => x.ImePrezime, "Odaberite ucenika")
            };


            return PartialView("_DodavanjeUcenikaOdjeljenje", stavkaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(DodavanjeUcenikOdjeljenjeVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            int decryptedOdjeljenjeId = int.Parse(_protector.Unprotect(model.OdjeljenjeId));
            int decryptedUcenikId = int.Parse(_protector.Unprotect(model.UcenikId));

            if (decryptedOdjeljenjeId <= 0)
                BadRequest($"Odjeljenje ID nije validan.");

            if (decryptedUcenikId <= 0)
                BadRequest($"Ucenik ID nije validan.");

            var dodavanjeResult =
                await _odjeljenjeService.DodajUcenika(decryptedUcenikId, decryptedOdjeljenjeId, model.BrojUDnevniku);

            if (dodavanjeResult.Failed)
                return BadRequest(dodavanjeResult.Message);

            return Ok(dodavanjeResult.Message);

        }

        public async Task<IActionResult> Obrisi(string stavkaId)
        {
            int decryptedStavkaId = int.Parse(_protector.Unprotect(stavkaId));

            if (decryptedStavkaId <= 0)
                return BadRequest("ID stavke nije validan.");

            var brisanjeResult = await _odjeljenjeService.ObrisiUcenika(decryptedStavkaId);

            if (brisanjeResult.Failed)
                return BadRequest(brisanjeResult.Message);

            return Ok(brisanjeResult.Message);
        }
    }
}