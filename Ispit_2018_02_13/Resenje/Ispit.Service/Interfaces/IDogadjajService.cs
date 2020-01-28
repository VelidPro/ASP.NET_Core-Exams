using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ispit.Data.EntityModels;
using Ispit.Service.Models;

namespace Ispit.Service.Interfaces
{
    public interface IDogadjajService
    {
        Task<List<OznacenDogadjaj>> GetOznaceneDogadjaje(int studentId);
        Task<List<Dogadjaj>> GetDogadjaje();

        int GetProcenatRealizovanihObaveza(int dogadjajId);

        Task<ServiceResult> OznaciDogadjaj(int dogadjajId, int studentId);

        bool IspunjavaUslovNotificiranja(StanjeObaveze stanjeObaveze);
        Task<List<string>> ListaDanasnjihNotifikacija(int studentId);

        Task<bool> OznaciNotifikacijuProcitanom(int notifikacijaId);

    }
}
