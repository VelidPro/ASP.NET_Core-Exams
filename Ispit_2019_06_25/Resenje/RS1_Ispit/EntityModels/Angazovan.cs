using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class Angazovan
    {
        public int Id { get; set; }

        public virtual Nastavnik Nastavnik { get; set; }
        [ForeignKey(nameof(Nastavnik))]
        public int NastavnikId { get; set; }

        public virtual AkademskaGodina AkademskaGodina{ get; set; }
        [ForeignKey(nameof(AkademskaGodina))]
        public int AkademskaGodinaId { get; set; }

        public virtual Predmet Predmet { get; set; }
        [ForeignKey(nameof(Predmet))]
        public int PredmetId { get; set; }
    }
}
