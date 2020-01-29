using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.HelpModels;
using RS1.Ispit.Web.Models;

namespace Ispit_2017_09_11_DotnetCore.Interfaces
{
    public interface IUputnicaService
    {
        Task<ServiceResult> DodajAsync(Uputnica uputnica);

        Task<string> GetReferentneVrijednosti(int labPretragaId);
    }
}