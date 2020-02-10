using System.Collections.Generic;
using System.Threading.Tasks;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.HelpModels;

namespace RS1_Ispit_asp.net_core.Interfaces
{
    public interface IMaturskiIspitService
    {
        Task<List<Ucenik>> UceniciNisuPristupili(int maturskiIspitId);

        Task<ServiceResult> DodajNovi(MaturskiIspit maturskiIspit);

       Task<double> GetProsjekUcenika(int ucenikId);
       Task<bool> IspunjavaUslovPolaganja(int ucenikId);

    }
}