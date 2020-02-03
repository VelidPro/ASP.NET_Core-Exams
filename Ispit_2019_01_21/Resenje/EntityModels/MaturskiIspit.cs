using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIspit
    {
        public int Id { get; set; }
        public DateTime DatumOdrzavanja { get; set; }
        public string Napomena { get; set; }

        public int SkolaId { get; set; }
        [ForeignKey(nameof(SkolaId))] 
        public Skola Skola { get; set; }

        public int NastavnikId { get; set; }
        [ForeignKey(nameof(NastavnikId))]
        public Nastavnik Nastavnik { get; set; }

        public int PredmetId { get; set; }
        [ForeignKey(nameof(PredmetId))]
        public Predmet Predmet { get; set; }

        public int SkolskaGodinaId { get; set; }
        [ForeignKey(nameof(SkolskaGodinaId))]
        public SkolskaGodina SkolskaGodina { get; set; }

        public ICollection<MaturskiIspitStavka> Ucenici { get; set; }
    }
}