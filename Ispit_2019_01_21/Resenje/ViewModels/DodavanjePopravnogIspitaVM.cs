using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodavanjePopravnogIspitaVM: IValidatableObject
    {
        public int PredmetId { get; set; }
        public int SkolaId { get; set; }
        public int SkolskaGodinaId { get; set; }

        public List<int> ClanoviKomisijaIds { get; set; }
        public List<SelectListItem> ClanoviKomisije { get; set; }

        public DateTime DatumIspita { get; set; }
        public string Skola { get; set; }
        public string SkolskaGodina { get; set; }
        public string Predmet { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var x in ClanoviKomisijaIds)
            {
                if(x<=0)
                    yield return new ValidationResult("Morate odabrati 3 clana komisije.");
            }
            if (ClanoviKomisijaIds.Count != ClanoviKomisijaIds.Distinct().Count())
            {
                yield return new ValidationResult("Morate odabrati 3 razlicita clana komisije.");
            }

            if (DatumIspita.Date<=DateTime.Now.Date)
                yield return new ValidationResult("Datum popravnog ispita mora biti u buducnosti");
        }
    }
}