using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class IspitniTermin
    {
        public int Id { get; set; }
        public DateTime DatumIspita { get; set; }
        public int BrojPrijavljenihStudenata { get; set; }
        public int BrojNepolozenih { get; set; }
        public bool EvidentiraniRezultati { get; set; }
        public string Napomena { get; set; }

        public int AngazovanId { get; set; }
        [ForeignKey(nameof(AngazovanId))] 
        public Angazovan Angazovan { get; set; }

        public ICollection<IspitPolaganje> Polaganja { get; set; }


    }
}