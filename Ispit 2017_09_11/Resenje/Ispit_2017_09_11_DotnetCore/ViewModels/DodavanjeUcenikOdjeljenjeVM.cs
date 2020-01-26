using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class DodavanjeUcenikOdjeljenjeVM
    {
        public string OdjeljenjeId { get; set; }

        [Display(Name="Broj u dnevniku")]
        [Required(ErrorMessage = "Morate unijeti broj u dnevniku.")]
        [Range(1,40)]
        public int BrojUDnevniku { get; set; }

        [Display(Name="Ucenik")]
        [Required(ErrorMessage = "Morate odabrati ucenika.")]
        public string UcenikId { get; set; }

        public List<SelectListItem> Ucenici { get; set; }
    }
}