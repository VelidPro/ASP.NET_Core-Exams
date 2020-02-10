using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_PrakticniDioIspita_2017_01_24.Models
{
    public class Nastavnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId{ get; set; }
        public User User { get; set; }


    }
}
