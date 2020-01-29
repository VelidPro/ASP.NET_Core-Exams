using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1.Ispit.Web.Models;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class DetaljiUputniceVM
    {
        public string Id { get; set; }
        [Display(Name="Datum uputnice")]
        public DateTime DatumUputnice { get; set; }
        public string Pacijent { get; set; }
        [Display(Name = "Datum rezultata")]
        public DateTime? DatumRezultata { get; set; }
        [Display(Name="Zavrsen unos")]
        public bool IsZavrsenUnos { get; set; }
        public List<RezultatPretrageVM> RezultatiPretraga { get; set; }
    }

    public class RezultatPretrageVM
    {
        public string Id { get; set; }
        public string Pretraga { get; set; }
        public string JMJ { get; set; }
        public string IzmjerenaVrijednost { get; set; }

        public VrstaVrijednosti VrstaVrijednosti { get; set; }

        public int? ModalitetId { get;set; }
        public List<SelectListItem> Modaliteti { get; set; }

        public string ReferentnaVrijednost { get; set; }
        public bool IsReferentnaVrijednost { get; set; }
    }
}