using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ispit_2017_02_15.ViewModels
{
    public class OdrzaniCasInputVM:IValidatableObject
    {
        public int? Id { get; set; }
        public string Nastavnik { get; set; }

        [Required]
        public DateTime Datum { get; set; }
        [Display(Name="Skolska godina / Predmet")]
        public int? AngazujeId { get; set; }
        [Display(Name = "Skolska godina / Predmet")]
        public string Angazman { get; set; }

        public List<SelectListItem> AkademskeGodinePredmeti { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Datum<=DateTime.Now)
                yield return new ValidationResult("Datum mora biti u buducnosti.");
        }
    }


}
