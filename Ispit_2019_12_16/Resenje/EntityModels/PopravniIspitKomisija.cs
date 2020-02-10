using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIspitKomisija
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Nastavnik))]
        public int NastavnikId { get; set; }
        public Nastavnik Nastavnik { get; set; }

        [ForeignKey(nameof(PopravniIspit))]
        public int PopravniIspitId { get; set; }
        public PopravniIspit PopravniIspit { get; set; }
    }
}