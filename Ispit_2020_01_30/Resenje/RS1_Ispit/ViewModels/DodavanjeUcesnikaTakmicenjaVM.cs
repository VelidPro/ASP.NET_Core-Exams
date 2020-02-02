using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjeUcesnikaTakmicenjaVM
    {
        public int TakmicenjeId { get; set; }
        [Required(ErrorMessage="Morate odabrati ucesnika.")]
        [Display(Name="Ucesnik")]
        public int OdjeljenjeStavkaId { get; set; }
        [Display(Name="Bodovi")]
        [Range(0,100)]
        public int OsvojeniBodovi { get; set; }

        public List<SelectListItem> PonudjeniUcesnici { get; set; }
    }
}