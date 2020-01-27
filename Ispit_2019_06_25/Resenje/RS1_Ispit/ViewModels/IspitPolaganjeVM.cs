using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class IspitPolaganjeVM
    {
        public string Id { get; set; }
        public string IspitniTerminId { get; set; }
        public string Student { get; set; }

        [Range(5,10)]
        public int? Ocjena { get; set; }

        public string UpisGodineId { get; set; }
        public List<SelectListItem> Studenti { get; set; }
    }
}