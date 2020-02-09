using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_2017_06_21_v1.Models
{
    public class AuthorizationToken
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateCreated { get; set; }
        
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        
    }
}
