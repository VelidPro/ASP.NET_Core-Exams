using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_PrakticniDioIspita_2017_01_24.Models
{
    public class OdrzaniCasDetalj
    {
        public int Id { get; set; }
        public bool Odsutan { get; set; }
        public int? Ocjena { get; set; }
        public bool? OpravdanoOdsutan { get; set; }

        public int OdrzaniCasId { get; set; }
        [ForeignKey(nameof(OdrzaniCasId))]
        public OdrzaniCas OdrzaniCas { get; set; }

        public int UpisUOdjeljenjeId { get; set; }
        [ForeignKey(nameof(UpisUOdjeljenjeId))]
        public UpisUOdjeljenje UpisUOdjeljenje { get; set; }
    }
}
