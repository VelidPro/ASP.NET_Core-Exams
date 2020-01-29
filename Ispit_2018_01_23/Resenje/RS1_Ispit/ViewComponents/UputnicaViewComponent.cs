using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_09_11_DotnetCore.Constants;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.Interfaces;
using Ispit_2017_09_11_DotnetCore.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class UputnicaViewComponent:ViewComponent
    {
        private readonly MojContext _context;
        private readonly IDataProtector _protector;
        private readonly IUputnicaService _uputnicaService;

        public UputnicaViewComponent(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants,
            IUputnicaService uputnicaService)
        {
            _context = context;
            _uputnicaService = uputnicaService;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await BuildUputniceViewModel();

            return View("Index",model);
        }





        private async Task<UputniceVM> BuildUputniceViewModel()
        {
            var uputnice = new List<UputnicaVM>();

            if (!await _context.Uputnica.AnyAsync())
                return new UputniceVM { Uputnice = uputnice };

            uputnice = await _context.Uputnica
                .Include(x => x.VrstaPretrage)
                .Include(x => x.UputioLjekar)
                .Include(x => x.Pacijent)
                .Select(x => new UputnicaVM
                {
                    Id = _protector.Protect(x.Id.ToString()),
                    DatumEvidentiranjaRezultataPretrage = x.DatumRezultata,
                    DatumUputnice = x.DatumUputnice,
                    Pacijent = x.Pacijent.Ime,
                    UputioLjekar = x.UputioLjekar.Ime,
                    VrstaPretraga = x.VrstaPretrage.Naziv
                }).ToListAsync();

            return new UputniceVM
            {
                Uputnice = uputnice
            };
        }

    }
}
