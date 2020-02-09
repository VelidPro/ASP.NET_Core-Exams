using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1_PrakticniDioIspita_2017_01_24.Models;
using RS1_PrakticniDioIspita_2017_01_24.ServiceModels;

namespace RS1_PrakticniDioIspita_2017_01_24.Interfaces
{
    public interface IOdrzaniCasService
    {
        Task<ServiceResult> DodajCas(OdrzaniCas cas);
        Task<int> BrojPrisutnih(int odrzaniCasId);
        Task<Ucenik> NajboljiUcenik(int predmetId,int odjeljenjeId);

        Task<List<SelectListItem>> GetOdjeljenjaPredmeti(int nastavnikId, int? angazovanId=null);
    }
}
