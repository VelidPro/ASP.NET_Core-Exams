using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RS1_Ispit_2017_06_21_v1.Helpers
{
    public static class EnumerableExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value,
            Func<T, string> text, string defaultOption = "", int? defaultId = null)
        {
            if(!enumerable.Any())
                return new List<SelectListItem>{new SelectListItem
                {
                    Value=string.Empty,
                    Text=defaultOption
                }};

            var items = enumerable.Select(x => new SelectListItem
            {
                Value=value(x),
                Text=text(x)
            }).ToList();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                items.Insert(0,new SelectListItem{Value = string.Empty,Text=defaultOption});
            }

            if (defaultId.HasValue)
            {
                var temp = items.FirstOrDefault(x => x.Value == defaultId.Value.ToString());

                if (temp != null)
                    temp.Selected = true;
            }

            return items;
        }


        public static double AverageOrZero<T>(this IEnumerable<T> enumerable, Func<T,float> selector)
        {
            return enumerable.Any() ? enumerable.Average(selector) : 0;
        }
    }
}