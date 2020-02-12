using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit_2017_02_15.Models
{
    public class OdrzaniCasDetalji
    {
        public int Id { get; set; }
        public bool Prisutan { get; set; }
        public int? BodoviNaCasu { get; set; }


        [ForeignKey(nameof(OdrzaniCas))]
        public int OdrzaniCasId { get; set; }
        public virtual OdrzaniCas OdrzaniCas { get; set; }

        [ForeignKey(nameof(SlusaPredmet))]
        public int SlusaPredmetId { get; set; }
        public virtual SlusaPredmet SlusaPredmet { get; set; }
        
 

    }
}
