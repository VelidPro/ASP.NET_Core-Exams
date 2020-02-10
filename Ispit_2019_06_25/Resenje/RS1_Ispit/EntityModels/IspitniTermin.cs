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

        [ForeignKey(nameof(Angazovan))]
        public int AngazovanId { get; set; }
        public Angazovan Angazovan { get; set; }

        public ICollection<IspitPolaganje> Polaganja { get; set; }


    }
}