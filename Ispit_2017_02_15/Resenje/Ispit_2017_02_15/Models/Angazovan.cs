using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ispit_2017_02_15.Models
{
    public class Angazovan
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Nastavnik))]
        public int NastavnikId { get; set; }
        public virtual Nastavnik Nastavnik { get; set; }

        [ForeignKey(nameof(AkademskaGodina))]
        public int AkademskaGodinaId { get; set; }
        public virtual AkademskaGodina AkademskaGodina{ get; set; }

        [ForeignKey(nameof(Predmet))]
        public int PredmetId { get; set; }
        public virtual Predmet Predmet { get; set; }
    }
}
