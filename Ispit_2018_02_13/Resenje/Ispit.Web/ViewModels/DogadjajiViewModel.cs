using System;
using System.Collections.Generic;

namespace eUniverzitet.Web.ViewModels
{
    public class DogadjajiViewModel
    {
        public List<DogadjajVM> OznaceniDogadjaji { get; set; }
        public List<DogadjajVM> NeoznaceniDogadjaji { get; set; }
    }

    public class DogadjajVM
    {
        public string DogadjajId { get; set; }
        public string Opis { get; set; }
        public DateTime DatumDogadjaja { get; set; }
        public string Nastavnik { get; set; }
        public int? BrojObaveza { get; set; }

        public string OznaceniDogadjajId { get; set; }
        public int? RealizovanoObaveza { get; set; }
    }
}