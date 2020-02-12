using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_02_15.EF;
using Ispit_2017_02_15.Interface;
using Ispit_2017_02_15.Models;

namespace Ispit_2017_02_15.Service
{
    public class OdrzaniCasService: IOdrzaniCasService
    {
        private readonly MojContext _dbContext;

        public OdrzaniCasService(MojContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Dodaj(OdrzaniCas cas)
        {
            if (cas == null)
                return false;

            try
            {
                await _dbContext.AddAsync(cas);
                await _dbContext.SaveChangesAsync();

                var slusajuPredemt = _dbContext.SlusaPredmet.Where(x => x.AngazovanId == cas.AngazovanId);

                foreach (var x in slusajuPredemt)
                {
                    await _dbContext.AddAsync(new OdrzaniCasDetalji
                    {
                        OdrzaniCasId = cas.Id,
                        Prisutan = true,
                        SlusaPredmetId = x.Id
                    });
                }

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
