using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;

namespace RS1_Ispit_asp.net_core.Test
{
    public class MojContextHelper
    {
        public static MojContext GetMojContext()
        {
            DbContextOptionsBuilder<MojContext> builder= 
                new DbContextOptionsBuilder<MojContext>()
                    .UseSqlServer("Server=.;Database=Ispit2019_01_21;Trusted_Connection=true;MultipleActiveResultSets=true;");

            return new MojContext(builder.Options);
        }
    }
}
