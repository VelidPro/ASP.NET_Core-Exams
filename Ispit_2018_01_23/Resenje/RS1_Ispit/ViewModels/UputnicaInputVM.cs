using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.KeyVault.Models;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class UputnicaInputVM:IValidatableObject
    {
        [Required(ErrorMessage = "Morate odabrati ljekara.")]
        [Display(Name="Uputio ljekar")]
        public string LjekarUputioId { get; set; }

        [Required(ErrorMessage = "Morate odabrati datum uputnice.")]
        [Display(Name = "Datum uputnice")]
        [DataType(DataType.Date)]
        public DateTime DatumUputnice { get; set; }

        [Required]
        [Display(Name = "Pacijent")]
        public string PacijentId { get; set; }

        [Required(ErrorMessage="Morate odabrati vrstu pretrage.")]
        [Display(Name = "Vrsta pretrage")]
        [Range(minimum:1,int.MaxValue)]
        public int VrstaPretrageId { get; set; }

        public List<SelectListItem> Ljekari { get; set; }
        public List<SelectListItem> Pacijenti { get; set; }
        public List<SelectListItem> VrstePretrage { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DatumUputnice.Date < DateTime.Now.Date)
                yield return new ValidationResult("Datum uputnice ne moze biti u proslosti.");

            if(PacijentId == string.Empty)
                yield return  new ValidationResult("Morate odabrati pacijenta.");


            if (LjekarUputioId == string.Empty)
                yield return new ValidationResult("Morate odabrati ljekara .");
        }
    }
}