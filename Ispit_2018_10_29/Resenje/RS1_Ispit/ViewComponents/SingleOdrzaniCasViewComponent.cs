using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class SingleOdrzaniCasViewComponent:ViewComponent
    {
        private readonly MojContext _context;
        private readonly IOdrzanaNastavaService _nastavaService;

        public SingleOdrzaniCasViewComponent(MojContext context,
            IOdrzanaNastavaService nastavaService)
        {
            _context = context;
            _nastavaService = nastavaService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int odrzaniCasId)
        {
            var model = await BuildOdrzaniCasVM(odrzaniCasId);

            return View("_OdrzaniCas", model);
        }


        private async Task<OdrzaniCasVM> BuildOdrzaniCasVM(int odrzaniCasId)
        {
            var odrzaniCas = await _context.OdrzaniCasovi
                .Include(x=>x.Odjeljenje)
                .ThenInclude(x=>x.SkolskaGodina)
                .Include(x=>x.PredajePredmet)
                .ThenInclude(x=>x.Predmet)
                .FirstOrDefaultAsync(x=>x.Id==odrzaniCasId);

            if(odrzaniCas==null)
                return new OdrzaniCasVM();

            return new OdrzaniCasVM
            {
                Datum=odrzaniCas.Datum,
                Id = odrzaniCas.Id,
                OdsutniUcenici = await _nastavaService.GetOdsutniUceniciString(odrzaniCasId),
                Predmet=odrzaniCas.PredajePredmet.Predmet.Naziv,
                SkGodinaOdjeljenje = string.Concat(odrzaniCas.Odjeljenje.SkolskaGodina.Naziv, " ", odrzaniCas.Odjeljenje.Oznaka)
            };
        }
    }
}