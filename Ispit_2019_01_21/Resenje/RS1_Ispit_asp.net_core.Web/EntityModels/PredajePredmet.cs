using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{

    public class PredajePredmet
    {
        public int Id { get; set; }

        public virtual Predmet Predmet { get; set; }
        [ForeignKey(nameof(Predmet))]
        public int PredmetID { get; set; }

        public virtual Odjeljenje Odjeljenje { get; set; }
        [ForeignKey(nameof(Odjeljenje))]
        public int OdjeljenjeID { get; set; }

        public virtual Nastavnik Nastavnik { get; set; }
        [ForeignKey(nameof(Nastavnik))]
        public int NastavnikID { get; set; }

    }
}
