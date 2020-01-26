using Ispit_2017_09_11_DotnetCore.Constants;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.Helpers;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Ispit_2017_09_11_DotnetCore.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var novoOdjeljenje = new Odjeljenje
            {
                IsPrebacenuViseOdjeljenje = false,
                NastavnikID = int.Parse(_protector.Unprotect(model.RazrednikId)),
                Oznaka = model.Oznaka,
                SkolskaGodina = model.SkolskaGodina,
                Razred = model.Razred
            };

            ServiceResult novoOdjeljenjeResult;

            if (!string.IsNullOrEmpty(model.OdjeljenjeId))
            {
                var nizeOdjeljenje = _context.Odjeljenje.Find(int.Parse(_protector.Unprotect(model.OdjeljenjeId)));
                novoOdjeljenjeResult=await _odjeljenjeService.PrebaciUViseOdjeljenje(novoOdjeljenje, nizeOdjeljenje);

            }
            else
            {
                novoOdjeljenjeResult=await _odjeljenjeService.PrebaciUViseOdjeljenje(novoOdjeljenje);
            }

            if (novoOdjeljenjeResult.Failed)
                return BadRequest(novoOdjeljenjeResult.Message);


            return Ok(novoOdjeljenjeResult.Message);
        }


        public async Task<IActionResult> Obrisi(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            if (decryptedId <= 0)
                return BadRequest("Invalid Odjeljenje ID");

            var brisanjeOdjeljenjaResult = await _odjeljenjeService.ObrisiOdjeljenje(decryptedId);

            if (brisanjeOdjeljenjaResult.Failed)
                return BadRequest(brisanjeOdjeljenjaResult.Message);


            return Ok(brisanjeOdjeljenjaResult.Message);
        }

        public async Task<IActionResult> Detalji(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            if (decryptedId <= 0)
                return BadRequest($"Odjeljenje sa ID-em {decryptedId} ne postoji.");

            var odjeljenejDetaljiVM = await BuildOdjeljenjeDetaljiViewModel(decryptedId);

            return View(odjeljenejDetaljiVM);
        }

        public async Task<IActionResult> RekonstruisiDnevnik(string odjeljenjeId)
        {
            int decryptedOdjeljenjeId = int.Parse(_protector.Unprotect(odjeljenjeId));

            var rekonstrukcijaResult = await _odjeljenjeService.RekonstruisiDnevnik(decryptedOdjeljenjeId);

            if (rekonstrukcijaResult.Failed)
                return BadRequest(rekonstrukcijaResult.Message);

            var model = await BuildOdjeljenjeDetaljiViewModel(decryptedOdjeljenjeId);


            return PartialView("_ListaStavkiOdjeljenje",model.Ucenici);
        }





        //ViewModel Builders

        private async Task<OdjeljenjaViewModel> BuildOdjeljenjaViewModel()
        {
            if (!await _context.Odjeljenje.AnyAsync())
                return new OdjeljenjaViewModel { Odjeljenja = new List<OdjeljenjeViewModel>() };

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

            return new OdjeljenjaViewModel { Odjeljenja = odjeljenja };
        }

        private async Task<NovoOdjeljenjeVM> BuildNovoOdjeljenjeViewModel()
        {
            var odjeljenja = _context.Odjeljenje.Where(x => !x.IsPrebacenuViseOdjeljenje && x.Razred < 4);

            var selectListOdjeljenja = odjeljenja.ToSelectList(x => _protector.Protect(x.Id.ToString()),
                x => x.SkolskaGodina + ", " + x.Oznaka, "Bez preuzimanja");

            return new NovoOdjeljenjeVM
            {
                Razred = 1,
                OdjeljenjeId = null,
                Odjeljenja = selectListOdjeljenja,
                Razrednici = _context.Nastavnik
                    .ToSelectList(x => _protector.Protect(x.NastavnikID.ToString()), x => x.ImePrezime)
            };
        }

        private async Task<OdjeljenjeDetaljiVM> BuildOdjeljenjeDetaljiViewModel(int odjeljenjeId)
        {
            if (odjeljenjeId <= 0)
                return new OdjeljenjeDetaljiVM();

            var odjeljenje = await _context.Odjeljenje.Include(x => x.Nastavnik).FirstOrDefaultAsync();

            if (odjeljenje == null)
                return new OdjeljenjeDetaljiVM();

            var odjeljenjeStavke = _context.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == odjeljenjeId);

            var ucenici = new List<OdjeljenjeStavkaVM>();

            if (odjeljenjeStavke.Any())
            {
                ucenici = await odjeljenjeStavke.Select(x => new OdjeljenjeStavkaVM
                {
                    Id = _protector.Protect(x.Id.ToString()),
                    BrojUDnevniku = x.BrojUDnevniku,
                    Ucenik = x.Ucenik.ImePrezime,
                    BrojZakljucnihKrajGodine = _context.DodjeljenPredmet.Count(dp => dp.OdjeljenjeStavkaId == x.Id && dp.ZakljucnoKrajGodine >= 1)
                }).ToListAsync();
            }

            return new OdjeljenjeDetaljiVM
            {
                Id = _protector.Protect(odjeljenje.Id.ToString()),
                SkolskaGodina = odjeljenje.SkolskaGodina,
                Razred = odjeljenje.Razred,
                Oznaka = odjeljenje.Oznaka,
                Razrednik = odjeljenje.Nastavnik.ImePrezime,
                BrojPredmeta = _context.Predmet.Count(x => x.Razred == odjeljenje.Razred),
                Ucenici = ucenici
            };
        }
    }
}