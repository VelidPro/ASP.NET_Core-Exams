using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class TakmicenjeViewComponent: ViewComponent
    {
        private readonly MojContext _context;
        private readonly ITakmicenjeService _takmicenjeService;

        public TakmicenjeViewComponent(MojContext context, ITakmicenjeService takmicenjeService)
        {
            _context = context;
            _takmicenjeService = takmicenjeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int skolaId, int razred = 0)
        {
            var modelVM = await BuildTakmicenjaVM(skolaId,razred);

            return View("RezultatPretrage", modelVM);
        }



        private async Task<TakmicenjaVM> BuildTakmicenjaVM(int skolaId, int razred)
        {

            var skola = await _context.Skola.FindAsync(skolaId);
            var takmicenja = await _takmicenjeService.GetTakmicenja(skolaId, razred);
            var takmicenjaVM = new List<TakmicenjeVM>();


            if (takmicenja.Any())
            {
                takmicenjaVM = takmicenja.Select(x => new TakmicenjeVM
                {
                    Id = x.Id,
                    Predmet = x.Predmet.Naziv,
                    Razred = x.Razred,
                    DatumOdrzavanja = x.DatumOdrzavanja.Date,
                    BrojUcenikaNisuPristupili = _context.TakmicenjeUcesnici.Count(t => t.IsPristupio == false && t.TakmicenjeId==x.Id),
                    NajboljiUcesnik = _takmicenjeService.GetNajboljiUcesnikString(x.Id)

                }).ToList();
            }

            return new TakmicenjaVM
            {
                SkolaDomacinId = skola?.Id ?? 0,
                Razred = razred,
                SkolaDomacin = skola?.Naziv ?? "",
                Takmicenja = takmicenjaVM
            };

        }
    }
}
