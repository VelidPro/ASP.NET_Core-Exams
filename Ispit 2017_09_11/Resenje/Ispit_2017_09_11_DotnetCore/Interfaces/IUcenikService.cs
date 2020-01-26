using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.EntityModels;

namespace Ispit_2017_09_11_DotnetCore.Interfaces
{
    public interface IUcenikService
    {
        Task<Ucenik> GetNajboljiUcenik(int odjeljenjeId);
        double GetProsjekUcenika(int ucenikId, int odjeljenjeId);
        bool IsNegativanOpstiUspjeh(int ucenikId, int odjeljenjeId);
    }
}