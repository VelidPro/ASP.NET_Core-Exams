using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class OdjeljenjeDetaljiVM
    {
        public string Id { get; set; }

        [Display(Name="Skolska godina")]
        public string SkolskaGodina { get; set; }

        public int Razred { get; set; }
        public string Oznaka { get; set; }
        public string Razrednik { get; set; }

        [Display(Name="Broj predmeta")]
        public int BrojPredmeta { get; set; }
        public List<OdjeljenjeStavkaVM> Ucenici { get; set; }
    }

    public class OdjeljenjeStavkaVM
    {
        public string Id { get; set; }
        public int BrojUDnevniku { get; set; }
        public string Ucenik { get; set; }
        public int BrojZakljucnihKrajGodine { get; set; }
    }
}