using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class OdrzaniCasStavka
    {
        public int Id { get; set; }

        private int ocjena;
        public int Ocjena
        {
            get => ocjena;
            set
            {
                if (IsPrisutan)
                    ocjena = value;
            }
        }

        public bool IsPrisutan { get; set; }
        public bool OpravdanoOdsustvo { get; set; }
        public string Napomena { get; set; }

        [ForeignKey(nameof(OdrzaniCas))]
        public int OdrzaniCasId { get; set; }
        public OdrzaniCas OdrzaniCas { get; set; }

        [ForeignKey(nameof(OdjeljenjeStavka))]
        public int OdjeljenjeStavkaId { get; set; }
        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }
    }
}