using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.Constants;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Helpers;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class PolaganjeIspitaController : Controller
    {
        private readonly MojContext _context;
        private readonly IPredmetService _predmetService;
        private readonly IDataProtector _protector;

        public PolaganjeIspitaController(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants, IPredmetService predmetService)
        {
            _context = context;
            _predmetService = predmetService;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }

        public async Task<IActionResult> Dodaj(string ispitniTerminId)
        {
            int decryptedItId = int.Parse(_protector.Unprotect(ispitniTerminId));

            if (!_context.IspitniTermini.Any(x => x.Id == decryptedItId))
                return BadRequest("Ispitni termin nije pronadjen.");

            var model = await BuildNoviIspitPolaganjeViewModel(decryptedItId);

            ViewData["Title"] = "Dodavanje novog polaganja";
            return PartialView("_NovoPolaganje", model);

        }

        public async Task<IActionResult> Uredi(string Id)
        {
            int decryptedPolaganjeId = int.Parse(_protector.Unprotect(Id));

            var polaganje = _context.PolaganjaIspita.Find(decryptedPolaganjeId);

            if (polaganje == null)
                return BadRequest("Polaganje nije pronadjeno.");

            var model = await BuildNoviIspitPolaganjeViewModel(polaganje.IspitniTerminId, polaganje.Id);
            model.Id = Id;

            ViewData["Title"] = "Izmjena polaganja";
            return PartialView("_NovoPolaganje", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Snimi(IspitPolaganjeVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Podaci nisu validni.");

            if (string.IsNullOrEmpty(model.Id))
            {
                var novoPolaganje = new IspitPolaganje
                {
                    IspitniTerminId = int.Parse(_protector.Unprotect(model.IspitniTerminId)),
                    Ocjena = 0,
                    PristupioIspitu = false,
                    UpisGodineId = int.Parse(_protector.Unprotect(model.UpisGodineId))
                };
                await _context.AddAsync(novoPolaganje);
                await _context.SaveChangesAsync();

                return Ok(novoPolaganje.Ocjena);

            }
            else
            {
                var polaganjeFromDb = _context.PolaganjaIspita.Find(int.Parse(_protector.Unprotect(model.Id)));

                if (polaganjeFromDb == null)
                    return BadRequest("Polaganje ne postoji.");

                polaganjeFromDb.Ocjena = model.Ocjena;
                polaganjeFromDb.PristupioIspitu = true;

                _context.Update(polaganjeFromDb);
                await _context.SaveChangesAsync();

                return Ok(polaganjeFromDb.Ocjena);

            }
        }

        [HttpGet]
        public async Task<IActionResult> Ocjena(string polaganjeId, int ocjena)
        {
            if (string.IsNullOrEmpty(polaganjeId) || ocjena < 5 || ocjena > 10)
                return BadRequest("Podaci nisu validni.");

            var polaganje = _context.PolaganjaIspita.Find(int.Parse(_protector.Unprotect(polaganjeId)));

            if (polaganje == null)
                return BadRequest("Polaganje nije pronadjeno.");

            polaganje.Ocjena = ocjena;

            _context.Update(polaganje);

            await _context.SaveChangesAsync();

            return Ok("Uspjesna izmjena ocjene.");
        }


        [HttpGet]
        public async Task<IActionResult> TogglePristupioIspitu(string Id)
        {
            int decryptedId = int.Parse(_protector.Unprotect(Id));

            var polaganje = _context.PolaganjaIspita.Find(decryptedId);

            if (polaganje == null)
                return BadRequest("Polaganje nije pronadjeno.");

            polaganje.PristupioIspitu = !polaganje.PristupioIspitu;

            _context.Update(polaganje);

            await _context.SaveChangesAsync();

            return Ok(polaganje.PristupioIspitu.ToString());
        }





        private async Task<IspitPolaganjeVM> BuildNoviIspitPolaganjeViewModel(int ispitniTerminId,int? polaganjeId = null)
        {
            IspitPolaganjeVM model=new IspitPolaganjeVM();

            if (polaganjeId == null)
            {

                var ispitniTermin = await _context.IspitniTermini
                    .Include(x => x.Angazovan)
                    .FirstOrDefaultAsync(x => x.Id == ispitniTerminId);

                if (ispitniTermin == null)
                    return null;

                var upisi = await _context.UpisGodine.Include(x => x.Student)
                    .Where(x => x.AkademskaGodinaId == ispitniTermin.Angazovan.AkademskaGodinaId)
                    .ToListAsync();

                var polaganjaVecDodata = _context.PolaganjaIspita
                    .Where(x => x.IspitniTerminId == ispitniTerminId);

                if (polaganjaVecDodata.Any())
                {
                    var upisiPostojeciIds = polaganjaVecDodata.Select(x => x.UpisGodine.Id);

                    upisi = upisi.Where(x => !upisiPostojeciIds.Contains(x.Id)).ToList();

                }

                

                model = new IspitPolaganjeVM{
                    IspitniTerminId = _protector.Protect(ispitniTerminId.ToString()),
                    Studenti =  upisi.ToSelectList(x => _protector.Protect(x.Id.ToString()),
                        x =>x.Student.ImePrezime(),"Odaberite studenta")

                };
            }
            else
            {
                var polaganje = await _context.PolaganjaIspita
                    .Include(x => x.UpisGodine)
                    .FirstOrDefaultAsync(x => x.Id == polaganjeId);

                var student = _context.Student.Find(polaganje.UpisGodine.StudentId);

                if (polaganje == null || student==null)
                    return model;

                model=new IspitPolaganjeVM{
                    Id=_protector.Protect(polaganje.Id.ToString()),
                    IspitniTerminId = _protector.Protect(ispitniTerminId.ToString()),
                    Student = student.Ime+" "+student.Prezime
                };
            }

            return model;

        }
    }
}