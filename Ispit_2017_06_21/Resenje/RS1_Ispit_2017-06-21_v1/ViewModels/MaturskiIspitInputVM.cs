using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1_Ispit_2017_06_21_v1.Helpers;

namespace RS1_Ispit_2017_06_21_v1.ViewModels
{
    public class MaturskiIspitInputVM:IValidatableObject
    {
        public string Ispitivac { get; set; }
        [Required]
        [FutureDateTime]
        public DateTime Datum { get; set; }
        [Required]
        public int OdjeljenjeId { get; set; }
        public List<SelectListItem> Odjeljenja { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Datum.Date<=DateTime.Now.Date)
                yield return new ValidationResult("Datum mora biti u buducnosti.");
        }
    }
}