using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_PrakticniDioIspita_2017_01_24.Models
{
    public class Odjeljenje
    {
        public int Id { get; set; }
        public string Oznaka { get; set; }
        public int Razred { get; set; }

        public int NastavnikId { get; set; }
        [ForeignKey(nameof(NastavnikId))]
        public Nastavnik Nastavnik { get; set; }

    }
}
