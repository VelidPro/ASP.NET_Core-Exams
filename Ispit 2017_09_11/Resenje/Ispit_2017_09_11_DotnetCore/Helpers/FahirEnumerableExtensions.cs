using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ispit_2017_09_11_DotnetCore.Helpers
{
    public static class FahirEnumerableExtensions
    {
        public static double AverageOrZero<T>(this IEnumerable<T> sequence, Func<T, double> filter)
        {
            return sequence.Any() ? sequence.Average(filter) : 0;
        }


        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value,
            Func<T, string> text, string defaultOption="")
        {
            if(!enumerable.Any())
                return new List<SelectListItem>{new SelectListItem{Value = string.Empty,Text=defaultOption}};

            var items = enumerable.Select(x => new SelectListItem
            {
                Text = text(x),
                Value = value(x)
            }).ToList();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                items.Insert(0,new SelectListItem
                {
                    Text=defaultOption,
                    Value=string.Empty

                });
            }

            return items;
        }
    }
}