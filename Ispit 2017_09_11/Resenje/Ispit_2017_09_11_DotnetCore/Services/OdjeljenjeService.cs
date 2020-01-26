using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.Helpers;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

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


        public async Task<ServiceResult> ObrisiOdjeljenje(int odjeljenjeId)
        {
            var odjeljenje = _context.Odjeljenje.Find(odjeljenjeId);

            if(odjeljenje==null)
                return new ServiceResult{
                    Message = "Odjeljenje nije pronadjeno",
                    Failed = true
                };

            foreach(var dodjeljeniPredmet in _context.DodjeljenPredmet
                .Where(x => x.OdjeljenjeStavka.OdjeljenjeId == odjeljenjeId))
            {
                _context.Remove(dodjeljeniPredmet);
            }

            foreach (var odjeljenjeStavka in _context.OdjeljenjeStavka
                .Where(x => x.OdjeljenjeId == odjeljenjeId))
            {
                _context.Remove(odjeljenjeStavka);
            }

            _context.Remove(odjeljenje);

            await _context.SaveChangesAsync();

            return new ServiceResult{
                Message = "Uspjesno brisanje odjeljenja",
                Success = true
            };


        }
        public async Task<double> GetProsjekOcjena(int odjeljenjeId)
        {
            return _context.DodjeljenPredmet
                .Where(d => d.OdjeljenjeStavka.OdjeljenjeId == odjeljenjeId)
                .AverageOrZero(dp => dp.ZakljucnoKrajGodine);
        }

        public async Task<ServiceResult> PrebaciUViseOdjeljenje(Odjeljenje novoOdjeljenje,Odjeljenje nizeOdjeljenje=null)
        {
            if (novoOdjeljenje == null)
                return new ServiceResult
                {
                    Message = "Greska",
                    Failed = true
                };

            if(_context.Odjeljenje.Any(x => x.Oznaka == novoOdjeljenje.Oznaka))
                return new ServiceResult
                {
                    Message = "Odjeljenje sa istom oznakom vec postoji.",
                    Failed = true
                };

            



            if (nizeOdjeljenje != null)
            {
                if (nizeOdjeljenje.Razred >= novoOdjeljenje.Razred)
                    return new ServiceResult
                    {
                        Message = "Razred u koji se prebacuje staro odjeljenje mora biti veci od trenutnog.",
                        Failed = true
                    };
                
                if(!IsValidnaNarednaSkolskaGodina(nizeOdjeljenje.SkolskaGodina,novoOdjeljenje.SkolskaGodina))
                    return new ServiceResult
                    {
                        Message = "Skolska godina mora biti za jednu godinu veca od stare skolske godine.",
                        Failed = true
                    };

                nizeOdjeljenje.IsPrebacenuViseOdjeljenje = true;

                _context.Update(nizeOdjeljenje);




                var stavke = new List<OdjeljenjeStavka>();
                var dodjeljeniPredmetiNovi = new List<DodjeljenPredmet>();

                await _context.AddAsync(novoOdjeljenje);

                await _context.SaveChangesAsync();

                foreach (var stavka in _context.OdjeljenjeStavka
                    .Where(x => x.OdjeljenjeId == nizeOdjeljenje.Id))
                {
                    if (!_ucenikService.IsNegativanOpstiUspjeh(stavka.UcenikId, stavka.OdjeljenjeId))
                    {
                        stavke.Add(new OdjeljenjeStavka
                        {
                            BrojUDnevniku = 0,
                            OdjeljenjeId = novoOdjeljenje.Id,
                            UcenikId = stavka.UcenikId
                        });
                        await _context.AddAsync(stavke.Last());
                        await _context.SaveChangesAsync();

                        foreach (var predmet in _context.Predmet.Where(x => x.Razred == novoOdjeljenje.Razred))
                        {
                            dodjeljeniPredmetiNovi.Add(new DodjeljenPredmet
                            {
                                OdjeljenjeStavkaId = stavke.Last().Id,
                                PredmetId = predmet.Id,
                                ZakljucnoKrajGodine = 0,
                                ZakljucnoPolugodiste = 0
                            });

                        }
                    }
                }

                if (dodjeljeniPredmetiNovi.Any())
                    await _context.AddRangeAsync(dodjeljeniPredmetiNovi);

                await _context.SaveChangesAsync();

            }

            return new ServiceResult
            {
                Message = "Uspjesno kreirano novo odjeljenje.",
                Success = true
            };
        }

        //public async Task IzbrisiOdjeljenjeAndStavke(int odjeljenjeId)
        //{
        //    if (odjeljenjeId <= 0)
        //        return;

        //    foreach (var dodjeljeniPredmet in _context.DodjeljenPredmet
        //        .Where(x => x.OdjeljenjeStavka.OdjeljenjeId == odjeljenjeId))
        //    {
        //        _context.Remove(dodjeljeniPredmet);
        //    }

        //    foreach (var odjeljenjeStavka in _context.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == odjeljenjeId))
        //    {
        //        _context.Remove(odjeljenjeStavka);
        //    }

        //    var odjeljenje = _context.Odjeljenje.Find(odjeljenjeId);

        //    if (odjeljenje != null)
        //        _context.Remove(odjeljenje);

        //    await _context.SaveChangesAsync();
        //}

        public async Task<ServiceResult> DodajUcenika(int ucenikId, int odjeljenjeId, int brojUDnevniku)
        {
            var odjeljenje = _context.Odjeljenje.Find(odjeljenjeId);

            if (_context.OdjeljenjeStavka.Any(x => x.OdjeljenjeId == odjeljenjeId
                                                   && x.UcenikId == ucenikId))
            {
                return new ServiceResult
                {
                    Message = $"Ucenik sa ID-em {ucenikId} vec postoji u odjeljenju ovom odjeljenju.",
                    Failed = true,
                };
            }

            if (_context.OdjeljenjeStavka.Any(x => x.BrojUDnevniku == brojUDnevniku))
            {
                return new ServiceResult
                {
                    Message = $"Ucenik sa brojem u dnevniku {brojUDnevniku} vec postoji.",
                    Failed = true,
                };
            }


            var novaStavka = new OdjeljenjeStavka
            {
                BrojUDnevniku = brojUDnevniku,
                OdjeljenjeId = odjeljenjeId,
                UcenikId = ucenikId
            };

            await _context.AddAsync(novaStavka);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Message="Uspjesno dodat ucenik u odjeljenje",
                Success = true
            };
        }

        public async Task<ServiceResult> ObrisiUcenika(int stavkaId)
        {
            var stavka = _context.OdjeljenjeStavka.Find(stavkaId);

            if (stavka ==null)
                return new ServiceResult
                {
                    Message = "Ucenik nije pronadjen u ovom odjeljenju.",
                    Failed = true
                };

            _context.Remove(stavka);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Message = "Uspjesno obrisan ucenik iz odjeljenja.",
                Success = true
            };
        }


        public async Task<ServiceResult> RekonstruisiDnevnik(int odjeljenjeId)
        {
            var odjeljenje = _context.Odjeljenje.Find(odjeljenjeId);

            if (odjeljenje == null)
                return new ServiceResult
                {
                    Message = "Odjeljenje nije pronadjeno.",
                    Failed = true
                };

            var uceniciStavke = _context.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == odjeljenjeId);

            if (uceniciStavke.Any())
            {
                var uceniciStavkeList = await uceniciStavke.OrderBy(x => x.Ucenik.ImePrezime).ToListAsync();

                for (int i = 0; i < uceniciStavkeList.Count(); i++)
                {
                    uceniciStavkeList[i].BrojUDnevniku = i+1;
                }

                _context.UpdateRange(uceniciStavkeList);
            }

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Message = "Uspjesno rekonstruisani brojevi u dnevniku abecednim redoslijedom",
                Success = true
            };
        }


        private bool IsValidnaNarednaSkolskaGodina(string skolskaNiza, string skolskaNova)
        {
            if (string.IsNullOrEmpty(skolskaNiza) || string.IsNullOrEmpty(skolskaNova))
                return false;

            var prviDeoNiza = int.Parse(skolskaNiza.Substring(0, 4));
            var prviDeoNova = int.Parse(skolskaNova.Substring(0, 4));

            var drugiDeoNiza = int.Parse(skolskaNiza.Substring(skolskaNiza.Length - 2, 2));
            var drugiDeoNova = int.Parse(skolskaNova.Substring(skolskaNova.Length - 2, 2));

            if (prviDeoNova != (prviDeoNiza + 1) || drugiDeoNova != (drugiDeoNiza+1))
                return false;
                    
            return true;
        }
    }
}