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

        public MaturskiIspit MaturskiIspit { get; set; }
        [ForeignKey(nameof(MaturskiIspit))]
        public int MaturskiIspitID { get; set; }


        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        [ForeignKey(nameof(OdjeljenjeStavka))]
        public int OdjeljenjeStavkaID { get; set; }
    }
}
