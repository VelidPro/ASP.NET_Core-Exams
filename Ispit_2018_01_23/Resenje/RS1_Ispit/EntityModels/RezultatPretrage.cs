using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1.Ispit.Web.Models
{
    public class RezultatPretrage
    {
        public int Id { get; set; }
        public virtual Uputnica Uputnica { get; set; }
        public int UputnicaId { get; set; }
        public double? NumerickaVrijednost { get; set; }


        public LabPretraga LabPretraga { get; set; }
        [ForeignKey(nameof(LabPretraga))]
        public int LabPretragaId  { get; set; }

        [ForeignKey(nameof(Modalitet))]
        public int? ModalitetId { get; set; }
        public Modalitet Modalitet { get; set; }

    }
}
