using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace RS1_Ispit_asp.net_core.ViewComponents
{
    public class DetaljiPopravniIspitVM
    {
        public int Id { get; set; }
        public int SkolskaGodinaId { get; set; }
        public int SkolaId { get; set; }
        public int PredmetId { get; set; }

        [Display(Name="Skolska godina")]
        public string SkolskaGodina { get; set; }
        public string Skola { get; set; }
        public string Predmet { get; set; }

        public List<string> ClanoviKomisije { get; set; }

        [Display(Name="Datum ispita")]
        public DateTime DatumIspita { get; set; }

        public List<PopravniIspitStavkaVM> Polaganja { get; set; }

    }

    public class PopravniIspitStavkaVM
    {
        public int Id { get; set; }
        public string Ucenik { get; set; }
        public string Odjeljenje { get; set; }
        public int BrojUDnevniku { get; set; }
        public bool IsPristupio { get; set; }
        public int OsvojenoBodova { get; set; }
        public bool ImaPravoIzlaska { get; set; }

    }
}