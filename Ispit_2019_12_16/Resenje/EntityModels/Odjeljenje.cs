using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class Odjeljenje
    {
        public int Id { get; set; }
        public int Razred { get; set; }
        public string Oznaka { get; set; }
        public bool IsPrebacenuViseOdjeljenje { get; set; }


        public virtual Skola Skola { get; set; }
        [ForeignKey(nameof(Skola))]
        public int SkolaID { get; set; }


        public virtual SkolskaGodina SkolskaGodina { get; set; }
        [ForeignKey(nameof(SkolskaGodina))]
        public int SkolskaGodinaID { get; set; }


        public virtual Nastavnik Razrednik { get; set; }
        [ForeignKey(nameof(Razrednik))]
        public int RazrednikID { get; set; }


    }
}
