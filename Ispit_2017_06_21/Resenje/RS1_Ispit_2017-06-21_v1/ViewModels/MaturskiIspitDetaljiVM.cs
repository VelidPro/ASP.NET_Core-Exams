using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace RS1_Ispit_2017_06_21_v1.ViewModels
{
    public class MaturskiIspitDetaljiVM
    {
        public int Id { get; set; }
        public string Ispitivac { get; set; }
        public DateTime Datum { get; set; }
        public string Odjeljenje { get; set; }

        public List<MaturskiIspitStavkaVM> Polaganja { get; set; }
    }

    public class MaturskiIspitStavkaVM
    {
        public int Id { get; set; }
        public string Ucenik { get; set; }
        public int OpstiUspjeh { get; set; }
        public float? Bodovi { get; set; }
        public bool Oslobodjen { get; set; }
    }
}
