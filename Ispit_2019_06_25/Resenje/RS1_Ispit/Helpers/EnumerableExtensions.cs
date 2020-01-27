using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_asp.net_core.Helpers
{
    public static class EnumerableExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> sequence, Func<T,string> value, Func<T,string> text, string defaultOption="" )
        {
            if(!sequence.Any())
                return new List<SelectListItem>{new SelectListItem{Value=string.Empty,Text=defaultOption}};

            var items = sequence.Select(x => new SelectListItem
            {
                Text=text(x),
                Value = value(x)
            }).ToList();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                items.Insert(0,new SelectListItem
                {
                    Value=string.Empty,
                    Text=defaultOption
                });
            }


            return items;

        }
    }
}