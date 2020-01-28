using eUniverzitet.Web.Helper;
using eUniverzitet.Web.ViewModels;
using Ispit.Data;
using Ispit.Service.Constants;
using Ispit.Service.Interfaces;
using Ispit.Web.Helper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.Controllers
{
    [Autorizacija]
    public class DogadjajController : Controller
    {
        private readonly MyContext _context;
        private readonly IDataProtector _protector;
        private readonly IDogadjajService _dogadjajService;

        public DogadjajController(MyContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants,
            IDogadjajService dogadjajService)
        {
            _protector = protectionProvider.CreateProtector(securityConstants.ProtectorDisplayingPurpose);
            _context = context;
            _dogadjajService = dogadjajService;
        }

        public async Task<IActionResult> DetaljiOznaceni(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            var model = await BuildDogadjajOznaceniDetaljiViewModel(decryptedId);

            return View(model);
        }

        [Route("/Dogadjaji/Obaveza/Stanje/{Id}")]
        public async Task<IActionResult> UrediStanjeObaveze(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));
            var stanjeObaveze = await _context.StanjeObaveze
                .Include(x => x.Obaveza)
                .FirstOrDefaultAsync(x => x.Id == decryptedId);

            if (stanjeObaveze == null)
                return BadRequest("Obaveza nije pronadjena.");

            var model = new StanjeObavezeInputVM
            {
                Id = Id,
                Obaveza = stanjeObaveze.Obaveza.Naziv,
                ZavrsenoProcentualno = stanjeObaveze.IzvrsenoProcentualno,
                NotificirajDanaUnaprijed = stanjeObaveze.NotifikacijaDanaPrije,
                NotificirajRekurzivno = stanjeObaveze.NotifikacijeRekurizivno
            };

            return PartialView("_StanjeObevezeEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Dogadjaji/Obaveza/Stanje")]
        public async Task<IActionResult> EvidentirajStanjeObaveze(StanjeObavezeInputVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            int decryptedId = int.Parse(_protector.Unprotect(model.Id));

            var stanjeObaveze = await _context.StanjeObaveze.FindAsync(decryptedId);

            stanjeObaveze.IzvrsenoProcentualno = model.ZavrsenoProcentualno;
            stanjeObaveze.NotifikacijaDanaPrije = model.NotificirajDanaUnaprijed;
            stanjeObaveze.NotifikacijeRekurizivno = model.NotificirajRekurzivno;

            _context.Update(stanjeObaveze);
            await _context.SaveChangesAsync();

            return Ok(stanjeObaveze.IzvrsenoProcentualno.ToString("F"));
        }

        [Route("/Obaveza/Notifikacija/{Id}")]
        public async Task<IActionResult> OznaciNotifikacijuProcitanom(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));
            if (await _dogadjajService.OznaciNotifikacijuProcitanom(decryptedId))
                return Ok("Notifikacija oznacena kao procitana.");

            return BadRequest("Notifikacija nije pronadjena.");
        }

        private async Task<DogadjajDetaljiVM> BuildDogadjajOznaceniDetaljiViewModel(int dogadjajId)
        {
            var student = HttpContext.GetLogiraniKorisnik();

            var oznaceniDogadjaj = await _context.OznacenDogadjaj
                .Include(x => x.Dogadjaj)
                .ThenInclude(x => x.Nastavnik)
                .FirstOrDefaultAsync(x => x.DogadjajID == dogadjajId);

            if (student == null || oznaceniDogadjaj == null)
                return new DogadjajDetaljiVM { Obaveze = new List<ObavezaVM>() };

            await _context.Entry(oznaceniDogadjaj).Collection(x => x.StanjaObaveza).LoadAsync();

            var model = new DogadjajDetaljiVM
            {
                Id = _protector.Protect(oznaceniDogadjaj.ID.ToString()),
                Opis = oznaceniDogadjaj.Dogadjaj.Opis,
                DatumDodavanja = oznaceniDogadjaj.DatumDodavanja,
                DatumDogadjaja = oznaceniDogadjaj.Dogadjaj.DatumOdrzavanja,
                Nastavnik = oznaceniDogadjaj.Dogadjaj.Nastavnik.ImePrezime,
                Obaveze = new List<ObavezaVM>()
            };

            if (oznaceniDogadjaj.StanjaObaveza.Any())
            {
                model.Obaveze = oznaceniDogadjaj.StanjaObaveza.Select(x => new ObavezaVM
                {
                    Id = _protector.Protect(x.Id.ToString()),
                    Naziv = _context.Obaveza.Find(x.ObavezaID)?.Naziv + x.Id ?? "",
                    ProcenatRealizacije = x.IzvrsenoProcentualno,
                    NotificirajDanaUnapred = x.NotifikacijaDanaPrije,
                    RekurzivnaNotifikacija = x.NotifikacijeRekurizivno
                }).ToList();
            }

            return model;
        }
    }
}