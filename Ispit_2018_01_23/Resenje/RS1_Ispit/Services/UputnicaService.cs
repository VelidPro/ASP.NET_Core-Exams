using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.Constants;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.HelpModels;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using RS1.Ispit.Web.Models;

namespace Ispit_2017_09_11_DotnetCore.Services
{
    public class UputnicaService:IUputnicaService
    {
        private readonly MojContext _context;
        private readonly IDataProtector _protector;

        public UputnicaService(MojContext context, IDataProtectionProvider protectionProvider, SecurityConstants securityConstants)
        {
            _context = context;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }


        public async Task<ServiceResult> DodajAsync(Uputnica uputnica)
        {
            try
            {
                if(await _context.Uputnica
                    .AnyAsync(x => x.PacijentId == uputnica.PacijentId
                                   && x.VrstaPretrageId == uputnica.VrstaPretrageId
                                   && x.DatumUputnice.Date == uputnica.DatumUputnice.Date))
                    return new ServiceResult{
                    Failed = true,
                    Message = "Ista uputnica vec postoji."
                    };

                await _context.AddAsync(uputnica);
                await _context.SaveChangesAsync();

                await KreirajNoveRezultatePretraga(uputnica.VrstaPretrageId, uputnica.Id);
            }
            catch(Exception ex)
            {
                return new ServiceResult
                {
                    Failed = true,
                    Message = ex.Message
                };
            }

            return new ServiceResult
            {
                Message = "Uspjesno kreirana uplatnica.",
                Success = true
            };
        }


        private async Task KreirajNoveRezultatePretraga(int vrstaPretrageId, int uputnicaId)
        {
            if (vrstaPretrageId <= 0 || uputnicaId<=0)
                return;

            var labPretrage =  _context.LabPretraga
                .Where(x => x.VrstaPretrageId == vrstaPretrageId);

            if (await labPretrage.AnyAsync())
            {
                foreach (var pretraga in labPretrage)
                {
                    await _context.AddAsync(new RezultatPretrage
                    {
                        LabPretragaId = pretraga.Id,
                        ModalitetId=null,
                        NumerickaVrijednost = null,
                        UputnicaId = uputnicaId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}