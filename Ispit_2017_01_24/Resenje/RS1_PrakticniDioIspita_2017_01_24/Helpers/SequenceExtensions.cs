using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RS1_PrakticniDioIspita_2017_01_24.Helpers
{
    public static class SequenceExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable,
            Func<T, string> value, Func<T, string> text, string defaultOption = "", int defaultId = 0)
        {
            if (!enumerable.Any())
                return new List<SelectListItem> {new SelectListItem {Value = string.Empty, Text = defaultOption}};

            var items =  enumerable.Select(x => new SelectListItem
            {
                Value=value(x),
                Text=text(x)
            }).ToList();

            if(!string.IsNullOrEmpty(defaultOption))
                items.Insert(0,new SelectListItem{Value=string.Empty,Text=defaultOption});

            if (defaultId > 0)
            {
                var item =  items.FirstOrDefault(x => x.Value == defaultId.ToString());

                if (item != null)
                    item.Selected = true;
            }

            return items;
        }



        public static double AverageOrZero<T>(this IEnumerable<T> enumerable, Func<T, double> keySelector)
        {
            return enumerable.Any()?enumerable.Average(keySelector):0;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }
    }
}