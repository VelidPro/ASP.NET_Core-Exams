using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_02_15.ViewModels
{
    public class OdrzaniCasoviListVM
    {
        public string Nastavnik { get; set; }
        public List<OdrzaniCasVM> OdrzaniCasovi { get; set; }
    }

    public class OdrzaniCasVM
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string AkademskaGodina { get; set; }
        public int BrojPrisutnih { get; set; }
        public double ProsjecnaOcjena { get; set; }
        public string Predmet { get; set; }
    }
}
