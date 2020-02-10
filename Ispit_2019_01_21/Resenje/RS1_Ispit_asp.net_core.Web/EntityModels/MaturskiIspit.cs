using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIspit
    {
        public int Id { get; set; }
        public DateTime DatumOdrzavanja { get; set; }
        public string Napomena { get; set; }

        [ForeignKey(nameof(Skola))]
        public int SkolaId { get; set; }
        public Skola Skola { get; set; }

        [ForeignKey(nameof(SkolskaGodina))]
        public int SkolskaGodinaId { get; set; }
        public SkolskaGodina SkolskaGodina { get; set; }

        [ForeignKey(nameof(Nastavnik))]
        public int NastavnikId { get; set; }
        public Nastavnik Nastavnik { get; set; }

        [ForeignKey(nameof(Predmet))]
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }

        public virtual ICollection<MaturskiIspitStavka> PrijavljeniUcenici { get; set; }
    }
}