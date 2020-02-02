using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjaVM
    {
        public int SkolaDomacinId { get; set; }
        public string SkolaDomacin { get; set; }
        public int? Razred { get; set; }
        public List<TakmicenjeVM> Takmicenja { get; set; }
    }

    public class TakmicenjeVM
    {
        public int Id { get; set; }
        public string Predmet { get; set; }

        public int Razred { get; set; }
        public DateTime DatumOdrzavanja { get; set; }
        public int BrojUcenikaNisuPristupili { get; set; }
        public string NajboljiUcesnik { get; set; }
    }
}