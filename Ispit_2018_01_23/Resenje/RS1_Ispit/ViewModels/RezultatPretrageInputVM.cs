using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1.Ispit.Web.Models;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class RezultatPretrageInputVM
    {
        public string Id { get; set; }
        public string Pretraga { get; set; }
        public VrstaVrijednosti VrstaVrijednosti { get; set; }

        [Display(Name="Izmjerena vrijednost")]
        public double? IzmjerenaVrijednost { get; set; }
        public string JMJ { get; set; }

        [Display(Name="Vrijednost")]
        public int? ModalitetId { get; set; }
        public List<SelectListItem> Modaliteti { get; set; }
    }
}