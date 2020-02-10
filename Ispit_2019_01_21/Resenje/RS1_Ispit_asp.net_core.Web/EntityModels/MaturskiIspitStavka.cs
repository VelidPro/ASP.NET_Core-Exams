using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIspitStavka
    {
        public int Id { get; set; }
        public bool IsPristupio { get; set; }
        public int? OsvojeniBodovi { get; set; }

        [ForeignKey(nameof(MaturskiIspit))]
        public int MaturskiIspitId { get; set; }
        public MaturskiIspit MaturskiIspit { get; set; }

        [ForeignKey(nameof(Ucenik))]
        public int UcenikId { get; set; }
        public Ucenik Ucenik { get; set; }

    }
}