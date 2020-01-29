using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIspitStavka
    {
        public int Id { get; set; }

        public bool PristupiIspitu { get; set; }
        public bool Osloboden { get; set; }

        public float Rezultat { get; set; }

        [ForeignKey(nameof(MaturskiIspitID))]
        public MaturskiIspit MaturskiIspit { get; set; }
        public int MaturskiIspitID { get; set; }


        [ForeignKey(nameof(OdjeljenjeStavkaID))]
        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        public int OdjeljenjeStavkaID { get; set; }
    }
}
