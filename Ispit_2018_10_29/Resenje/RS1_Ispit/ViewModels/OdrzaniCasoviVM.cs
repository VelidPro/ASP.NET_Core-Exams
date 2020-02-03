using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class OdrzaniCasoviVM
    {
        public int NastavnikId { get; set; }
        public List<OdrzaniCasVM> OdrzaniCasovi { get; set; }
    }

    public class OdrzaniCasVM
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string SkGodinaOdjeljenje { get; set; }
        public string Predmet { get; set; }
        public List<string> OdsutniUcenici { get; set; }
    }
}