using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit.Data.EntityModels
{
    public class Dogadjaj
    {
        [Key]
        public int ID { get; set; }
        public string Opis{ get; set; }
        public DateTime DatumOdrzavanja { get; set; }

        [ForeignKey(nameof(Nastavnik))]
        public int? NastavnikID { get; set; }
        public Nastavnik Nastavnik { get; set; }

        public ICollection<Obaveza> Obaveze { get; set; }
    }
}
