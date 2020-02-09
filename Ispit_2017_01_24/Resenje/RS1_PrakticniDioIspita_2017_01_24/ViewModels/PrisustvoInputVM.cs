using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RS1_PrakticniDioIspita_2017_01_24.ViewModels
{
    public class PrisustvoInputVM
    {
        public int Id { get; set; }
        public string Ucenik { get; set; }

        [Range(1,5)]
        public int? Ocjena { get; set; }

        [Display(Name = "Opravdano odsutan?")]
        public bool OpravdanoOdsutan { get; set; }

        public bool Odsutan { get; set; }
    }
}