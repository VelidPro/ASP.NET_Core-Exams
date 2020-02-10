using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class MaturskiIspitDetaljiVM
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Predmet { get; set; }
        public string Napomena { get; set; }
        public List<MaturskiIspitStavkaVM> PrijavljeniUcenici { get; set; }
    }

    public class MaturskiIspitStavkaVM
    {
        public int Id { get; set; }
        public string Ucenik { get; set; }
        public double ProsjekOcjena { get; set; }
        public bool IsPristupio { get; set; }
        public int? OsvojioBodova { get; set; }
    }
}