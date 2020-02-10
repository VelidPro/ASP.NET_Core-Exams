using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS1_Ispit_2017_06_21_v1.EF;
using RS1_Ispit_2017_06_21_v1.Helpers;
using RS1_Ispit_2017_06_21_v1.HelpModels;
using RS1_Ispit_2017_06_21_v1.Interfaces;
using RS1_Ispit_2017_06_21_v1.Models;

namespace RS1_Ispit_2017_06_21_v1.Services
{
    public class MaturskiIspitService:IMaturskiIspitService
    {
        private readonly MojContext _dbContext;

        public MaturskiIspitService(MojContext context)
        {
            _dbContext = context;
        }

        public double GetProsjekBodova(int maturskiIspitId)
        {
            return _dbContext.MaturskiIspitStavke
                .Where(x => x.MaturskiIspitId == maturskiIspitId && x.Bodovi.HasValue)
                .AsEnumerable()
                .AverageOrZero(x => x.Bodovi.Value);
        }

        public async Task<ServiceResult> Dodaj(MaturskiIspit ispit)
        {
            if (ispit == null)
                return new ServiceResult{Success = false,Message = string.Empty};
            try
            {
                await _dbContext.AddAsync(ispit);
                await _dbContext.SaveChangesAsync();

                var uceniciZaDodavanje = _dbContext.UpisUOdjeljenje
                    .Where(x => x.OdjeljenjeId == ispit.OdjeljenjeId && x.OpciUspjeh > 1);

                foreach (var x in uceniciZaDodavanje)
                {
                    await _dbContext.AddAsync(new MaturskiIspitStavka
                    {
                        Bodovi = null,
                        MaturskiIspitId = ispit.Id,
                        Oslobodjen = x.OpciUspjeh == 5,
                        UpisUOdjeljenjeId = x.Id
                    });
                }

                await _dbContext.SaveChangesAsync();

                return new ServiceResult
                {
                    Success = true,
                    Message = "Uspjesno dodat maturski ispit."
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

    }
}
