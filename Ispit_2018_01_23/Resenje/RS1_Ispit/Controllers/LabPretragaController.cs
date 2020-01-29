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

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class LabPretragaController : Controller
    {
        private readonly MojContext _context;
        private readonly IDataProtector _protector;
        private readonly IUputnicaService _uputnicaService;

        public LabPretragaController(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants,
            IUputnicaService uputnicaService)
        {
            _context = context;
            _uputnicaService = uputnicaService;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }

        [Route("/Rezultat/Evidencija/{Id}")]
        public async Task<IActionResult> Evidencija(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            var rezultatPretrageFromDb = await _context.RezultatPretrage
                .Include(x => x.LabPretraga)
                .FirstOrDefaultAsync(x => x.Id == decryptedId);

            if (rezultatPretrageFromDb == null)
                return BadRequest("Rezultat pretrage nije pronadjen.");

            var model = await BuildRezultatPretrageInputViewModel(rezultatPretrageFromDb);
            model.Id = Id;

            return PartialView("_EvidentiranjeRezultata",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EvidencijaSnimi(RezultatPretrageInputVM model)
        {
            int decryptedId = int.Parse(_protector.Unprotect(model.Id));

            var rezultatPretrage = await _context.RezultatPretrage.FindAsync(decryptedId);

            if (rezultatPretrage == null)
                return BadRequest("Rezultat pretrage nije pronadjen.");

            if (model.ModalitetId!=null)
            {
                rezultatPretrage.ModalitetId = model.ModalitetId;
            }
            else
            {
                rezultatPretrage.NumerickaVrijednost = model.IzmjerenaVrijednost;
            }

            _context.Update(rezultatPretrage);
            await _context.SaveChangesAsync();

            return Ok(model.ModalitetId!=null
                ? (await _context.FindAsync<Modalitet>(model.ModalitetId))?.Opis??""
                : rezultatPretrage.NumerickaVrijednost?.ToString()??"");

        }



        private async Task<RezultatPretrageInputVM> BuildRezultatPretrageInputViewModel(RezultatPretrage rezultatPretrage)
        {
            if(rezultatPretrage==null)
                return null;

            var modaliteti = new List<SelectListItem>();

            if (rezultatPretrage.LabPretraga.VrstaVr == VrstaVrijednosti.Modalitet)
            {
                modaliteti = _context.Modalitet
                    .Where(x => x.LabPretragaId == rezultatPretrage.LabPretragaId)?
                    .ToSelectList(x => x.Id.ToString(),
                        x => x.Opis, defaultId: (await _context.Modalitet.FindAsync(rezultatPretrage.ModalitetId??0))?.Id??0)
                    ??new List<SelectListItem>();
            }

            return new RezultatPretrageInputVM
            {
                Id = _protector.Protect(rezultatPretrage.Id.ToString()),
                IzmjerenaVrijednost =  rezultatPretrage.NumerickaVrijednost,
                JMJ=rezultatPretrage.LabPretraga.MjernaJedinica,
                ModalitetId = rezultatPretrage.ModalitetId,
                Modaliteti = modaliteti,
                Pretraga = rezultatPretrage.LabPretraga.Naziv,
                VrstaVrijednosti = rezultatPretrage.LabPretraga.VrstaVr
            };
        }
    }
}