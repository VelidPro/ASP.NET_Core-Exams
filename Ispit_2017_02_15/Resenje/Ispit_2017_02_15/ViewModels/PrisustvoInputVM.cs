using System.ComponentModel.DataAnnotations;

namespace Ispit_2017_02_15.ViewModels
{
    public class PrisustvoInputVM
    {
        public int Id { get; set; }
        public string Student { get; set; }

        [Range(0,100,ErrorMessage = "Broj bodova mora biti izmedju 0 i 100")]
        [Required(ErrorMessage = "Morate unijeti broj bodova.")]
        public int Bodovi { get; set; }
    }
}