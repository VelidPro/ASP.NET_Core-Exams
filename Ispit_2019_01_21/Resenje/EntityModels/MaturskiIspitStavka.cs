using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIspitStavka
    {
        public int Id { get; set; }
        public int MaturskiIspitId { get; set; }
        public bool IsPristupio { get; set; }
        public int? OsvojeniBodovi { get; set; }



        [ForeignKey(nameof(MaturskiIspitId))]
        public MaturskiIspit MaturskiIspit { get; set; }

        public int UcenikId { get; set; }
        [ForeignKey(nameof(UcenikId))]
        public Ucenik Ucenik { get; set; }

    }
}