using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.Helpers;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ispit_2017_09_11_DotnetCore.Services
{
    public class OdjeljenjeService:IOdjeljenjeService
    {
        private readonly MojContext _context;
        private readonly IUcenikService _ucenikService;

        public OdjeljenjeService(MojContext context, IUcenikService ucenikService)
        {
            _context = context;
            _ucenikService = ucenikService;
        }
        public async Task<double> GetProsjekOcjena(int odjeljenjeId)
        {
            return _context.DodjeljenPredmet
                .Where(d => d.OdjeljenjeStavka.OdjeljenjeId == odjeljenjeId)
                .AverageOrZero(dp => dp.ZakljucnoKrajGodine);
        }

        public async Task PrebaciUViseOdjeljenje(Odjeljenje nizeOdjeljenje, Odjeljenje novoOdjeljenje)
        {
            if (nizeOdjeljenje == null || novoOdjeljenje==null)
                 return;

            nizeOdjeljenje.IsPrebacenuViseOdjeljenje = true;

            _context.Update(nizeOdjeljenje);

            var stavke = new List<OdjeljenjeStavka>();

            foreach(var stavka in _context.OdjeljenjeStavka
                .Where(x => x.OdjeljenjeId == nizeOdjeljenje.Id))
            {
                if (!_ucenikService.IsNegativanOpstiUspjeh(stavka.UcenikId,stavka.OdjeljenjeId))
                {
                    stavke.Add(new OdjeljenjeStavka
                    {
                        BrojUDnevniku = 0,
                        OdjeljenjeId = novoOdjeljenje.Id,
                        UcenikId = stavka.UcenikId
                    });
                }
            }


            if(stavke.Any())
                _context.AddRange(stavke);

            await _context.SaveChangesAsync();
        }

        public async Task IzbrisiOdjeljenjeAndStavke(int odjeljenjeId)
        {
            if (odjeljenjeId <= 0)
                return;

            foreach (var dodjeljeniPredmet in _context.DodjeljenPredmet
                .Where(x => x.OdjeljenjeStavka.OdjeljenjeId == odjeljenjeId))
            {
                _context.Remove(dodjeljeniPredmet);
            }

            foreach (var odjeljenjeStavka in _context.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == odjeljenjeId))
            {
                _context.Remove(odjeljenjeStavka);
            }

            var odjeljenje = _context.Odjeljenje.Find(odjeljenjeId);

            if (odjeljenje != null)
                _context.Remove(odjeljenje);

            await _context.SaveChangesAsync();
        }
    }
}