using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}