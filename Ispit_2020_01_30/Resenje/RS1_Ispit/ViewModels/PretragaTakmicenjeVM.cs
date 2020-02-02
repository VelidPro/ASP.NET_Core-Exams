using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PretragaTakmicenjeVM
    {
        [Required(ErrorMessage = "Morate odabrati skolu.")]
        [Display(Name="Skola domacin")]
        public int SkolaDomacinId { get; set; }

        public int? Razred { get; set; }

        public List<SelectListItem> Skole { get; set; }
        public List<SelectListItem> Razredi { get; set; }
    }
}