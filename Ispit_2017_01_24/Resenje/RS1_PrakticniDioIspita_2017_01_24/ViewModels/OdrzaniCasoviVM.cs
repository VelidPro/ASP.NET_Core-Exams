using System;
using System.Collections.Generic;

namespace RS1_PrakticniDioIspita_2017_01_24.ViewModels
{
    public class OdrzaniCasoviVM
    {
        public int NastavnikId { get; set; }
        public string Nastavnik { get; set; }
        public List<OdrzaniCasVM> OdrzaniCasovi { get; set; }
    }

    public class OdrzaniCasVM
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Odjeljenje { get; set; }
        public string Predmet { get; set; }
    }
}