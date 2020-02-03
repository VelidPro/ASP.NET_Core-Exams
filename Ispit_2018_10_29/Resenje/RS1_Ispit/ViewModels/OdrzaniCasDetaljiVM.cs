using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class OdrzaniCasDetaljiVM
    {
        public int OdrzaniCasId { get; set; }
        public DateTime Datum { get; set; }

        public string Odjeljenje { get; set; }
        public string Napomena { get; set; }
        public List<OdrzaniCasStavkaVM> Prisustva { get; set; }
    }

    public class OdrzaniCasStavkaVM{
        public int Id { get; set; }
        public string Ucenik { get; set; }
        public int Ocjena { get; set; }
        public bool IsPrisutan { get; set; }
        public bool OpravdanoOdsutan { get; set; }
    }
}