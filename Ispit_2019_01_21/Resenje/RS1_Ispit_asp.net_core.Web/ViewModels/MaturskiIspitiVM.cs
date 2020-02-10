using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class MaturskiIspitiVM
    {
        public int NastavnikId { get; set; }
        public string Nastavnik { get; set; }
        public List<MaturskiIspitVM> MaturskiIspiti { get; set; }
    }

    public class MaturskiIspitVM
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Skola { get; set; }
        public string Predmet { get; set; }
        public List<string> UceniciNisuPristupili { get; set; }
    }
}