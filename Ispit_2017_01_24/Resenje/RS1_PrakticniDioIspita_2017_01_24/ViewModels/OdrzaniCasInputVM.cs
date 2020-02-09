using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_PrakticniDioIspita_2017_01_24.ViewModels
{
    public class OdrzaniCasInputVM
    {
        public int? Id { get; set; }
        public string Nastavnik { get; set; }

        [Required(ErrorMessage = "Morate unijeti datum odrzanog casa.")]
        [DataType(DataType.Date)]
        public DateTime DatumOdrzanogCasa { get; set; }

        [Required(ErrorMessage = "Morate odabrati odjeljenje/predmet.")]
        [Display(Name = "Odjeljenje/Predmet")]
        public int AngazovanId { get; set; }
        public List<SelectListItem> OdjeljenjaPredmeti { get; set; }
    }
}