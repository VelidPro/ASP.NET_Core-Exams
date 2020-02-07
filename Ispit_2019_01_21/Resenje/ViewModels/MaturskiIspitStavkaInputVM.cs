using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class MaturskiIspitStavkaInputVM:IValidatableObject
    {
        public int Id { get; set; }
        public string Ucenik { get; set; }

        [Required(ErrorMessage = "Morate unijeti broj bodova.")]
        [Range(1,100)]
        public int Bodovi { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Bodovi < 0 || Bodovi > 100)
                yield return new ValidationResult("Bodovi moraju biti izmedju 0 i 100");
        }
    }
}