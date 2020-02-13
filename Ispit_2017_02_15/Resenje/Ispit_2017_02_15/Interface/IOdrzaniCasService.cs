using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ispit_2017_02_15.Models;

namespace Ispit_2017_02_15.Interface
{
    public interface IOdrzaniCasService
    {
        Task<bool> Dodaj(OdrzaniCas cas);
        double GetProsjecnuOcjenu(int angazovanId);
    }
}
