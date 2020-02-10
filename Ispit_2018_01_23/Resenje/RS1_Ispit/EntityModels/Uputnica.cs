using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1.Ispit.Web.Models
{
    public class Uputnica
    {
        public int Id { get; set; }
        public DateTime DatumUputnice { get; set; }
        public DateTime? DatumRezultata { get; set; }
        public bool IsGotovNalaz { get; set; }


        public Ljekar UputioLjekar{ get; set; }
        [ForeignKey(nameof(UputioLjekar))]
        public int UputioLjekarId { get; set; }

        public Ljekar LaboratorijLjekar { get; set; }
        [ForeignKey(nameof(LaboratorijLjekar))]
        public int? LaboratorijLjekarId { get; set; }

        public Pacijent Pacijent { get; set; }
        [ForeignKey(nameof(Pacijent))]
        public int PacijentId { get; set; }

        public VrstaPretrage VrstaPretrage { get; set; }
        [ForeignKey(nameof(VrstaPretrage))]
        public int VrstaPretrageId { get; set; }

    }
}
