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

        public int BrojPrijavljenih { get; set; }
        public int BrojKojiNisuPristupili { get; set; }
        public bool IsEvidentiraniRezultati { get; set; }

        public int SkolaDomacinId { get; set; }
        [ForeignKey(nameof(SkolaDomacinId))]
        public Skola SkolaDomacin { get; set; }

        public int PredmetId { get; set; }
        [ForeignKey(nameof(PredmetId))]
        public Predmet Predmet { get; set; }

        public ICollection<TakmicenjeUcesnik> Ucesnici { get; set; }
    }
}