using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.EntityModels;

namespace Ispit_2017_09_11_DotnetCore.Interfaces
{
    public interface IOdjeljenjeService
    {
        Task<double> GetProsjekOcjena(int odjeljenjeId);
        Task PrebaciUViseOdjeljenje(Odjeljenje nizeOdjeljenje, Odjeljenje novoOdjeljenje);
        Task IzbrisiOdjeljenjeAndStavke(int odjeljenjeId);
    }
}