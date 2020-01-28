using System.ComponentModel.DataAnnotations;

namespace eUniverzitet.Web.ViewModels
{
    public class StanjeObavezeInputVM
    {
        public string Id { get; set; }
        public string Obaveza { get; set; }
        [Display(Name="Zavrseno procentualno")]
        [Range(0,100)]
        public float ZavrsenoProcentualno { get; set; }
        [Display(Name="Notificiraj dana prije")]
        public int NotificirajDanaUnaprijed { get; set; }
        [Display(Name="Notificiraj svaki dan")]
        public bool NotificirajRekurzivno { get; set; }
    }
}