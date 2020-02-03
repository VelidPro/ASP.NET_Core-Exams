using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class RezultatPretragePopravnihVM
    {
        public int PredmetId { get; set; }
        public string Predmet { get; set; }

        public int SkolskaGodinaId { get; set; }
        public string SkolskaGodina { get; set; }

        public int SkolaId { get; set; }
        public string Skola { get; set; }

        public List<PopravniIspitVM> PopravniIspiti { get; set; }
    }

    public class PopravniIspitVM
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Komisija { get; set; }
        public int BrojUcenika  { get; set; }
        public int BrojKojiNisuPolozili { get; set; }
    }
}