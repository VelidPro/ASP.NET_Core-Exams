using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class Takmicenje
    {
        public int Id { get; set; }
        public int Razred { get; set; }
        public DateTime DatumOdrzavanja { get; set; }
        public ICollection<TakmicenjeUcesnik> Ucesnici { get; set; }

        public int BrojPrijavljenih { get; set; }
        public int BrojKojiNisuPristupili { get; set; }
        public bool IsEvidentiraniRezultati { get; set; }

        [ForeignKey(nameof(SkolaDomacin))]
        public int SkolaDomacinId { get; set; }
        public Skola SkolaDomacin { get; set; }

        [ForeignKey(nameof(Predmet))]
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }

    }
}