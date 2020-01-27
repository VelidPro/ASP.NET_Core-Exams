using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PredmetiVM
    {
        //U stringu ce se cuvati naziv predmeta, a u listi predmeti po akademskim godinama
        public Dictionary<string,List<AngazovanVM>> Predmeti { get; set; }
    }

    public class AngazovanVM
    {
        public string Id { get; set; }
        public string AkademskaGodina { get; set; }
        public string Nastavnik { get; set; }
        public int BrojOdrzanihCasova { get; set; }
        public int BrojStudenataNaPredmetu { get; set; }

    }
}