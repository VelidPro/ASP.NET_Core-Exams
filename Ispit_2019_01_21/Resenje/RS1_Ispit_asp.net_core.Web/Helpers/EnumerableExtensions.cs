using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace RS1_Ispit_asp.net_core.Helpers
{
    public static class EnumerableExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T,string> value,
            Func<T,string> text,string defaultOption="",int selectedId = 0)
        {
            if(!enumerable.Any())
                return new List<SelectListItem>{new SelectListItem{Value=string.Empty,Text=defaultOption}};

            var items = enumerable.Select(x => new SelectListItem
            {
                Value=value(x),
                Text=text(x)
            }).ToList();

            if(!string.IsNullOrEmpty(defaultOption))
                items.Insert(0,new SelectListItem{Value = string.Empty,Text=defaultOption});

            if (selectedId != 0)
            {
                var temp = items.FirstOrDefault(x => x.Value == selectedId.ToString());

                if (temp != null)
                    temp.Selected = true;
            }

            return items;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }


        public static async Task<double> AverageOrZeroAsync<T>(this IQueryable<T> queryable, Expression<Func<T, double>> valueSelector)
        {
            return await queryable.AnyAsync() ? await queryable.AverageAsync(valueSelector) : 1;
        }
    }
}
