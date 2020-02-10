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


        public Predmet Predmet { get; set; }
        [ForeignKey(nameof(Predmet))]
        public int PredmetID { get; set; }


        public Odjeljenje Odjeljenje { get; set; }
        [ForeignKey(nameof(Odjeljenje))]
        public int OdjeljenjeID { get; set; }

        public Nastavnik Nastavnik { get; set; }
        [ForeignKey(nameof(Nastavnik))]
        public int NastavnikID { get; set; }

    }
}
