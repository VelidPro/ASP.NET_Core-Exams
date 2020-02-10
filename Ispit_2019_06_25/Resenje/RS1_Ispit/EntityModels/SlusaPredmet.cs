using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class SlusaPredmet
    {
        public int Id { get; set; }
        public DateTime? DatumOcjene { get; set; }
        public int? Ocjena { get; set; }


        public virtual Angazovan Angazovan { get; set; }
        [ForeignKey(nameof(Angazovan))]
        public int AngazovanId { get; set; }

        public virtual UpisGodine UpisGodine { get; set; }
        [ForeignKey(nameof(UpisGodine))]
        public int UpisGodineId { get; set; }
    }
}
