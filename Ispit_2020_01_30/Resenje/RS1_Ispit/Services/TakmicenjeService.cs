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
    public class TakmicenjeService: ITakmicenjeService
    {

        private readonly MojContext _context;

        public TakmicenjeService(MojContext context)
        {
            _context = context;
        }


        public async Task<List<Takmicenje>> GetTakmicenja(int skolaId, int razred=0)
        {
            var skola = await _context.Skola.FindAsync(skolaId);

            if (skola == null)
                return null;

            if (razred == 0)
                return await _context.Takmicenja
                    .Include(x => x.Predmet)
                    .Where(x => x.SkolaDomacinId == skolaId).ToListAsync();

            return await _context.Takmicenja
                .Include(x => x.Predmet)
                .Where(x => x.SkolaDomacinId == skolaId
                                                        && x.Razred==razred).ToListAsync();

           
        }

        public string GetNajboljiUcesnikString(int takmicenjeId)
        {
            var takmicenjeUcestvovanja = _context.TakmicenjeUcesnici.Where(x => x.TakmicenjeId == takmicenjeId);

            if (!takmicenjeUcestvovanja.Any())
                return string.Empty;

            var najviseBodova = takmicenjeUcestvovanja.Max(x => x.OsvojeniBodovi);

            var najbolji = takmicenjeUcestvovanja
                .Include(x => x.OdjeljenjeStavka)
                .ThenInclude(x => x.Odjeljenje)
                .ThenInclude(x => x.Skola)
                .FirstOrDefault(x => x.OsvojeniBodovi == najviseBodova);

            string najboljiString = (najbolji?.OdjeljenjeStavka?.Odjeljenje?.Skola.Naziv ?? "") + " | ";
            najboljiString += (najbolji?.OdjeljenjeStavka?.Odjeljenje?.Oznaka ?? "" ) + " | ";
            najboljiString += _context.Ucenik.Find(najbolji?.OdjeljenjeStavka?.UcenikId ?? 0)?.ImePrezime ?? "";

            return najboljiString;
        }

        public async Task<ServiceResult> DodajTakmicenje(Takmicenje takmicenje)
        {
            if(await _context.Takmicenja.AnyAsync(x => x.PredmetId == takmicenje.PredmetId 
                                                       && x.DatumOdrzavanja.Date == takmicenje.DatumOdrzavanja.Date))
                return new ServiceResult
                {
                    Message = "Takmicenje vec dodato.",
                    Success = false
                };

            try
            {
                await _context.AddAsync(takmicenje);
                await _context.SaveChangesAsync();


                var uceniciOcjenaPet = _context.DodjeljenPredmet
                    .Include(x => x.OdjeljenjeStavka)
                    .Where(x => x.PredmetId == takmicenje.PredmetId && x.ZakljucnoKrajGodine == 5);

                var uceniciVeciProsjekOdCetiri = new List<DodjeljenPredmet>();


              

                if (await uceniciOcjenaPet.AnyAsync())
                {
                    foreach (var u in uceniciOcjenaPet)
                    {
                        if (ProsjekUcenika(u.OdjeljenjeStavka.UcenikId, takmicenje.Razred) >= 4)
                            await _context.TakmicenjeUcesnici.AddAsync(new TakmicenjeUcesnik
                            {
                                TakmicenjeId = takmicenje.Id,
                                OdjeljenjeStavkaId = u.OdjeljenjeStavkaId,
                                IsPristupio = false,
                                OsvojeniBodovi = 0
                            });
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Message = ex.Message,
                    Success = false
                };
            }

        return new ServiceResult
            {
                Message="Uspjesno dodato novo takmicenje.",
                Success = true
            };
        }

        public async Task<ServiceResult> DodajUcesnika(TakmicenjeUcesnik ucesnik)
        {
            try
            {

                if(await _context.TakmicenjeUcesnici.AnyAsync(x => x.OdjeljenjeStavkaId == ucesnik.OdjeljenjeStavkaId))
                    return new ServiceResult{
                    Message = "Ucesnik vec postoji.",
                    Success = false
                    };

                await _context.AddAsync(ucesnik);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult{
                Message = ex.Message,
                Success = false
                };
            }

            return new ServiceResult
            {
                Message="Uspjesno dodat ucesnik."
                ,Success = true
            };
        }

        private double ProsjekUcenika(int ucenikId,int razred)
        {
            return _context.DodjeljenPredmet.Where(x => x.Predmet.Razred == razred && x.OdjeljenjeStavka.UcenikId==ucenikId)
                .AverageOrZero(x => x.ZakljucnoKrajGodine);
        }

        //Najbolji učesnik
        //    (Škola | Odjeljenje
        //| Ime i prezime)

    }
}