using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS1_Ispit_2017_06_21_v1.HelpModels;
using RS1_Ispit_2017_06_21_v1.Models;

namespace RS1_Ispit_2017_06_21_v1.Interfaces
{
    public interface IMaturskiIspitService
    {
        double GetProsjekBodova(int maturskiIspitId);

        Task<ServiceResult> Dodaj(MaturskiIspit ispit);
    }
}
