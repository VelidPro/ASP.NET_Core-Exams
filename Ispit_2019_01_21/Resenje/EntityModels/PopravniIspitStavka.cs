using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIspitStavka
    {
        public int Id { get; set; }
        public int? OsvojeniBodovi { get; set; }
        public bool IsPrisutupio { get; set; }

        public bool ImaPravoNaIzlazask { get; set; }
        public int PopravniIspitId { get; set; }
        [ForeignKey(nameof(PopravniIspitId))]
        public PopravniIspit PopravniIspit { get; set; }

        public int UcenikId { get; set; }
        [ForeignKey(nameof(UcenikId))]
        public Ucenik Ucenik { get; set; }
    }
}