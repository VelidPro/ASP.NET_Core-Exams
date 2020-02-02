using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class RezultatiTakmicenjaVM
    {
        public int Id { get; set; }
        public int SkolaDomacinId { get; set; }
        [Display(Name="Skola domacin")]
        public string SkolaDomacin { get; set; }
        public string Predmet { get; set; }
        public bool IsEvidentiraniRezultati { get; set; }
        public int Razred { get; set; }
        [Display(Name="Datum")]
        public DateTime DatumOdrzavanja { get; set; }
        public List<RezultatTakmicenjaVM> Rezultati { get; set; }
    }

    public class RezultatTakmicenjaVM
    {
        public int Id { get; set; }
        public string Odjeljenje { get; set; }
        public int BrojUDnevniku { get; set; }
        public bool IsPristupio { get; set; }
        public int OsvojeniBodovi { get; set; }
    }
}