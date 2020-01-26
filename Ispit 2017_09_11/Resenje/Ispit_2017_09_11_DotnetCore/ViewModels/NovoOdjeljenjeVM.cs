using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class NovoOdjeljenjeVM: IValidatableObject
    {
        [Display(Name="Skolska godina")]
        [Required(ErrorMessage = "Skolska godina je obavezno polje")]
        public string SkolskaGodina { get; set; }

        [Display(Name="Razred")]
        [Required(ErrorMessage = "Morate odabrati razred za odjeljenje")]
        [Range(1,4,ErrorMessage="Razred moze biti vrijednost od 1 do 4")]
        public int Razred { get; set; }

        [Display(Name = "Oznaka")]
        [Required(ErrorMessage = "Oznaka odjeljenja je obavezna")]
        public string Oznaka { get; set; }

        [Required(ErrorMessage = "Morate odabrati razrednika")]
        [Display(Name = "Razrednik")]
        public string RazrednikId { get; set; }

        [Display(Name = "Preuzmi ucenike iz nizeg odeljenja")]
        public string OdjeljenjeId { get; set; }

        public List<SelectListItem> Razrednici { get; set; }
        public List<SelectListItem> Odjeljenja { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(int.Parse(SkolskaGodina.Substring(0,4)) < DateTime.Now.Year)
                yield return new ValidationResult("Skolska godina mora biti veca od trenutne trenutne godine.");
        }
    }
}