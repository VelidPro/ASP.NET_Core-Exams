using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.Constants;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;

namespace RS1_Ispit_asp.net_core.Services
{
    public class PredmetService: IPredmetService
    {

        private readonly MojContext _context;
        private readonly IDataProtector _protector;

        public PredmetService(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants)
        {
            _context = context;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }

        public int GetBrojOdrzanihCasova(int angazovanId)
        {

            return  _context.OdrzaniCas.Count(x => x.AngazovaniId == angazovanId);
        }

        public int GetBrojStudenata(int angazovanId)
        {
            return  _context.SlusaPredmet.Count(x => x.AngazovanId == angazovanId);
        }
    }
}