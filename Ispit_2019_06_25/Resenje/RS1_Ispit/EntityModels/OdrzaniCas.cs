using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class OdrzaniCas
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }

        public Angazovan Angazovani { get; set; }
        [ForeignKey(nameof(Angazovani))]
        public int AngazovaniId { get; set; }


    }
}