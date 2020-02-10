using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{

    public class OdjeljenjeStavka
    {
        public int Id { get; set; }
        public int BrojUDnevniku { get; set; }

        public virtual Ucenik Ucenik { get; set; }
        public int UcenikId { get; set; }

        public virtual Odjeljenje Odjeljenje { get; set; }
        [ForeignKey(nameof(Odjeljenje))]
        public int OdjeljenjeId { get; set; }


    }
}
