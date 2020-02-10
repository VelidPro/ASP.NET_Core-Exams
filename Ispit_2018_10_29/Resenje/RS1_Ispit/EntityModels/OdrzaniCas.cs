using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class OdrzaniCas
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Napomena { get; set; }

        [ForeignKey(nameof(Odjeljenje))]
        public int OdjeljenjeId { get; set; }
        public Odjeljenje Odjeljenje { get; set; }

        [ForeignKey(nameof(PredajePredmet))]
        public int PredajePredmetId { get; set; }
        public PredajePredmet PredajePredmet { get; set; }

        public ICollection<OdrzaniCasStavka> Prisustva { get; set; }
    }
}