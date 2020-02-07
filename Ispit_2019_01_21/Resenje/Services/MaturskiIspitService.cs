using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Helpers;
using RS1_Ispit_asp.net_core.HelpModels;
using RS1_Ispit_asp.net_core.Interfaces;

namespace RS1_Ispit_asp.net_core.Services
{
    public class MaturskiIspitService:IMaturskiIspitService
    {
        private readonly MojContext _context;
        public MaturskiIspitService(MojContext context)
        {
            _context = context;
        }

        public async Task<List<Ucenik>> UceniciNisuPristupili(int maturskiIspitId)
        {
            var maturskiIspitStavke = _context.MaturskiIspitStavke
                .Where(x => x.MaturskiIspitId == maturskiIspitId && !x.IsPristupio)
                .Select(x=>x.Ucenik);

            if (!await maturskiIspitStavke.AnyAsync())
                return new List<Ucenik>();

            return await maturskiIspitStavke.ToListAsync();
        }

        public async Task<ServiceResult> DodajNovi(MaturskiIspit maturskiIspit)
        {
            if(maturskiIspit==null)
                return new ServiceResult{Message = "Maturski ispit nije validan.",Success = false};

            if (await _context.MaturskiIspiti.AnyAsync(x => x.PredmetId == maturskiIspit.PredmetId
                                                          && x.DatumOdrzavanja.Date == maturskiIspit.DatumOdrzavanja.Date))
                return new ServiceResult { Message="Maturski ispit vec postoji.",Success=false};


            try
            {
                await _context.AddAsync(maturskiIspit);
                await _context.SaveChangesAsync();


                var uceniciIspunjavajuUslov = _context.DodjeljenPredmet
                    .Where(x => x.OdjeljenjeStavka.Odjeljenje.SkolaID == maturskiIspit.SkolaId
                                && x.OdjeljenjeStavka.Odjeljenje.SkolskaGodinaID == maturskiIspit.SkolskaGodinaId)
                    .Select(x => x.OdjeljenjeStavka.Ucenik)
                    .AsEnumerable()
                    .DistinctBy(x=>x.Id);


                foreach (var x in uceniciIspunjavajuUslov)
                {
                    if (await IspunjavaUslovPolaganja(x.Id))
                        await _context.AddAsync(new MaturskiIspitStavka
                        {
                            IsPristupio = false,
                            MaturskiIspitId = maturskiIspit.Id,
                            OsvojeniBodovi = 0,
                            UcenikId = x.Id
                        });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult{Message = ex.Message,Success = false};
            }

            return new ServiceResult{Message = "Uspjesno dodat maturski ispit.",Success = true};
        }

        public async Task<double> GetProsjekUcenika(int ucenikId)
        {
            return await _context.DodjeljenPredmet
                .Where(x => x.OdjeljenjeStavka.UcenikId == ucenikId)
                .AverageOrZeroAsync(x => x.ZakljucnoKrajGodine);
        }


        public async Task<bool> IspunjavaUslovPolaganja(int ucenikId)
        {
            return !await _context.DodjeljenPredmet
                       .AnyAsync(x => x.OdjeljenjeStavka.UcenikId == ucenikId && x.ZakljucnoKrajGodine == 1)
                   && ((await _context.MaturskiIspitStavke.LastOrDefaultAsync(x =>
                       x.UcenikId == ucenikId))?.OsvojeniBodovi??0)<55;
        }
    }
}