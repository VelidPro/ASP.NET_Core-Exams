
using System.Collections.Generic;
using System.Threading.Tasks;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.HelpModels;

namespace RS1_Ispit_asp.net_core.Interfaces
{
    public interface ITakmicenjeService
    {
        Task<List<Takmicenje>> GetTakmicenja(int skolaId, int razred);
        string GetNajboljiUcesnikString(int takmicenjeId);

        Task<ServiceResult> DodajTakmicenje(Takmicenje takmicenje);

        Task<ServiceResult> DodajUcesnika(TakmicenjeUcesnik ucesnik);

    }
}