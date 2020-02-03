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
        public int OdjeljenjeId { get; set; }
        [ForeignKey(nameof(OdjeljenjeId))]
        public Odjeljenje Odjeljenje { get; set; }

        public int PredajePredmetId { get; set; }
        [ForeignKey(nameof(PredajePredmetId))]
        public PredajePredmet PredajePredmet { get; set; }

        public ICollection<OdrzaniCasStavka> Prisustva { get; set; }
    }
}