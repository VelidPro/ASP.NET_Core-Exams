using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIspit
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }


        [ForeignKey(nameof(PredmetID))]
        public Predmet Predmet { get; set; }
        public int PredmetID { get; set; }


        [ForeignKey(nameof(OdjeljenjeID))]
        public Odjeljenje Odjeljenje { get; set; }
        public int OdjeljenjeID { get; set; }

        [ForeignKey(nameof(NastavnikID))]
        public Nastavnik Nastavnik { get; set; }
        public int NastavnikID { get; set; }

    }
}
