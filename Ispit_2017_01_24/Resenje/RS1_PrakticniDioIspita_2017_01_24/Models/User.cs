using System.Collections.Generic;

namespace RS1_PrakticniDioIspita_2017_01_24.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<AuthorizationToken> AuthTokens { get; set; }

    }
}