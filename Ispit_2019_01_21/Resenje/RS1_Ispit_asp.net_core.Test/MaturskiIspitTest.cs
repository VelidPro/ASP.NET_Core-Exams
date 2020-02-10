using Microsoft.VisualStudio.TestTools.UnitTesting;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RS1_Ispit_asp.net_core.Test
{
    [TestClass]
    public class MaturskiIspitTest
    {
        [TestMethod]
        public async Task Student_Ispunjava_Uslove__Za_Polaganje()
        {
            var _context = MojContextHelper.GetMojContext();
            var ucenici = new List<Ucenik>
            {
                new Ucenik {ImePrezime = "I122 I333"},
                new Ucenik {ImePrezime = "B122 B333"},
                new Ucenik {ImePrezime = "C122 C333"},
                new Ucenik {ImePrezime = "D122 D333"},
                new Ucenik {ImePrezime = "E122 E333"},
                new Ucenik {ImePrezime = "F122 F333"},
                new Ucenik {ImePrezime = "G122 G333"}


            };

            var uceniciImena = ucenici.Select(x => x.ImePrezime);

            if (!await _context.Ucenik.AnyAsync(x=>x.ImePrezime==ucenici[5].ImePrezime))
            {
                await SeedTestData(ucenici);
            }
            else
            {
                for (int i = 0; i < ucenici.Count(); i++)
                {
                    ucenici[i] = _context.Ucenik.LastOrDefault(x => x.ImePrezime == ucenici[i].ImePrezime);
                }
            }

            MaturskiIspitService maturskiIspitService = new MaturskiIspitService(_context);

            //Prvi ucenik ima jednu negativnu zakljucnu ocjenu
            //Nema maturskih ispita koje je polagao iz ovog predmeta
            Assert.AreEqual(await maturskiIspitService.IspunjavaUslovPolaganja(ucenici[0].Id), false);

            //Drugi ucenik nema negativnih ocjena
            //Nema maturskih ispita koje je polagao iz ovog predmeta
            Assert.AreEqual(await maturskiIspitService.IspunjavaUslovPolaganja(ucenici[1].Id), true);

            //Treci ucenik ima 3 negativne ocjene
            //Nema maturskih ispita koje je polagao iz ovog predmeta
            Assert.AreEqual(await maturskiIspitService.IspunjavaUslovPolaganja(ucenici[2].Id), false);

            //Cetvrti ucenik nema negativnih ocjena
            //Iz ovog predmeta je polagao maturski ispit 2 puta i to jednom neuspjesno i drugi put uspjesno polozio
            //Tako da je ocekivana vrijednost FALSE tj. nema pravo izlaska na predstojeci rok
            Assert.AreEqual(await maturskiIspitService.IspunjavaUslovPolaganja(ucenici[3].Id), false);

            //Peti ucenik nema negativnih ocjena
            //Iz ovog predmeta je polagao maturski ispit jednom i polozio ga
            //Tako da je ocekivana vrijednost FALSE tj. nema pravo izlaska na predstojeci rok
            Assert.AreEqual(await maturskiIspitService.IspunjavaUslovPolaganja(ucenici[4].Id), false);

            //Sesti ucenik nema negativnih ocjena
            //Iz ovog predmeta je polagao maturski ispit jednom i nije polozio
            //Tako da je ocekivana vrijednost TRUE tj. ima pravo izlaska na predstojeci rok
            Assert.AreEqual(await maturskiIspitService.IspunjavaUslovPolaganja(ucenici[5].Id), true);


        }

        private async Task SeedTestData(List<Ucenik> ucenici)
        {
            var _context = MojContextHelper.GetMojContext();

            MaturskiIspitService maturskiIspitService = new MaturskiIspitService(_context);

            Skola skola1 = new Skola { Naziv = "FIT Mostar" };
            SkolskaGodina skolskaGodina1 = new SkolskaGodina { Aktuelna = true, Naziv = "2017/18" };
            Nastavnik n1 = new Nastavnik { Ime = "Nastavnik1", Prezime = "PrezimeNastavnik1" };

            var predmeti = new List<Predmet>();

            for (int i = 0; i < 5; i++)
            {
                predmeti.Add(new Predmet { Naziv = Guid.NewGuid().ToString(), Razred = 4 });
            }

            await _context.AddAsync(skola1);
            await _context.AddAsync(skolskaGodina1);
            await _context.AddAsync(n1);
            await _context.AddRangeAsync(predmeti);
            await _context.SaveChangesAsync();

            Odjeljenje o1 = new Odjeljenje
            {
                IsPrebacenuViseOdjeljenje = false,
                Oznaka = "IV-4",
                Razred = 4,
                RazrednikID = n1.Id,
                SkolskaGodinaID = skolskaGodina1.Id,
                SkolaID = skola1.Id
            };

            await _context.AddRangeAsync(ucenici);
            await _context.AddAsync(o1);
            await _context.SaveChangesAsync();

            var odjeljenjeStavke = new List<OdjeljenjeStavka>();

           for(int i=0;i<ucenici.Count();i++)
            {
                odjeljenjeStavke.Add(new OdjeljenjeStavka
                {
                    BrojUDnevniku = i+1,
                    OdjeljenjeId = o1.Id,
                    UcenikId = ucenici[i].Id
                });
            }

            await _context.AddRangeAsync(odjeljenjeStavke);

            await _context.SaveChangesAsync();

            var dodjeljeniPredmeti = new List<DodjeljenPredmet>();

            for (int j = 0; j < odjeljenjeStavke.Count()/2; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    var ocjena = 1;

                    if (j == 0 && i < 4)
                        ocjena = 4;
                    else if (j == 1)
                        ocjena = 3;
                    else if (j == 2 && i < 3)
                        ocjena = 5;

                    dodjeljeniPredmeti.Add(new DodjeljenPredmet
                    {
                        OdjeljenjeStavkaId = odjeljenjeStavke[j].Id,
                        PredmetId = predmeti[i].Id,
                        ZakljucnoKrajGodine = ocjena
                    });
                }
            }

            for (int j = odjeljenjeStavke.Count() / 2; j <odjeljenjeStavke.Count()-1 ; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    dodjeljeniPredmeti.Add(new DodjeljenPredmet
                    {
                        OdjeljenjeStavkaId = odjeljenjeStavke[j].Id,
                        PredmetId = predmeti[i].Id,
                        ZakljucnoKrajGodine = 5
                    });
                }
            }


            await _context.AddRangeAsync(dodjeljeniPredmeti);
            await _context.SaveChangesAsync();

            MaturskiIspit m1 = new MaturskiIspit
            {
                DatumOdrzavanja = DateTime.Now.AddDays(-20),
                Napomena = string.Empty,
                NastavnikId = n1.Id,
                PredmetId = predmeti[0].Id,
                SkolaId = skola1.Id,
                SkolskaGodinaId = skolskaGodina1.Id
            };

            MaturskiIspit m2 = new MaturskiIspit
            {
                DatumOdrzavanja = DateTime.Now.AddDays(-10),
                Napomena = string.Empty,
                NastavnikId = n1.Id,
                PredmetId = predmeti[0].Id,
                SkolaId = skola1.Id,
                SkolskaGodinaId = skolskaGodina1.Id
            };

            await _context.AddAsync(m1);
            await _context.SaveChangesAsync();

            var polaganja = new List<MaturskiIspitStavka>();

            polaganja.Add(new MaturskiIspitStavka
            {
                IsPristupio = true,
                MaturskiIspitId = m1.Id,
                OsvojeniBodovi = 50,
                UcenikId = ucenici[3].Id
            });

            polaganja.Add(new MaturskiIspitStavka
            {
                IsPristupio = true,
                MaturskiIspitId = m1.Id,
                OsvojeniBodovi = 70,
                UcenikId = ucenici[3].Id
            });

            polaganja.Add(new MaturskiIspitStavka
            {
                IsPristupio = true,
                MaturskiIspitId = m1.Id,
                OsvojeniBodovi = 75,
                UcenikId = ucenici[4].Id
            });


            polaganja.Add(new MaturskiIspitStavka
            {
                IsPristupio = true,
                MaturskiIspitId = m1.Id,
                OsvojeniBodovi = 20,
                UcenikId = ucenici[5].Id
            });

            await _context.AddRangeAsync(polaganja);
            await _context.SaveChangesAsync();
        }
    }
}