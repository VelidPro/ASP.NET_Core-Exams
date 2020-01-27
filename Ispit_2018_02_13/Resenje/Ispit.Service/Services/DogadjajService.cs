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
    public class DogadjajService: IDogadjajService
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
            return await _context.OznacenDogadjaj
                .Include(x => x.Dogadjaj)
                .ThenInclude(x=>x.Nastavnik)
                .ToListAsync() ?? new List<OznacenDogadjaj>();
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
                await _context.AddAsync(new StanjeObaveze
                {
                    IzvrsenoProcentualno = 0,
                    NotifikacijaDanaPrije = 5,
                    NotifikacijeRekurizivno = false,
                    IsZavrseno = false,
                    ObavezaID = x.ID,
                    OznacenDogadjajID = noviOznaceni.ID
                });
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
    }
}
