using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.KeyVault.Models;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjeCasaVM:IValidatableObject
    {
        public int NastavnikId { get; set; }
        public string Nastavnik { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name="Datum")]
        public DateTime DatumOdrzavanja { get; set; }

        [Required(ErrorMessage="Morate odabrati predmet i odjeljenje.")]
        [Display(Name = "Odjeljenje / Predmet")]
        public int PredajePredmetId { get; set; }

        public List<SelectListItem> OdjeljenjaPredmeti { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DatumOdrzavanja<=DateTime.Now)
                yield return new ValidationResult("Datum mora biti u buducnosti.");
        }
    }
}