using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit_2017_09_11_DotnetCore.EntityModels
{
   
    public class OdjeljenjeStavka
    {
        public int Id { get; set; }

        public virtual Ucenik Ucenik { get; set; }
        [ForeignKey(nameof(Ucenik))]
        public int UcenikId { get; set; }

        public virtual Odjeljenje Odjeljenje { get; set; }
        [ForeignKey(nameof(Odjeljenje))]
        public int OdjeljenjeId { get; set; }

        public int BrojUDnevniku { get; set; }

    }
}
