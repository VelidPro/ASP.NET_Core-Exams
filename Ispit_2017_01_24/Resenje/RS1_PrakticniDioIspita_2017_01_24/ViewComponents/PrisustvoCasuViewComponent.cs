using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Interfaces;
using RS1_PrakticniDioIspita_2017_01_24.ViewModels;

namespace RS1_PrakticniDioIspita_2017_01_24.ViewComponents
{
    public class PrisustvoCasuViewComponent:ViewComponent
    {
        private readonly MojContext _context;
        private readonly IOdrzaniCasService _odrzaniCasService;

        public PrisustvoCasuViewComponent(MojContext context, IOdrzaniCasService odrzaniCasService)
        {
            _context = context;
            _odrzaniCasService = odrzaniCasService;
        }


        public async Task<IViewComponentResult> InvokeAsync(int odrzaniCasDetaljiId)
        {
            var vModel = await BuildOdrzaniCasDetaljVM(odrzaniCasDetaljiId);

            return View("_Prisustvo", vModel);
        }


        public async Task<OdrzaniCasDetaljEditVM> BuildOdrzaniCasDetaljVM(int odrzaniCasDetaljiId)
        {
            var prisustvo = await _context.OdrzaniCasDetalji
                .Include(x => x.UpisUOdjeljenje)
                .ThenInclude(x => x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == odrzaniCasDetaljiId);

            var prisustvoVM = new OdrzaniCasDetaljEditVM();

            if (prisustvo!=null)
            {
                prisustvoVM = new OdrzaniCasDetaljEditVM
                {
                    Id = prisustvo.Id,
                    Odsutan = prisustvo.Odsutan,
                    OpravdanoOdsutan = prisustvo.OpravdanoOdsutan,
                    Ocjena = prisustvo.Ocjena,
                    Ucenik = prisustvo.UpisUOdjeljenje.Ucenik.Ime
                };
            }

            return prisustvoVM;
        }
    }
}