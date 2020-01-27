using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Interfaces
{
    public interface IPredmetService
    {
        int GetBrojOdrzanihCasova(int angazovanId);
        int GetBrojStudenata(int angazovanId);
    }
}