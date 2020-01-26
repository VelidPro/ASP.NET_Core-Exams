using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.Helpers;

namespace Ispit_2017_09_11_DotnetCore.Interfaces
{
    public interface IOdjeljenjeService
    {
        Task<double> GetProsjekOcjena(int odjeljenjeId);
        Task<ServiceResult> ObrisiOdjeljenje(int odjeljenjeId);
        Task<ServiceResult> PrebaciUViseOdjeljenje(Odjeljenje novoOdjeljenje, Odjeljenje nizeOdjeljenje=null);
        //Task IzbrisiOdjeljenjeAndStavke(int odjeljenjeId);
        Task<ServiceResult> DodajUcenika(int ucenikId,int odjeljenjeId,int brojUDnevniku);
        Task<ServiceResult> ObrisiUcenika(int stavkaId);
        Task<ServiceResult> RekonstruisiDnevnik(int odjeljenjeId);

    }
}