using System.Collections.Generic;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class OdjeljenjaViewModel
    {
        public List<OdjeljenjeViewModel> Odjeljenja { get; set; }
    }

    public class OdjeljenjeViewModel
    {
        public string Id { get; set; }
        public string SkolskaGodina { get; set; }
        public string Oznaka { get; set; }
        public int Razred { get; set; }
        public string Nastavnik { get; set; }
        public bool PrebaceniUViseOdjeljenje { get; set; }
        public double ProsjekOcjena { get; set; }
        public string NajboljiUcenik { get; set; }
    }
}