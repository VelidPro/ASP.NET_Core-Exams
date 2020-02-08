using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_PrakticniDioIspita_2017_01_24.Models
{
    public class Angazovan
    {
        public int Id { get; set; }

        public int NastavnikId { get; set; }
        [ForeignKey(nameof(NastavnikId))]
        public Nastavnik Nastavnik { get; set; }

        public int PredmetId { get; set; }
        [ForeignKey(nameof(PredmetId))]
        public Predmet Predmet { get; set; }

        public int OdjeljenjeId { get; set; }
        [ForeignKey(nameof(OdjeljenjeId))]
        public Odjeljenje Odjeljenje { get; set; }

        public ICollection<OdrzaniCas> OdrzaniCasovi { get; set; }
    }
}
