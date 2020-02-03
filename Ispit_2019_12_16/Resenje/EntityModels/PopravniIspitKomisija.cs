using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIspitKomisija
    {

        public int Id { get; set; }

        public int NastavnikId { get; set; }
        [ForeignKey(nameof(NastavnikId))]
        public Nastavnik Nastavnik { get; set; }

        public int PopravniIspitId { get; set; }
        [ForeignKey(nameof(PopravniIspitId))]
        public PopravniIspit PopravniIspit { get; set; }
    }
}