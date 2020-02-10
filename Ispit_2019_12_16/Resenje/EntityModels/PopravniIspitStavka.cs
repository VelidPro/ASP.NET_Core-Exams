using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIspitStavka
    {
        public int Id { get; set; }
        public int? OsvojeniBodovi { get; set; }
        public bool IsPrisutupio { get; set; }
        public bool ImaPravoNaIzlazask { get; set; }

        [ForeignKey(nameof(PopravniIspit))]
        public int PopravniIspitId { get; set; }
        public PopravniIspit PopravniIspit { get; set; }

        [ForeignKey(nameof(Ucenik))]
        public int UcenikId { get; set; }
        public Ucenik Ucenik { get; set; }
    }
}