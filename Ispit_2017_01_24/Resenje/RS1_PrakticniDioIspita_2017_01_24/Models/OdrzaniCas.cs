using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_PrakticniDioIspita_2017_01_24.Models
{
    public class OdrzaniCas
    {
        public int Id { get; set; }
        public DateTime datum { get; set; }

        public int AngazovanId { get; set; }
        [ForeignKey(nameof(AngazovanId))]
        public Angazovan Angazovan { get; set; }
    }
}
