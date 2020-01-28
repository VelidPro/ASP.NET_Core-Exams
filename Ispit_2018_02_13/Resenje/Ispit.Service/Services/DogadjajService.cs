using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ispit.Data;
using Ispit.Data.EntityModels;
using Ispit.Service.Constants;
using Ispit.Service.Interfaces;
using Ispit.Service.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Ispit.Service.Services
{
    public class DogadjajService: IDogadjajService, INotificiraj
    {
        private readonly MyContext _context;
        private readonly IDataProtector _protector;

        public DogadjajService(MyContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants)
        {
            _protector = protectionProvider.CreateProtector(securityConstants.ProtectorDisplayingPurpose);
            _context = context;
        }


        public async Task<List<OznacenDogadjaj>> GetOznaceneDogadjaje(int studentId)
        {
            var student = await _context.Student.FindAsync(studentId);

            return await _context.OznacenDogadjaj
                .Include(x => x.Dogadjaj)
                .Include(x => x.Dogadjaj.Nastavnik)
               .Where(x => x.StudentID == studentId).ToListAsync();

        }

        public async Task<List<Dogadjaj>> GetDogadjaje()
        {
            return await _context.Dogadjaj
                       .Include(x => x.Nastavnik)
                       .ToListAsync() ?? new List<Dogadjaj>();
        }

        public int GetProcenatRealizovanihObaveza(int dogadjajId)
        {
            var dogadjaj =  _context.Dogadjaj.Find(dogadjajId);

            if (dogadjaj == null)
                return 0;

            _context.Entry(dogadjaj).Collection(x => x.Obaveze).Load();

            int brojObaveza = dogadjaj.Obaveze.Count();

            if (brojObaveza == 0)
                return 0;

            float stanjeSum = 0;

            foreach (var o in dogadjaj.Obaveze)
            {
                stanjeSum = _context.StanjeObaveze
                    .Where(s => s.ObavezaID == o.ID)
                    .Sum(s=>s.IzvrsenoProcentualno);

            }

            if (stanjeSum==0)
                return 0;

            return (int)(stanjeSum / brojObaveza);
        }

        public async Task<ServiceResult> OznaciDogadjaj(int dogadjajId, int studentId)
        {
            if (await VecOznacen(dogadjajId, studentId))
                return new ServiceResult
                {
                    Failed=true,
                    Message="Dogadjaj vec oznacen."
                };

            if (!await _context.Student.AnyAsync(x => x.ID == studentId))
                return new ServiceResult
                {
                    Failed = true,
                    Message = "Student ne postoji."
                };

            var dogadjaj = await _context.Dogadjaj.FindAsync(dogadjajId);

            if (dogadjaj==null)
                return new ServiceResult
                {
                    Failed = true,
                    Message = "Dogadjaj nije pronadjen."
                };

            var noviOznaceni = new OznacenDogadjaj
            {
                DatumDodavanja = DateTime.Now,
                DogadjajID = dogadjajId,
                StudentID = studentId
            };


            await _context.AddAsync(noviOznaceni);

            await _context.SaveChangesAsync();


            await _context.Entry(dogadjaj).Collection(x => x.Obaveze).LoadAsync();

            foreach (var x in dogadjaj.Obaveze)
            {
                var novoStanjeObaveze = new StanjeObaveze
                {
                    IzvrsenoProcentualno = 0,
                    NotifikacijaDanaPrije = 30,
                    NotifikacijeRekurizivno = true,
                    IsZavrseno = false,
                    ObavezaID = x.ID,
                    OznacenDogadjajID = noviOznaceni.ID
                };

                await _context.AddAsync(novoStanjeObaveze);

            }

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Uspjesno oznacen."
            };
        }

        private async Task<bool> VecOznacen(int dogadjajId, int studentId)
        {
            return await _context.OznacenDogadjaj
                .AnyAsync(x => x.DogadjajID == dogadjajId
                               && x.StudentID == studentId);
        }

        public async Task<PoslataNotifikacija> CreateNotification(int stanjeObavezeId)
        {
            var notifikacija = new PoslataNotifikacija{
                DatumSlanja = DateTime.Now,
                StanjeObavezeID = stanjeObavezeId,
                Procitana = false
            };

            await _context.AddAsync(notifikacija);
            await _context.SaveChangesAsync();

            return notifikacija;
        }

        public string CreateNotificationMessageHtml(PoslataNotifikacija notifikacija)
        {
            if (notifikacija == null)
                return string.Empty;

            var dogadjaj = notifikacija.StanjeObaveze.OznacenDogadjaj.Dogadjaj;
            var obaveza = notifikacija.StanjeObaveze.Obaveza;

            if (dogadjaj == null || obaveza == null) 
                return string.Empty;

            if (notifikacija.StanjeObaveze.NotifikacijeRekurizivno == false &&
                notifikacija.StanjeObaveze.NotifikacijaDanaPrije == 0)
                return string.Empty;

            var encryptedNotifikacijaId = _protector.Protect(notifikacija.Id.ToString());

            var uniqueHtmlId = string.Concat("Notifikacija_",
                encryptedNotifikacijaId.Substring(encryptedNotifikacijaId.Length - 6, 5));

            var ajaxAttributes =
                "ajax-poziv='da' ajax-notify='da' ajax-message='Uspjesno oznacena' ajax-remove='da' ajax-remove-element='" +
                encryptedNotifikacijaId + "'";

            var template = "<div class='row pt-5' id='"+encryptedNotifikacijaId+"'><strong>Dogadjaj '"+dogadjaj.Opis+"' - "+ dogadjaj.DatumOdrzavanja.ToString("d")+".</strong><br/>";
            template += "Ovo je podsetnik za obavezu '" + obaveza.Naziv + "'. Oznaci kao ";
            template += "<a href='/Obaveza/Notifikacija/"+ encryptedNotifikacijaId + "' "+ajaxAttributes+" >procitanu</a></div>";

            return template;
        }

        public bool IspunjavaUslovNotificiranja(StanjeObaveze stanjeObaveze)
        {
            if (stanjeObaveze.NotifikacijaDanaPrije == 0)
                return false;

            var dogadjaj = stanjeObaveze.OznacenDogadjaj.Dogadjaj;

            if (dogadjaj == null)
                return false;

            if (stanjeObaveze.NotifikacijeRekurizivno && dogadjaj.DatumOdrzavanja >= DateTime.Now 
                               && stanjeObaveze.NotifikacijaDanaPrije >= dogadjaj.DatumOdrzavanja.Day - DateTime.Now.Day)
                return true;

            return stanjeObaveze.NotifikacijaDanaPrije == dogadjaj.DatumOdrzavanja.Day - DateTime.Now.Day;
        }

        public async Task<List<string>> ListaDanasnjihNotifikacija(int studentId)
        {
            var student = await _context.Student.FindAsync(studentId);
            var notifikacije = new List<string>();


            if (student==null)
                return notifikacije;

            var oznaceniDogadjaji = await GetOznaceneDogadjaje(studentId);

            foreach (var x in oznaceniDogadjaji)
            {
                await _context.Entry(x).Collection(d => d.StanjaObaveza).LoadAsync();

                foreach (var stanje in x.StanjaObaveza)
                {
                    stanje.OznacenDogadjaj = await _context.OznacenDogadjaj
                        .Include(d => d.Dogadjaj)
                        .FirstOrDefaultAsync(d => d.ID == x.ID);

                    if (IspunjavaUslovNotificiranja(stanje))
                    {
                        var novaNotifikacija =
                            await _context.PoslataNotifikacija
                                .FirstOrDefaultAsync(n => n.DatumSlanja.Date == DateTime.Now.Date
                                                          && n.StanjeObavezeID==stanje.Id);

                        if (novaNotifikacija == null)
                            novaNotifikacija = await CreateNotification(stanje.Id);

                        if (novaNotifikacija.Procitana)
                            continue;

                        notifikacije.Add(CreateNotificationMessageHtml(novaNotifikacija));
                    }
                        
                }

            }

            return notifikacije;
        }

        public async Task<bool> OznaciNotifikacijuProcitanom(int notifikacijaId)
        {
            var notifikacija = await _context.PoslataNotifikacija
                .FirstOrDefaultAsync(x => x.Id == notifikacijaId);

            if (notifikacija == null)
                return false;

            notifikacija.Procitana = true;
            _context.Update(notifikacija);

            await _context.SaveChangesAsync();
            return true;

        }
    }
}
