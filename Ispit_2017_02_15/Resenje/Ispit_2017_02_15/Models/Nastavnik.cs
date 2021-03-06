﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ispit_2017_02_15.Models
{
    public class Nastavnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        public string ImePrezime() => Ime + " " + Prezime;
    }
}
