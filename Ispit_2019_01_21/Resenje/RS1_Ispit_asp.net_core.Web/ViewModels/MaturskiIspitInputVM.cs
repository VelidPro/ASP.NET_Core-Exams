using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class MaturskiIspitInputVM:IValidatableObject
    {
        [Display(Name="Nastavnik")]
        public int NastavnikId { get; set; }
        public string Nastavnik { get; set; }

        [Display(Name="Skolska godina")]
        public string SkolskaGodina { get; set; }
        public int SkolskaGodinaId { get; set; }


        [Display(Name="Skola")]
        [Required(ErrorMessage = "Morate odabrati skolu.")]
        public int SkolaId { get; set; }
        public List<SelectListItem> Skole { get; set; }

        [Display(Name="Datum ispita")]
        [Required(ErrorMessage = "Morate odabrati datum ispita.")]
        public DateTime DatumIspita { get; set; }

        [Display(Name = "Predmet")]
        [Required(ErrorMessage = "Morate odabrati predmet.")]
        public int PredmetId { get; set; }
        public List<SelectListItem> Predmeti { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DatumIspita.Date<=DateTime.Now.Date)
                yield return new ValidationResult("Datum ispita mora biti u buducnosti.");
        }
    }
}