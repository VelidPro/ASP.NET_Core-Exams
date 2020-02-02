using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RS1_Ispit_asp.net_core.Helpers
{
    public static class EnumerableExtensions

    {

        public static double AverageOrZero<T>(this IEnumerable<T> enumerable, Func<T, double> filter)
        {
            return enumerable.Any() ?  enumerable.Average(filter) : 0;
        }

        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value,
            Func<T, string> text, int defaultId = 0, string defaultOption = "")
        {
            if(!enumerable.Any())
                return new List<SelectListItem>{new SelectListItem{Value = string.Empty,Text=defaultOption}};


            var items = enumerable.Select(x => new SelectListItem
            {
                Value = value(x),
                Text=text(x)
            }).ToList();

            if(!string.IsNullOrEmpty(defaultOption))
                items.Insert(0,new SelectListItem
                {
                    Value=string.Empty,
                    Text=defaultOption
                });


            if(defaultId>0)
                if (items.Any(x => x.Value == defaultId.ToString()))
                    items.FirstOrDefault(x => x.Value == defaultId.ToString()).Selected = true;

            return items;

        }

            public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
            {
                return enumerable.GroupBy(keySelector).Select(grp => grp.First());
            }
    }
}