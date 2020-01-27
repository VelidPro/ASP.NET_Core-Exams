using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ispit.Data.EntityModels
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        public string ImePrezime { get; set; }


        [ForeignKey(nameof(KorisnickiNalog))]
        public int KorisnickiNalogId { get; set; }
        public KorisnickiNalog KorisnickiNalog { get; set; }

        public ICollection<OznacenDogadjaj> OznaceniDogadjaji { get; set; }
    }
}
