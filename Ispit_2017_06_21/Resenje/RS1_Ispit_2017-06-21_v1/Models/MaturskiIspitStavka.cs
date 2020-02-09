using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_2017_06_21_v1.Models
{
    public class MaturskiIspitStavka
    {
        public int Id { get; set; }
        public float? Bodovi { get; set; }
        public bool Oslobodjen { get; set; }

        public int MaturskiIspitId { get; set; }
        [ForeignKey(nameof(MaturskiIspitId))]
        public MaturskiIspit MaturskiIspit { get; set; }

        public int UpisUOdjeljenjeId { get; set; }
        [ForeignKey(nameof(UpisUOdjeljenjeId))]
        public UpisUOdjeljenje UpisUOdjeljenje { get; set; }
    }
}