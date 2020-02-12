using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_2017_06_21_v1.ViewModels
{
    public class MaturskiIspitStavkaInputVM
    {
        public int Id { get; set; }
        public string Ucenik { get; set; }

        [Required]
        [Range(0,100)]
        public float Bodovi { get; set; }
    }
}