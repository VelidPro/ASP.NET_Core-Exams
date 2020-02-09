using Microsoft.EntityFrameworkCore;
using RS1_PrakticniDioIspita_2017_01_24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_PrakticniDioIspita_2017_01_24.EF
{
    public class DbInitializer
    {
        public static async Task SeedData(MojContext _context)
        {
            var users = new List<User>();
            var nastavnici = new List<Nastavnik>();
            var predmeti = new List<Predmet>();
            var ucenici = new List<Ucenik>();
            var odjeljenja = new List<Odjeljenje>();
            var odrzaniCasovi = new List<OdrzaniCas>();
            var odrzaniCasDetalji = new List<OdrzaniCasDetalj>();
            var angazovani = new List<Angazovan>();
            var upisiUOdjeljenje = new List<UpisUOdjeljenje>();

            if (!await _context.Users.AnyAsync())
            {
                for (int i = 0; i < 40; i++)
                {
                    var tempGuid = Guid.NewGuid().ToString();

                    users.Add(new User
                    {
                        Username="Nastavnik"+(i+1),
                        Password = "1234"
                    });
                }

                await _context.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }
            else
            {
                users = await _context.Users.ToListAsync();
            }

            if (!await _context.Nastavnici.AnyAsync())
            {
                for (int i = 0; i < 40; i++)
                {
                    var tempGuid = Guid.NewGuid().ToString();

                    nastavnici.Add(new Nastavnik
                    {
                        Ime = tempGuid.Substring(tempGuid.Length - 5, 4),
                        UserId=users[i].Id
                    });
                }

                await _context.AddRangeAsync(nastavnici);
                await _context.SaveChangesAsync();
            }
            else
            {
                nastavnici = await _context.Nastavnici.ToListAsync();
            }

            if (!await _context.Predmeti.AnyAsync())
            {
                for (int i = 0; i < 47; i++)
                {
                    var tempGuid = Guid.NewGuid().ToString();

                    predmeti.Add(new Predmet
                    {
                        Naziv = tempGuid.Substring(tempGuid.Length - 5, 4)
                    });
                }

                await _context.AddRangeAsync(predmeti);
                await _context.SaveChangesAsync();
            }
            else
            {
                predmeti = await _context.Predmeti.ToListAsync();
            }

            if (!await _context.Ucenici.AnyAsync())
            {
                for (int i = 0; i < 80; i++)
                {
                    var tempGuid = Guid.NewGuid().ToString();

                    ucenici.Add(new Ucenik
                    {
                        Ime = tempGuid.Replace("-", string.Empty).Substring(2, 4).ToUpper() + " " +
                              tempGuid.Replace("-", string.Empty).Substring(tempGuid.Length / 2, 4).ToUpper()
                    });
                }

                await _context.AddRangeAsync(ucenici);
                await _context.SaveChangesAsync();
            }
            else
            {
                ucenici = await _context.Ucenici.ToListAsync();
            }

            if (!await _context.Odjeljenja.AnyAsync())
            {
                for (int i = 0; i < 8; i++)
                {
                    var tempRazred = i <=2 ?1 : i!=0 && i%2==0?i/2:i/2+1;
                    odjeljenja.Add(new Odjeljenje
                    {
                        NastavnikId = nastavnici[i].Id,
                        Oznaka = tempRazred + " - " + (i!=0 && i%2==0?1:2),
                        Razred = tempRazred
                    });
                }

                await _context.AddRangeAsync(odjeljenja);
                await _context.SaveChangesAsync();
            }
            else
            {
                odjeljenja = await _context.Odjeljenja.ToListAsync();
            }

            if (!await _context.UpisiUOdjeljenja.AnyAsync())
            {
                var odjeljenjeCounter = 0;
                var brojDnevnik = 1;

                for (int i = 0; i < ucenici.Count; i++)
                {
                   
                    if (i != 0 && i % 10 == 0)
                        odjeljenjeCounter++;
                    if (odjeljenjeCounter>= 8)
                        break;
                    if (brojDnevnik > 10)
                        brojDnevnik = 1;

                    Random rand = new Random();

                    upisiUOdjeljenje.Add(new UpisUOdjeljenje
                    {
                        BrojUDnevniku = brojDnevnik++,
                        OdjeljenjeId = odjeljenja[odjeljenjeCounter].Id,
                        UcenikId = ucenici[i].Id
                    });
                }

                await _context.AddRangeAsync(upisiUOdjeljenje);
                await _context.SaveChangesAsync();
            }
            else
            {
                upisiUOdjeljenje = await _context.UpisiUOdjeljenja.ToListAsync();
            }


            if (!await _context.Angazovani.AnyAsync())
            {
                for (int i = 0; i < 2; i++)
                {

                    for (int j = 0; j < 10; j++)
                    {
                        var rand = new Random();

                        angazovani.Add(new Angazovan
                        {
                            NastavnikId = nastavnici[rand.Next(0, nastavnici.Count)].Id,
                            OdjeljenjeId = odjeljenja[i].Id,
                            PredmetId = predmeti[j].Id
                        });
                    }
                }

                for (int i = 2; i < 4; i++)
                {

                    for (int j = 10; j < 20; j++)
                    {
                        var rand = new Random();

                        angazovani.Add(new Angazovan
                        {
                            NastavnikId = nastavnici[rand.Next(0,nastavnici.Count)].Id,
                            OdjeljenjeId = odjeljenja[i].Id,
                            PredmetId = predmeti[j].Id
                        });
                    }
                }

                for (int i = 4; i < 6; i++)
                {
                    for (int j = 20; j < 30; j++)
                    {
                        var rand = new Random();

                        angazovani.Add(new Angazovan
                        {
                            NastavnikId = nastavnici[rand.Next(0, nastavnici.Count)].Id,
                            OdjeljenjeId = odjeljenja[i].Id,
                            PredmetId = predmeti[j].Id
                        });
                    }
                }

                for (int i = 6; i < 8; i++)
                {
                    for (int j = 30; j < 40; j++)
                    {
                        var rand = new Random();

                        angazovani.Add(new Angazovan
                        {
                            NastavnikId = nastavnici[rand.Next(0, nastavnici.Count)].Id,
                            OdjeljenjeId = odjeljenja[i].Id,
                            PredmetId = predmeti[j].Id
                        });
                    }
                }

                for (int i = 6; i < 8; i++)
                {
                    for (int j = 40; j < 43; j++)
                    {
                        var rand = new Random();

                        angazovani.Add(new Angazovan
                        {
                            NastavnikId = nastavnici[rand.Next(0, nastavnici.Count)].Id,
                            OdjeljenjeId = odjeljenja[i].Id,
                            PredmetId = predmeti[j].Id
                        });
                    }
                }

                for (int i = 4; i < 6; i++)
                {

                    for (int j = 43; j < 47; j++)
                    {
                        var rand = new Random();

                        angazovani.Add(new Angazovan
                        {
                            NastavnikId = nastavnici[rand.Next(0, nastavnici.Count)].Id,
                            OdjeljenjeId = odjeljenja[i].Id,
                            PredmetId = predmeti[j].Id
                        });
                    }
                }

                await _context.AddRangeAsync(angazovani);
                await _context.SaveChangesAsync();
            }
            else
            {
                angazovani = await _context.Angazovani.ToListAsync();
            }

            if (!await _context.OdrzaniCasovi.AnyAsync())
            {
                for (int i = 0; i < angazovani.Count; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        Random rand = new Random();

                        odrzaniCasovi.Add(new OdrzaniCas
                        {
                            datum = DateTime.Now.AddDays(-rand.Next(5, 200)),
                            AngazovanId = angazovani[i].Id
                        });
                    }
                }

                await _context.AddRangeAsync(odrzaniCasovi);
                await _context.SaveChangesAsync();
            }
            else
            {
                odrzaniCasovi = await _context.OdrzaniCasovi.ToListAsync();
            }

            if (!await _context.OdrzaniCasDetalji.AnyAsync())
            {
                for (int i = 0; i < odrzaniCasovi.Count; i++)
                {
                    var upisi = upisiUOdjeljenje
                        .Where(x => x.OdjeljenjeId == odrzaniCasovi[i].Angazovan.OdjeljenjeId);

                    foreach (var x in upisi)
                    {
                        Random rand = new Random();

                        odrzaniCasDetalji.Add(new OdrzaniCasDetalj
                        {
                            Ocjena = rand.Next(1, 5),
                            OdrzaniCasId = odrzaniCasovi[i].Id,
                            Odsutan = rand.Next(2) == 1,
                            UpisUOdjeljenjeId = x.Id
                        });
                    }
                }

                await _context.AddRangeAsync(odrzaniCasDetalji);
                await _context.SaveChangesAsync();
            }
            else
            {
                odrzaniCasDetalji = await _context.OdrzaniCasDetalji.ToListAsync();
            }
        }
    }
}