using eUniverzitet.Web.Helper;
using eUniverzitet.Web.ViewModels;
using Ispit.Data;
using Ispit.Web.Helper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit.Service.Constants;
using Ispit.Service.Interfaces;

namespace Ispit.Web.Controllers
{
    [Autorizacija]
    public class OznaceniDogadajiController : Controller
    {
        private readonly MyContext _context;
        private readonly IDataProtector _protector;
        private readonly IDogadjajService _dogadjajService;

        public OznaceniDogadajiController(MyContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants, 
            IDogadjajService dogadjajService)
        {
            _protector = protectionProvider.CreateProtector(securityConstants.ProtectorDisplayingPurpose);
            _context = context;
            _dogadjajService = dogadjajService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await BuildDogadjajiViewModel();
            return View(model);
        }

        public async Task<IActionResult> Dodaj(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));
            var student = HttpContext.GetLogiraniKorisnik();

            if (student == null)
                return BadRequest("Neautorizovan pristup.");

            var oznacenResult = await _dogadjajService.OznaciDogadjaj(decryptedId, student.Id);

            if (oznacenResult.Success)
            {
                var listaOznacenih = await _dogadjajService.GetOznaceneDogadjaje(student.Id);
                var model = new List<DogadjajVM>();

                if (listaOznacenih.Any())
                {
                    model = listaOznacenih.Select(x => new DogadjajVM
                    {
                        DogadjajId = _protector.Protect(x.DogadjajID.ToString()),
                        OznaceniDogadjajId = _protector.Protect(x.DogadjajID.ToString()),
                        Opis = x.Dogadjaj.Opis,
                        DatumDogadjaja = x.Dogadjaj.DatumOdrzavanja,
                        Nastavnik = x.Dogadjaj.Nastavnik.ImePrezime,
                        RealizovanoObaveza = _dogadjajService.GetProcenatRealizovanihObaveza(x.DogadjajID)
                    }).ToList();
                }
                return PartialView("_TabelaOznacenih", model);
            }
            return BadRequest(oznacenResult.Message);
        }





        private async Task<DogadjajiViewModel> BuildDogadjajiViewModel()
        {
            var student = HttpContext.GetLogiraniKorisnik();

            var dogadjaji = await _dogadjajService.GetDogadjaje();

            if (student == null || !dogadjaji.Any())
                return new DogadjajiViewModel
                    {NeoznaceniDogadjaji = new List<DogadjajVM>(), OznaceniDogadjaji = new List<DogadjajVM>()};


          

            var oznaceniDogadjaji = await _dogadjajService.GetOznaceneDogadjaje(student.Id);

            var oznaceniDogadjajiVM = new List<DogadjajVM>();

            if (oznaceniDogadjaji.Any())
            {
                var oznaceniDogadjajiIds = oznaceniDogadjaji.Select(x => x.DogadjajID);

                dogadjaji = dogadjaji.Where(x => !oznaceniDogadjajiIds.Contains(x.ID)).ToList();

                oznaceniDogadjajiVM = oznaceniDogadjaji.Select(x => new DogadjajVM
                {
                    DogadjajId = _protector.Protect(x.DogadjajID.ToString()),
                    OznaceniDogadjajId = _protector.Protect(x.DogadjajID.ToString()),
                    Opis = x.Dogadjaj.Opis,
                    DatumDogadjaja = x.Dogadjaj.DatumOdrzavanja,
                    Nastavnik = x.Dogadjaj.Nastavnik.ImePrezime,
                    RealizovanoObaveza =  _dogadjajService.GetProcenatRealizovanihObaveza(x.DogadjajID)


                }).ToList();
            }

            var neoznaceniDogadjaji = dogadjaji.Select(x => new DogadjajVM
            {
                DogadjajId = _protector.Protect(x.ID.ToString()),
                Opis = x.Opis,
                DatumDogadjaja = x.DatumOdrzavanja,
                Nastavnik = x.Nastavnik.ImePrezime,
                BrojObaveza = _context.Obaveza.Count(o => o.DogadjajID == x.ID)
            });

            return new DogadjajiViewModel
            {
                Notifikacije =await _dogadjajService.ListaDanasnjihNotifikacija(HttpContext.GetLogiraniKorisnik()?.Id ?? 0),
                OznaceniDogadjaji = oznaceniDogadjajiVM.ToList(),
                NeoznaceniDogadjaji = neoznaceniDogadjaji.ToList()
            };

        }
    }
}