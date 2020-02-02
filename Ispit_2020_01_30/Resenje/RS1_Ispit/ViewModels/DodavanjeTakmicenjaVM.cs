using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjeTakmicenjaVM
    {
        public string SkolaDomacin { get; set; }
        public int SkolaDomacinId { get; set; }

        [Required(ErrorMessage ="Morate odabrati predmet.")]
        [Display(Name="Predmet")]
        public string Predmet { get; set; }

        [Required(ErrorMessage = "Morate odabrati razred.")]
        public int Razred { get; set; }

        [Required(ErrorMessage = "Morate odabrati datum odrzavanja.")]
        [DataType(DataType.Date)]
        public DateTime DatumOdrzavanja { get; set; }

        public List<SelectListItem> Razredi { get; set; }
        public List<SelectListItem> Predmeti { get; set; }

    }
}