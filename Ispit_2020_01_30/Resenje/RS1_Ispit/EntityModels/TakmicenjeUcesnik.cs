using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore.Storage;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class TakmicenjeUcesnik
    {
        public int  Id { get; set; }
        public bool IsPristupio { get; set; }
        public int OsvojeniBodovi { get; set; }


        [ForeignKey(nameof(OdjeljenjeStavka))]
        public int OdjeljenjeStavkaId { get; set; }
        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }

        [ForeignKey(nameof(Takmicenje))]
        public int TakmicenjeId { get; set; }
        public Takmicenje Takmicenje { get; set; }



    }
}