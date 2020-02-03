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

        public int OdrzaniCasId { get; set; }
        [ForeignKey(nameof(OdrzaniCasId))]
        public OdrzaniCas OdrzaniCas { get; set; }

        public int OdjeljenjeStavkaId { get; set; }
        [ForeignKey(nameof(OdjeljenjeStavkaId))]
        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }
    }
}