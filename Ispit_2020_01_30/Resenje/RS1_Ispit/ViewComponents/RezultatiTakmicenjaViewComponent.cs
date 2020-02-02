using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class RezultatiTakmicenjaViewComponent :ViewComponent
    {
        private readonly MojContext _context;
        private readonly ITakmicenjeService _takmicenjeService;

        public RezultatiTakmicenjaViewComponent(MojContext context, ITakmicenjeService takmicenjeService)
        {
            _context = context;
            _takmicenjeService = takmicenjeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int takmicenjeId)
        {
            var model = await BuildRezultatiTakmicenjaVM(takmicenjeId);

            return View("Rezultati", model);
        }


        private async Task<RezultatiTakmicenjaVM> BuildRezultatiTakmicenjaVM(int takmicenjeId)
        {
            var takmicenje = await _context.Takmicenja.FindAsync(takmicenjeId);


            var takmicenjeRezultati = _context.TakmicenjeUcesnici
                .Include(x => x.OdjeljenjeStavka)
                .ThenInclude(x=>x.Odjeljenje)
                .Where(x => x.TakmicenjeId == takmicenjeId);

            if (!await takmicenjeRezultati.AnyAsync())
                return new RezultatiTakmicenjaVM
                {
                   Rezultati = new List<RezultatTakmicenjaVM>()

                };

            var rezultati = await takmicenjeRezultati.Select(x => new RezultatTakmicenjaVM
            {
                Id=x.Id,
                Odjeljenje = x.OdjeljenjeStavka.Odjeljenje.Oznaka,
                BrojUDnevniku = x.OdjeljenjeStavka.BrojUDnevniku,
                IsPristupio = x.IsPristupio,
                OsvojeniBodovi = x.OsvojeniBodovi
            }).ToListAsync();


              return new RezultatiTakmicenjaVM{Rezultati = rezultati,IsEvidentiraniRezultati = takmicenje.IsEvidentiraniRezultati};
        }
    }
}
