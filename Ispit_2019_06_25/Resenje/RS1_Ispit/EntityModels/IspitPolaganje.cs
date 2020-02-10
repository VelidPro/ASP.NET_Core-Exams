using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class IspitPolaganje
    {
        public int Id { get; set; }
        public bool PristupioIspitu { get; set; }
        public int? Ocjena { get; set; }


        [ForeignKey(nameof(IspitniTermin))]
        public int IspitniTerminId { get; set; }
        public IspitniTermin IspitniTermin { get; set; }

        [ForeignKey(nameof(UpisGodine))]
        public int UpisGodineId { get; set; }
        public UpisGodine UpisGodine { get; set; }

    }
}