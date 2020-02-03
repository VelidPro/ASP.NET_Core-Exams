using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.HelpModels;

namespace RS1_Ispit_asp.net_core.Interfaces
{
   public interface IPopravniIspitService
   {
       Task<int> BrojUcenikaNisuPolozili(int popravniIspitId);
       Task<int> BrojUcenikaPolozili(int popravniIspitId);

       Task<ServiceResult> Dodaj(PopravniIspit popravniIspit,List<int> komisijaIds);
   }
}
