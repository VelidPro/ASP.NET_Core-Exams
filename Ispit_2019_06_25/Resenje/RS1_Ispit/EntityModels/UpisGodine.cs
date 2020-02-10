using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class UpisGodine
    {
        public int Id { get; set; }
        public int PolozioECTS { get; set; }
        public int SlusaECTS { get; set; }
        public DateTime DatumUpisa { get; set; }
        public int GodinaStudija { get; set; }

        public virtual Student Student { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }

        public virtual AkademskaGodina AkademskaGodina { get; set; }
        [ForeignKey(nameof(AkademskaGodina))]
        public int AkademskaGodinaId { get; set; }
    }
}
