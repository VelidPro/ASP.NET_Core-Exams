using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Helpers;
using RS1_PrakticniDioIspita_2017_01_24.Interfaces;
using RS1_PrakticniDioIspita_2017_01_24.Models;
using RS1_PrakticniDioIspita_2017_01_24.ServiceModels;
using SQLitePCL;

namespace RS1_PrakticniDioIspita_2017_01_24.Services
{
    public class OdrzaniCasService:IOdrzaniCasService
    {
        private readonly MojContext _context;

        public OdrzaniCasService(MojContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> DodajCas(OdrzaniCas cas)
        {
            if(cas==null)
                return new ServiceResult{Message = "Morate proslijediti cas koji zelite dodati.",Success = false};

            try
            {
                await _context.AddAsync(cas);
                await _context.SaveChangesAsync();

                if (cas.Angazovan == null)
                    cas.Angazovan = await _context.Angazovani.FindAsync(cas.AngazovanId);

                foreach (var x in _context.UpisiUOdjeljenja
                    .Where(x => x.OdjeljenjeId == cas.Angazovan.OdjeljenjeId))
                {
                    await _context.AddAsync(new OdrzaniCasDetalj
                    {
                        OdrzaniCasId = cas.Id,
                        Odsutan = false,
                        UpisUOdjeljenjeId = x.Id
                    });
                }

                await _context.SaveChangesAsync();
                return new ServiceResult { Message = "Uspjesno dodat novi odrzani cas.", Success = true };


            }
            catch (Exception ex)
            {
                return new ServiceResult{Message = ex.Message,Success = false};
            }

            return new ServiceResult { Message = "Neuspjesno dodavanje odrzanog casa.",Success = false};
        }

        public async Task<int> BrojPrisutnih(int odrzaniCasId)
        {
            return await _context.OdrzaniCasDetalji.CountAsync(x => x.OdrzaniCasId == odrzaniCasId);
        }

        public async Task<Ucenik> NajboljiUcenik(int predmetId, int odjeljenjeId)
        {
            var uceniciProsjeci= new Dictionary<int,double>();

            var odrzaniCasoviDetalji = _context.OdrzaniCasDetalji
                .Include(x=>x.UpisUOdjeljenje)
                .Where(x => x.OdrzaniCas.Angazovan.PredmetId == predmetId 
                            && x.OdrzaniCas.Angazovan.OdjeljenjeId==odjeljenjeId);

            if (!await odrzaniCasoviDetalji.AnyAsync())
                return null;

            var upisiUOdjeljenjeIds =  odrzaniCasoviDetalji.Select(x => x.UpisUOdjeljenjeId).AsEnumerable();

            upisiUOdjeljenjeIds = upisiUOdjeljenjeIds.DistinctBy(x=>x);


            foreach (var x in upisiUOdjeljenjeIds)
            {
                uceniciProsjeci.Add(x,odrzaniCasoviDetalji.Where(u=>u.UpisUOdjeljenjeId==x).AverageOrZero(z=>z.Ocjena??0));
            }

            var najboljiUpisUOdjeljenjeId = uceniciProsjeci
                .Aggregate((x,y)=>x.Value>y.Value?x:y).Key;

            var najboljiUcenik = (await _context.UpisiUOdjeljenja
                .Include(x => x.Ucenik)
                .FirstOrDefaultAsync(x => x.Id == najboljiUpisUOdjeljenjeId))?.Ucenik;

            return najboljiUcenik;
        }

        public async Task<List<SelectListItem>> GetOdjeljenjaPredmeti(int nastavnikId,int? angazovanId=null)
        {
            var odjeljenjaPredmeti = _context.Angazovani
                .Include(x => x.Predmet)
                .Include(x => x.Odjeljenje)
                .Where(x => x.NastavnikId == nastavnikId);

            var odjeljenjaPredmetiVM = new List<SelectListItem>();

            if (await odjeljenjaPredmeti.AnyAsync())
            {
                odjeljenjaPredmetiVM = odjeljenjaPredmeti
                    .ToSelectList(x => x.Id.ToString(),
                        x => (x.Odjeljenje?.Oznaka ?? string.Empty) + " / " + (x.Predmet?.Naziv ?? string.Empty),
                        "Odaberite odjeljenje", angazovanId??0);
            }

            return odjeljenjaPredmetiVM;
        }
    }
}
