using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspitStavkaInputVM:IValidatableObject
    {
        public int? Id { get; set; }
        public int PopravniIspitId { get; set; }
        public int UcenikId { get; set; }
        public List<SelectListItem> Ucenici { get; set; }

        public string Ucenik { get; set; }
        public int Bodovi { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           if(Bodovi>100 || Bodovi<0)
               yield return new ValidationResult("Bodovi moraju biti izmedju 0 i 100.");
        }
    }
}