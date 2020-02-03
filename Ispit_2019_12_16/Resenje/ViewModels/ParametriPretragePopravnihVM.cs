using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class ParametriPretragePopravnihVM
    {
        [Required(ErrorMessage = "Morate odabrati skolsku godinu.")]
        [Display(Name = "Skolska godina")]
        public int SkolskaGodinaId { get; set; }
        public List<SelectListItem> SkolskeGodine { get; set; }

        [Required(ErrorMessage = "Morate odabrati skolu.")]
        [Display(Name = "Skola")]
        public int SkolaId { get; set; }
        public List<SelectListItem> Skole { get; set; }

        [Required(ErrorMessage = "Morate odabrati predmet.")]
        [Display(Name = "Predmet")]
        public int PredmetId { get; set; }
        public List<SelectListItem> Predmeti { get; set; }
    }
}