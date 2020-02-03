using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.KeyVault.Models;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class UredjivanjePrisustvaVM
    {
        public int OdrzaniCasStavkaId { get; set; }
        public bool IsPrisutan { get; set; }

        [Display(Name="Ucenik")]
        public string UcenikGodina { get; set; }
        public int Ocjena { get; set; }

        [MaxLength(50,ErrorMessage="Maximalna duzina napomene je 50 karaktera.")]
        public string Napomena { get; set; }
        [Display(Name = "Opravdano odsutan.")]
        public bool OpravdanoOdsutan { get; set; }
    }
}