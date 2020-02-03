using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class NastavniciPredavaciVM
    {
        public List<NastavnikVM> Nastavnici { get; set; }
    }

    public class NastavnikVM
    {
        public int Id { get; set; }
        public string Skola { get; set; }
        public string ImePrezime { get; set; }
    }
}