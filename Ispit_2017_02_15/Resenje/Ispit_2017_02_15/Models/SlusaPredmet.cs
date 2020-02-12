using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit_2017_02_15.Models
{
    public class SlusaPredmet
    {
        public int Id { get; set; }
        public DateTime? DatumOcjene { get; set; }
        public int? Ocjena { get; set; }

        [ForeignKey(nameof(Angazovan))]
        public int AngazovanId { get; set; }
        public virtual Angazovan Angazovan { get; set; }

        [ForeignKey(nameof(UpisGodine))]
        public int UpisGodineId { get; set; }
        public virtual UpisGodine UpisGodine { get; set; }

        public virtual ICollection<OdrzaniCasDetalji> Casovi { get; set; }
    }
}
