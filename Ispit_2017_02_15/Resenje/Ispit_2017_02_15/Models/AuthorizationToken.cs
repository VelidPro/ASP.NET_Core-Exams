using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_02_15.Models
{
    public class AuthorizationToken
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string IpAddress { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
