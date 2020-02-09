using System;
using System.Collections.Generic;

namespace RS1_PrakticniDioIspita_2017_01_24.ViewModels
{
    public class OdrzaniCasoviDetaljiVM
    {
        public string Nastavnik { get; set; }
        public List<OdrzaniCasDetaljiOdrzavanjaVM> OdrzaniCasoviDetalji { get; set; }
    }

    public class OdrzaniCasDetaljiOdrzavanjaVM
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Odjeljenje { get; set; }
        public int BrojPrisutnih { get; set; }
        public string Predmet { get; set; }
        public string NajboljiUcenik { get; set; }
    }
}