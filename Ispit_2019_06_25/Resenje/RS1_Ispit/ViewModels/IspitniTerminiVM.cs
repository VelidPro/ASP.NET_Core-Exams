using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class IspitniTerminiVM
    {
        public string AngazmanId { get; set; }
        public string Predmet { get; set; }
        public string Nastavnik { get; set; }

        [Display(Name = "Akademska godina")]
        public string AkademskaGodina { get; set; }

        public List<IspitniTerminVM> IspitniTermini { get; set; }
    }

    public class IspitniTerminVM
    {
        public string Id { get; set; }
        public DateTime DatumIspita { get; set; }
        public int BrojStudenataNepolozeno { get; set; }
        public int BrojPrijavljenihStudenata { get; set; }
        public bool EvidentiraniRazultati { get; set; }

    }

}