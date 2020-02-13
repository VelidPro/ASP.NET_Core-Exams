using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ispit_2017_02_15.ViewModels
{
    public class OdrzaniCasDetaljiVM
    {
        public int Id { get; set; }
        public string Nastavnik { get; set; }
        [Display(Name="Akademska godian / Predmet")]
        public string AkademskaGodinaPredmet { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Morate odabrati datum.")]
        public DateTime Datum { get; set; }

        public List<OdrzaniCasDetaljVM> Prisustva { get; set; }
    }

    public class OdrzaniCasDetaljVM
    {
        public int Id { get; set; }
        public string Student { get; set; }
        public int? Bodovi { get; set; }
        public bool IsPrisutan { get; set; }
    }
}