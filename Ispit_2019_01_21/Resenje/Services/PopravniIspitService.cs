using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.HelpModels;
using RS1_Ispit_asp.net_core.Interfaces;

namespace RS1_Ispit_asp.net_core.Services
{
    public class PopravniIspitService:IPopravniIspitService
    {
        private readonly MojContext _context;

        public PopravniIspitService(MojContext context )
        {
            _context = context;
        }

        public async Task<int> BrojUcenikaNisuPolozili(int popravniIspitId)
        {

            if (!await _context.PopravniIspiti.AnyAsync(x=>x.Id==popravniIspitId))
                return 0;

            return await _context.PopravniIspitStavke.CountAsync(
                x => x.PopravniIspitId == popravniIspitId && x.IsPrisutupio && x.OsvojeniBodovi < 50);
        }

        public async Task<int> BrojUcenikaPolozili(int popravniIspitId)
        {

            if (!await _context.PopravniIspiti.AnyAsync(x => x.Id == popravniIspitId))
                return 0;

            return (await _context.PopravniIspitStavke.CountAsync(
                x => x.PopravniIspitId == popravniIspitId && x.IsPrisutupio && x.OsvojeniBodovi>=50));
        }

        public async Task<ServiceResult> Dodaj(PopravniIspit popravniIspit,List<int> komisijaIds)
        {

            try
            {
                if(!komisijaIds.Any())
                    return new ServiceResult
                    {
                        Message = "Podaci nisu validni.",
                        Success = false
                    };

                if(_context.PopravniIspiti.Any(x => x.PredmetId == popravniIspit.PredmetId
                                                    && x.DatumOdrzavanja.Date == popravniIspit.DatumOdrzavanja.Date))
                    return new ServiceResult
                    {
                        Message = "Popravni ispit vec postoji.",
                        Success = false
                    };


                await _context.AddAsync(popravniIspit);
                await _context.SaveChangesAsync();

                var uceniciNegativnoOcjenjeni = _context.DodjeljenPredmet
                    .Where(x => x.PredmetId == popravniIspit.PredmetId && x.ZakljucnoKrajGodine == 1)
                    .Select(x => x.OdjeljenjeStavka.Ucenik);

                foreach(var k in komisijaIds)
                {
                    await _context.AddAsync(new PopravniIspitKomisija
                    {
                        PopravniIspitId = popravniIspit.Id,
                        NastavnikId = k
                    });
                }

                foreach (var u in uceniciNegativnoOcjenjeni)
                {
                    var novaStavkaPopravni = new PopravniIspitStavka
                    {
                        ImaPravoNaIzlazask = true,
                        IsPrisutupio = false,
                        PopravniIspitId = popravniIspit.Id,
                        UcenikId = u.Id
                    };


                    if (await ImaTriNegativne(u.Id))
                    {
                        novaStavkaPopravni.ImaPravoNaIzlazask = false;
                    }

                    await _context.AddAsync(novaStavkaPopravni);

                }
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _context.Remove(popravniIspit);
                return new ServiceResult
                {
                    Message = ex.Message,
                    Success = false
                };
            }
           

            return new ServiceResult
            {
                Message = "Uspjesno dodat popravni ispit.",
                Success=true
            };

        }


        private async Task<bool> ImaTriNegativne(int ucenikId)
        {
            return await _context.DodjeljenPredmet
                       .CountAsync(x => x.ZakljucnoKrajGodine == 1 
                                        && x.OdjeljenjeStavka.UcenikId == ucenikId) > 3;
        }
    }
}
