using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class SinglePolaganjeViewComponent:ViewComponent
    {
        private const string NOT_FOUND = "N/A";
        private readonly MojContext _context;
        private readonly IMaturskiIspitService _maturskiIspitService;
        public SinglePolaganjeViewComponent(MojContext context, IMaturskiIspitService maturskiIspitService)
        {
            _context = context;
            _maturskiIspitService = maturskiIspitService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int maturskiIspitStavkaId, int rowNumber)
        {
            var vModel = await BuildMaturskiIspitStavkaVM(maturskiIspitStavkaId);

            ViewData["rowNumber"] = rowNumber;
            return View("_SinglePolaganja", vModel);
        }


        private async Task<MaturskiIspitStavkaVM> BuildMaturskiIspitStavkaVM(int maturskiIspitStavkaId)
        {
            var maturskiIspitStavka = await _context.MaturskiIspitStavke
                .Include(x=>x.Ucenik)
                .FirstOrDefaultAsync(x=>x.Id==maturskiIspitStavkaId);


            return new MaturskiIspitStavkaVM
            {
                Id = maturskiIspitStavka?.Id??0,
                IsPristupio = maturskiIspitStavka?.IsPristupio??false,
                ProsjekOcjena = await _maturskiIspitService.GetProsjekUcenika(maturskiIspitStavka?.UcenikId??0),
                OsvojioBodova = maturskiIspitStavka?.OsvojeniBodovi??0,
                Ucenik = maturskiIspitStavka?.Ucenik?.ImePrezime??NOT_FOUND
            };
        }
    }
}