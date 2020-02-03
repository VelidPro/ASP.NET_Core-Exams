using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Helpers;
using RS1_Ispit_asp.net_core.HelpModels;
using RS1_Ispit_asp.net_core.Interfaces;

namespace RS1_Ispit_asp.net_core.Services
{
    public class OdrzanaNastavaService:IOdrzanaNastavaService
    {
        private readonly MojContext _context;

        public OdrzanaNastavaService(MojContext context)
        {
            _context = context;
        }

        public async Task<List<Nastavnik>> GetNastavnikePredavace()
        {

            return await _context.PredajePredmet.AnyAsync()
                ?  _context.PredajePredmet
                    .Include(x=>x.Nastavnik)
                    .ThenInclude(x=>x.Skola)
                    .AsEnumerable()
                    .Select(x=>x.Nastavnik)
                    .DistinctBy(x=>x.Id)
                    .ToList()
                : new List<Nastavnik>();
        }

        public async Task<List<string>> GetOdsutniUceniciString(int odrzaniCasId)
        {
            var odrzaniCasoviStavke = _context.OdrzaniCasStavke
                .Include(x=>x.OdjeljenjeStavka)
                .ThenInclude(x=>x.Ucenik)
                .Where(x => x.Id == odrzaniCasId && !x.IsPrisutan);

            if(!await odrzaniCasoviStavke.AnyAsync())
                return new List<string>();

            return  await odrzaniCasoviStavke.Select(x => x.OdjeljenjeStavka.Ucenik.ImePrezime).ToListAsync();
        }

        public async Task<List<PredajePredmet>> GetPredmetePredaje(int nastavnikId)
        {
            var predmeti = _context.PredajePredmet
                .Include(x=>x.Predmet)
                .Include(x=>x.Odjeljenje)
                .Where(x => x.NastavnikID == nastavnikId);

            if (!await predmeti.AnyAsync())
                return new List<PredajePredmet>();

            return await predmeti.ToListAsync();
        }

        public async Task<ServiceResult> DodajOdrzaniCas(OdrzaniCas cas)
        {
            if(await _context.OdrzaniCasovi.AnyAsync(x=>x.PredajePredmetId == cas.PredajePredmetId &&
                                                        x.Datum == cas.Datum))
                return new ServiceResult
                {
                    Message = "Isti cas vec postoji.",
                    Success = false
                };

            try
            {
                await _context.AddAsync(cas);
                await _context.SaveChangesAsync();

                var uceniciOdjeljenje = _context.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == cas.OdjeljenjeId);

                if (await uceniciOdjeljenje.AnyAsync())
                {
                    foreach (var u in uceniciOdjeljenje)
                    {
                        await _context.AddAsync(new OdrzaniCasStavka
                        {
                            IsPrisutan = false,
                            Napomena = string.Empty,
                            Ocjena = 0,
                            OdjeljenjeStavkaId = u.Id,
                            OdrzaniCasId = cas.Id,
                            OpravdanoOdsustvo = false
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
                Message = "Uspjesno dodat cas.",
                Success = true
            };

        }

        public async Task<ServiceResult> ObrisiOdrzaniCas(int odrzaniCasId)
        {
            var odrzaniCas = await _context.OdrzaniCasovi.FindAsync(odrzaniCasId);

            if(odrzaniCas==null)
                return new ServiceResult
                {
                    Message = "Odrzani cas nije pronadjen.",
                    Success = false
                };

            try
            {
                foreach (var x in _context.OdrzaniCasStavke.Where(oc => oc.OdrzaniCasId == odrzaniCasId))
                {
                    _context.Remove(x);
                }

                _context.Remove(odrzaniCas);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new ServiceResult
                {
                    Message=ex.Message,
                    Success = false
                };
            }

            return new ServiceResult
            {
                Message = "Uspjesno obrisan odrzani cas.",
                Success = true
            };

        }
    }
}