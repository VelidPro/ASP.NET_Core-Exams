using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Helpers;
using RS1_PrakticniDioIspita_2017_01_24.Interfaces;
using RS1_PrakticniDioIspita_2017_01_24.Models;
using RS1_PrakticniDioIspita_2017_01_24.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_PrakticniDioIspita_2017_01_24.Controllers
{
    public class OdrzaniCasDetaljiController : Controller
    {
        private readonly MojContext _context;
        private readonly IOdrzaniCasService _odrzaniCasService;

        public OdrzaniCasDetaljiController(MojContext context, IOdrzaniCasService odrzaniCasService)
        {
            _context = context;
            _odrzaniCasService = odrzaniCasService;
        }


        public async Task<IActionResult> PrisustvoToggler(int Id)
        {
            var prisustvo = await _context.OdrzaniCasDetalji.FindAsync(Id);

            if (prisustvo == null)
                return NotFound("Prisustvo nije pronadjeno.");

            prisustvo.Odsutan = !prisustvo.Odsutan;
            _context.Update(prisustvo);
            await _context.SaveChangesAsync();

            return ViewComponent("PrisustvoCasu", new { odrzaniCasDetaljiId = Id});
        }


        public async Task<IActionResult> Edit(int Id)
        {
            var prisustvo = await _context.OdrzaniCasDetalji
                .Include(x => x.UpisUOdjeljenje)
                .ThenInclude(x => x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (prisustvo == null)
                return NotFound("Prisustvo nije pronadjeno.");

            var vModel = await BuildPrisustvoInputVM(prisustvo);

            return PartialView("_Edit",vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrisustvoInputVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new {Errors = ModelState.Values.SelectMany(x => x.Errors)});

            var prisustvo = await _context.OdrzaniCasDetalji.FindAsync(model.Id);

            if (prisustvo == null)
                return NotFound("Prisustvo nije pronadjeno.");

            if (prisustvo.Odsutan)
            {
                prisustvo.OpravdanoOdsutan = model.OpravdanoOdsutan;
            }
            else
            {
                if (model.Ocjena.HasValue)
                    prisustvo.Ocjena = model.Ocjena.Value;
            }

            _context.Update(prisustvo);
            await _context.SaveChangesAsync();

            return ViewComponent("PrisustvoCasu", new {odrzaniCasDetaljiId = model.Id});
        }

        public async Task<PrisustvoInputVM> BuildPrisustvoInputVM(OdrzaniCasDetalj prisustvo)
        {
            if (prisustvo == null)
                return null;

            return new PrisustvoInputVM
            {
                Id=prisustvo.Id,
                Ocjena = prisustvo.Ocjena,
                OpravdanoOdsutan = prisustvo.OpravdanoOdsutan??false,
                Odsutan = prisustvo.Odsutan,
                Ucenik = prisustvo.UpisUOdjeljenje.Ucenik.Ime+" ("+prisustvo.UpisUOdjeljenje.BrojUDnevniku+")"
            };
        }
    }
}