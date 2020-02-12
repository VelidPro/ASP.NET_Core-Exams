using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit_2017_02_15.Models
{
    public class OdrzaniCas
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }


        [ForeignKey(nameof(Angazovan))]
        public int AngazovanId { get; set; }
        public virtual Angazovan Angazovan { get; set; }

        public virtual ICollection<OdrzaniCasDetalji> OdrzaniCasDetaljii { get; set; }
    }
}
