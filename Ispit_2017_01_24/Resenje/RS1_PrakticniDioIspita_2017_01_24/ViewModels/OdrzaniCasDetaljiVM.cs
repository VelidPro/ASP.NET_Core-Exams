using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_PrakticniDioIspita_2017_01_24.ViewModels
{
    public class OdrzaniCasEditVM
    {
        public int Id { get; set; }
        public DateTime DatumOdrzavanja { get; set; }
        public int AngazovanId { get; set; }
        public List<SelectListItem> OdjeljenjaPredmeti { get; set; }
        public List<OdrzaniCasDetaljEditVM> Prisustva { get; set; }
    }

    public class OdrzaniCasDetaljEditVM
    {
        public int Id { get; set; }
        public string Ucenik { get; set; }
        public int? Ocjena { get; set; }
        public bool Odsutan { get; set; }
        public bool? OpravdanoOdsutan { get; set; }
    }
}