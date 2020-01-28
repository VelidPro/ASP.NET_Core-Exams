using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eUniverzitet.Web.ViewModels
{
    public class DogadjajDetaljiVM
    {
        public string Id { get; set; }

        public string Opis { get; set; }
        [Display(Name="Datum dogadjaja")]
        public DateTime DatumDogadjaja { get; set; }

        [Display(Name="Datum dodavanja")]
        public DateTime DatumDodavanja { get; set; }

        public string Nastavnik { get; set; }

        public List<ObavezaVM> Obaveze { get; set; }
    }

    public class ObavezaVM
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public float ProcenatRealizacije { get; set; }
        public int NotificirajDanaUnapred { get; set; }
        public bool RekurzivnaNotifikacija { get; set; }
        
    }
}