using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_2017_06_21_v1.Models
{
    public class MaturskiIspit
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }

        public int NastavnikId { get; set; }
        [ForeignKey(nameof(NastavnikId))]
        public Nastavnik Nastavnik { get; set; }

        public int OdjeljenjeId { get; set; }
        [ForeignKey(nameof(OdjeljenjeId))]
        public Odjeljenje Odjeljenje { get; set; }

        public virtual ICollection<MaturskiIspitStavka> MaturskiIspitStavke { get; set; }
    }
}