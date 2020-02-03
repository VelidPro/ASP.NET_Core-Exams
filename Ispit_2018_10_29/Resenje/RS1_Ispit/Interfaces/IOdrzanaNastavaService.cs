using System.Collections.Generic;
using System.Threading.Tasks;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.HelpModels;

namespace RS1_Ispit_asp.net_core.Interfaces
{
    public interface IOdrzanaNastavaService
    {
        Task<List<Nastavnik>> GetNastavnikePredavace();
       Task<List<string>> GetOdsutniUceniciString(int odrzaniCasId);

       Task<List<PredajePredmet>> GetPredmetePredaje(int nastavnikId);

       Task<ServiceResult> DodajOdrzaniCas(OdrzaniCas cas);

       Task<ServiceResult> ObrisiOdrzaniCas(int odrzaniCasId);

    }
}