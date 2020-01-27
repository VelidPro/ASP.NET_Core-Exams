using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.Constants;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Interfaces;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class PredmetiController : Controller
    {
        private readonly MojContext _context;
        private readonly IDataProtector _protector;
        private readonly IPredmetService _predmetService;

        public PredmetiController(MojContext context,
            IDataProtectionProvider protectionProvider,
            SecurityConstants securityConstants, IPredmetService predmetService)
        {
            _context = context;
            _predmetService = predmetService;
            _protector = protectionProvider.CreateProtector(securityConstants.DataProtectorDisplayingPurpose);
        }

        public async Task<IActionResult> Index()
        {
            var model = await BuildPredmetiViewModel();

            return View(model);
        }





        private async Task<PredmetiVM> BuildPredmetiViewModel()
        {

            if(!_context.Angazovan.Any())
                return new PredmetiVM{Predmeti = new Dictionary<string, List<AngazovanVM>>()};


            var predmeti = await _context.Angazovan
                .Include(x => x.Predmet)
                .Include(x => x.Nastavnik)
                .Include(x => x.AkademskaGodina)
                .ToListAsync();

            var predmetiGroups = new Dictionary<string, List<AngazovanVM>>();

            var predmetiNazivi = predmeti.Select(x => x.Predmet.Naziv);
            predmetiNazivi = predmetiNazivi.Distinct();

            foreach (var x in predmetiNazivi)
            {
                predmetiGroups.Add(x, predmeti
                    .Where(y => y.Predmet.Naziv == x)
                    .Select( y => new AngazovanVM
                    {
                        Id = _protector.Protect(y.Id.ToString()),
                        AkademskaGodina = y.AkademskaGodina.Opis,
                        Nastavnik = y.Nastavnik.Ime +" "+y.Nastavnik.Prezime,
                        BrojOdrzanihCasova = _predmetService.GetBrojOdrzanihCasova(y.Id),
                        BrojStudenataNaPredmetu = _predmetService.GetBrojStudenata(y.Id)

                    }).ToList());
            }

            return new PredmetiVM
            {
                Predmeti = predmetiGroups
            };

        }
    }
}