using System.ComponentModel.DataAnnotations.Schema;

namespace RS1.Ispit.Web.Models
{
    public class Modalitet
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public bool IsReferentnaVrijednost { get; set; }

        public LabPretraga LabPretraga { get; set; }
        [ForeignKey(nameof(LabPretraga))]
        public int LabPretragaId  { get; set; }
    }
}
