using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class IspitPolaganje
    {
        public int Id { get; set; }

        public int IspitniTerminId { get; set; }
        [ForeignKey(nameof(IspitniTerminId))] 
        public IspitniTermin IspitniTermin { get; set; }

        public int UpisGodineId { get; set; }
        [ForeignKey(nameof(UpisGodineId))]
        public UpisGodine UpisGodine { get; set; }

        public bool PristupioIspitu { get; set; }
        public int? Ocjena { get; set; }
    }
}