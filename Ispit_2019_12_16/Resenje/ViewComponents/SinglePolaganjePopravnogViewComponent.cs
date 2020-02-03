using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class SinglePolaganjePopravnogViewComponent:ViewComponent
    {
        private readonly MojContext _context;
        private readonly IPopravniIspitService _popravniIspitService;

        public SinglePolaganjePopravnogViewComponent(MojContext context
            , IPopravniIspitService popravniIspitService)
        {
            _context = context;
            _popravniIspitService = popravniIspitService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int popravniIspitStavkaId)
        {
            var model = await BuildPopravniIspitStavkaVM(popravniIspitStavkaId);

            return View("_SinglePolaganje", model);
        }


        private async Task<PopravniIspitStavkaVM> BuildPopravniIspitStavkaVM(int popravniIspitStavkaId)
        {
            var stavka = await _context.PopravniIspitStavke
                .Include(x=>x.Ucenik)
                .FirstOrDefaultAsync(x=>x.Id==popravniIspitStavkaId);

            var odjeljenjeStavka =
                await _context.OdjeljenjeStavka
                    .Include(x=>x.Odjeljenje)
                    .FirstOrDefaultAsync(x => x.UcenikId == stavka.UcenikId);

            if(stavka==null || odjeljenjeStavka==null)
                return new PopravniIspitStavkaVM();

            return new PopravniIspitStavkaVM
            {
                Id=stavka.Id,
                BrojUDnevniku = odjeljenjeStavka.BrojUDnevniku,
                ImaPravoIzlaska = stavka.ImaPravoNaIzlazask,
                IsPristupio = stavka.IsPrisutupio,
                Odjeljenje = odjeljenjeStavka.Odjeljenje?.Oznaka??"",
                OsvojenoBodova = stavka.OsvojeniBodovi??0,
                Ucenik = stavka.Ucenik.ImePrezime
            };
        }
    }
}