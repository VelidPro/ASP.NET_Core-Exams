using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class NoviIspitniTerminVM: IValidatableObject
    {
        public string AngazmanId { get; set; }

        public string Predmet { get; set; }
        public string Nastavnik { get; set; }
        [Display(Name="Skolska godina")]
        public string SkolskaGodina { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Morate unijeti datum ispita.")]
        public DateTime Datum { get; set; }

        [DataType(DataType.Text)]
        public string Napomena { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
            //if(Datum <= DateTime.Now)
            //    yield return  new ValidationResult("Datum ispita mora biti u buducnosti.");
        }
    }
}