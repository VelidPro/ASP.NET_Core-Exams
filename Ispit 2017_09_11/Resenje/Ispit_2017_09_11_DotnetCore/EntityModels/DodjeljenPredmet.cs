using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit_2017_09_11_DotnetCore.EntityModels
{
    public class DodjeljenPredmet
    {
        public int Id { get; set; }

        public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        [ForeignKey(nameof(OdjeljenjeStavka))]
        public int OdjeljenjeStavkaId { get; set; }

        public virtual Predmet Predmet { get; set; }
        [ForeignKey(nameof(Predmet))]
        public int PredmetId { get; set; }
    
        public int ZakljucnoPolugodiste { get; set; }
        public int ZakljucnoKrajGodine { get; set; }
    }
}
