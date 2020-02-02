using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class SingleRezultatTakmicenjaViewComponent:ViewComponent
    {
        private readonly MojContext _context;
        private readonly ITakmicenjeService _takmicenjeService;

        public SingleRezultatTakmicenjaViewComponent(MojContext context, ITakmicenjeService takmicenjeService)
        {
            _context = context;
            _takmicenjeService = takmicenjeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int takmicenjeUcesnikId)
        {

            var takmicenjeUcesnik = await _context.TakmicenjeUcesnici
                .Include(x => x.Takmicenje)
                .Include(x=>x.OdjeljenjeStavka)
                .ThenInclude(x=>x.Odjeljenje)
                .FirstOrDefaultAsync(x => x.Id == takmicenjeUcesnikId);

            var model = await BuildRezultatTakmicenjaVM(takmicenjeUcesnik);


            ViewData["evidentiraniRez"] = takmicenjeUcesnik.Takmicenje.IsEvidentiraniRezultati;

            return View("_SingleRezultat",model);
        }


        private async Task<RezultatTakmicenjaVM> BuildRezultatTakmicenjaVM(TakmicenjeUcesnik takmicenjeUcesnik)
        {

            if(takmicenjeUcesnik==null)
                return new RezultatTakmicenjaVM();

            return new RezultatTakmicenjaVM
            {
                BrojUDnevniku = takmicenjeUcesnik.OdjeljenjeStavka.BrojUDnevniku,
                Odjeljenje = takmicenjeUcesnik.OdjeljenjeStavka.Odjeljenje.Oznaka,
                Id = takmicenjeUcesnik.Id,
                IsPristupio = takmicenjeUcesnik.IsPristupio,
                OsvojeniBodovi = takmicenjeUcesnik.OsvojeniBodovi
            };
        }
    }
}