using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class RezultatEditVM
    {
        public int TakmicenjeUcesnikId { get; set; }
        public string Ucesnik { get; set; }
        [Required(ErrorMessage = "Morate unijeti broj osvojenih bodova.")]
        [Display(Name="Bodovi")]
        public int OsvojeniBodovi { get; set; }
    }
}