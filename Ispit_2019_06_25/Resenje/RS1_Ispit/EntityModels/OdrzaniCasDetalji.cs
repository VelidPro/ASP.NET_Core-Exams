using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class OdrzaniCasDetalji
    {
        public int Id { get; set; }
        public bool Prisutan { get; set; }
        public int BodoviNaCasu { get; set; }

        public OdrzaniCas OdrzaniCasovi { get; set; }
        [ForeignKey(nameof(OdrzaniCasovi))]
        public int OdrzaniCasoviId { get; set; }

        public SlusaPredmet SlusaPredmete { get; set; }
        [ForeignKey(nameof(SlusaPredmete))]
        public int SlusaPredmeteId { get; set; }


    }
}