using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_PrakticniDioIspita_2017_01_24.Models
{
    public class UpisUOdjeljenje
    {
        public int Id { get; set; }
        public int BrojUDnevniku { get; set; }

        public int UcenikId { get; set; }
        [ForeignKey(nameof(UcenikId))]
        public Ucenik Ucenik { get; set; }

        public int OdjeljenjeId { get; set; }
        [ForeignKey(nameof(OdjeljenjeId))]
        public Odjeljenje Odjeljenje { get; set; }

    }
}
