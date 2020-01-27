using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class IspitniTerminDetaljiVM
    {
        public string Id { get; set; }

        public string Predmet { get; set; }

        [Display(Name="Skolska godina")]
        public string SkolskaGodina { get; set; }

        public string Nastavnik { get; set; }

        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Display(Name="Sadrzaj ispita")]
        public string Napomena { get; set; }
        public bool Zakljucan { get; set; }


        public List<PolaganjeIspitaVM> Polaganja { get; set; }
    }

    public class PolaganjeIspitaVM
    {
        public string Id { get; set; }
        public string Student { get; set; }
        public bool PristupioIspitu { get; set; }
        public int? Ocjena { get; set; }
    }

}