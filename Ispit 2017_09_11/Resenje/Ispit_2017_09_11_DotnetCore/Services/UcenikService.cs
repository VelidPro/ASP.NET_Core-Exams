using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Linq;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.Helpers;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace Ispit_2017_09_11_DotnetCore.Services
{
    public class UcenikService:IUcenikService
    {
        private readonly MojContext _context;
        public UcenikService(MojContext context)
        {
            _context = context;
        }
        public async Task<Ucenik> GetNajboljiUcenik(int odjeljenjeId)
        {
            if (!_context.Ucenik.Any() || !_context.OdjeljenjeStavka.Any() || !_context.DodjeljenPredmet.Any())
                return null;

            var uceniciProsjeci = new Dictionary<int, double>();

            var ucenici = await _context.Ucenik.ToListAsync();

            foreach (var ucenik in ucenici)
            {
                var dodjeljeniPredmeti = _context.DodjeljenPredmet
                    .Where(x => x.OdjeljenjeStavka.UcenikId == ucenik.Id);

                double prosjek = 0;
                if(dodjeljeniPredmeti.Any())
                    prosjek = dodjeljeniPredmeti.Average(x => x.ZakljucnoKrajGodine);

                uceniciProsjeci.Add(ucenik.Id, prosjek);
            }

            double najvecaProsjecna = uceniciProsjeci.Max(x => x.Value);

            var najboljiUcenikId = uceniciProsjeci.First(up => up.Value == najvecaProsjecna).Key;


            return ucenici.FirstOrDefault(x => x.Id == najboljiUcenikId);
        }

        public double GetProsjekUcenika(int ucenikId, int odjeljenjeId)
        {
            return _context.DodjeljenPredmet
                .Where(x => x.OdjeljenjeStavka.OdjeljenjeId == odjeljenjeId && x.OdjeljenjeStavka.UcenikId == ucenikId)
                .AverageOrZero(x => x.ZakljucnoKrajGodine);
        }

        public bool IsNegativanOpstiUspjeh(int ucenikId, int odjeljenjeId)
        {
            if (ucenikId <= 0 || odjeljenjeId <= 0)
                return false;

            var dodjeljeniPredmetiUcenika = _context.DodjeljenPredmet
                .Where(x => x.OdjeljenjeStavka.UcenikId == ucenikId && x.OdjeljenjeStavka.OdjeljenjeId == odjeljenjeId);

            if (!dodjeljeniPredmetiUcenika.Any())
                return false;

            return dodjeljeniPredmetiUcenika.Count(x => x.ZakljucnoKrajGodine == 1) >= 1;
        }
    }
}