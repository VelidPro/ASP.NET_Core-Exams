using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ispit_2017_02_15.ViewModels
{
    public class OdrzaniCasInputVM
    {
        public int? Id { get; set; }
        public string Nastavnik { get; set; }

        [Required(ErrorMessage = "Morate odabrati datum.")]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Morate odabrati akademsku godinu i predmet.")]
        [Display(Name="Skolska godina / Predmet")]
        public int AngazujeId { get; set; }
        public List<SelectListItem> AkademskeGodinePredmeti { get; set; }


    }


}
