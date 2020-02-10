using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
   
    public class OdjeljenjeStavka
    {
        public int Id { get; set; }
        public int BrojUDnevniku { get; set; }
        public ICollection<DodjeljenPredmet> DodjeljeniPredmeti { get; set; }


        public virtual Ucenik Ucenik { get; set; }
        [ForeignKey(nameof(Ucenik))]
        public int UcenikId { get; set; }


        public virtual Odjeljenje Odjeljenje { get; set; }
        [ForeignKey(nameof(OdjeljenjeId))]
        public int OdjeljenjeId { get; set; }


    }
}
