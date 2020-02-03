using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class SinglePrisustvoViewComponent: ViewComponent
    {
        private readonly MojContext _context;
        private readonly IOdrzanaNastavaService _nastavaService;

        public SinglePrisustvoViewComponent(MojContext context,
            IOdrzanaNastavaService nastavaService)
        {
            _context = context;
            _nastavaService = nastavaService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int odrzaniCasStavkaId)
        {
            var model = await BuildOdrzaniCasStavkaVM(odrzaniCasStavkaId);

            return View("_SinglePrisustvoCasu", model);
        }


        private async Task<OdrzaniCasStavkaVM> BuildOdrzaniCasStavkaVM(int odrzaniCasStavkaId)
        {
            var odrzaniCasStavka = await _context.OdrzaniCasStavke
                .Include(x=>x.OdjeljenjeStavka)
                .ThenInclude(x=>x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == odrzaniCasStavkaId);

            if (odrzaniCasStavka == null)
                return new OdrzaniCasStavkaVM();

            return new OdrzaniCasStavkaVM
            {
                Id = odrzaniCasStavka.Id,
                IsPrisutan = odrzaniCasStavka.IsPrisutan,
                Ocjena = odrzaniCasStavka.Ocjena,
                OpravdanoOdsutan = odrzaniCasStavka.OpravdanoOdsustvo,
                Ucenik = odrzaniCasStavka.OdjeljenjeStavka.Ucenik.ImePrezime
            };
        }
    }
}