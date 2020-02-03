using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class SinglePopravniIspitViewComponent : ViewComponent
    {
        private readonly MojContext _context;
        private readonly IPopravniIspitService _popravniIspitService;

        public SinglePopravniIspitViewComponent(MojContext context
            , IPopravniIspitService popravniIspitService)
        {
            _context = context;
            _popravniIspitService = popravniIspitService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int popravniIspitId)
        {
            var model = await BuildPopravniIspitVM(popravniIspitId);

            return View("_SinglePopravniIspit", model);
        }

        private async Task<PopravniIspitVM> BuildPopravniIspitVM(int popravniIspitId)
        {
            var popravniIspit = await _context.PopravniIspiti.FindAsync(popravniIspitId);

            return new PopravniIspitVM
            {
                Id = popravniIspit.Id,
                BrojKojiNisuPolozili = await _popravniIspitService.BrojUcenikaNisuPolozili(popravniIspit.Id),
                BrojUcenika = await _context.PopravniIspitStavke
                    .CountAsync(x => x.PopravniIspitId == popravniIspit.Id),
                Datum = popravniIspit.DatumOdrzavanja,
                Komisija = string.Join(", ", await _context.PopravniIspitKomisija
                                                 .Include(x => x.Nastavnik)
                                                 .Where(x => x.PopravniIspitId == popravniIspit.Id)
                                                 .Select(x => x.Nastavnik.ImePrezime()).ToListAsync() ?? new System.Collections.Generic.List<string>())
            };
        }
    }
}