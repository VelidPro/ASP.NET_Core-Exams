using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ispit_2017_02_15.Helpers
{
    public static class EnumerableExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value,
            Func<T, string> text, string defaultOption = "", int? defaultId = null)
        {
            if (!enumerable.Any())
            {
                return new List<SelectListItem>{new SelectListItem
                {
                    Value = string.Empty,
                    Text = defaultOption
                }};
            }

            var items = enumerable.Select(x => new SelectListItem
            {
                Value=value(x),
                Text=text(x)
            }).ToList();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                items.Insert(0, new SelectListItem
                {
                    Value=string.Empty,
                    Text=defaultOption
                });
            }

            if (defaultId.HasValue)
            {
                if (items.Any(x => x.Value == defaultId.Value.ToString()))
                {
                    items.FirstOrDefault(x => x.Value == defaultId.Value.ToString()).Selected = true;
                }
            }

            return items;
        }

        public static double AverageOrZero<T>(this IEnumerable<T> enumerable, Func<T, double> keySelector)
        {
            return enumerable.Any() ? enumerable.Average(keySelector) : 0;
        }

        public static IEnumerable<T> DistintBy<T,TKey>(this IEnumerable<T> enumerable, Func<T,TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp=>grp.First());
        }
    }
}