using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.Helpers;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class NoviIspitniTerminVM
    {
        public string AngazmanId { get; set; }

        public string Predmet { get; set; }
        public string Nastavnik { get; set; }

        [Display(Name="Skolska godina")]
        public string SkolskaGodina { get; set; }

        [Required(ErrorMessage = "Morate unijeti datum ispita.")]
        [FutureDateTime]
        public DateTime Datum { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(150,ErrorMessage = "Napomena ne smije sadrzati vise od 150 karaktera.")]
        public string Napomena { get; set; }

    }
}