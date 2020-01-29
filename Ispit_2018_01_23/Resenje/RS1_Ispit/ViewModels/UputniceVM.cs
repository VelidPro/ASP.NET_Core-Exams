using System;
using System.Collections.Generic;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
    public class UputniceVM
    {
        public List<UputnicaVM> Uputnice { get; set; }
    }

    public class UputnicaVM
    {
        public string Id { get; set; }
        public string UputioLjekar { get; set; }
        public string Pacijent { get; set; }
        public string VrstaPretraga { get; set; }
        public DateTime DatumUputnice { get; set; }
        public DateTime? DatumEvidentiranjaRezultataPretrage { get; set; }

    }
}