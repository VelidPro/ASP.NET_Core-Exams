using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ispit_2017_09_11_DotnetCore.Helpers
{
    public static class EnumerableExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> sequence, Func<T, string> value,
            Func<T, string> text, string defaultOption="",int defaultId=0)
        {
            if (!sequence.Any())
                return new List<SelectListItem>
                {
                    new SelectListItem {Value = string.Empty, Text = defaultOption}
                };

            var items = sequence.Select(x => new SelectListItem
            {
                Text = text(x),
                Value = value(x)
            }).ToList();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                items.Insert(0, new SelectListItem {Value = string.Empty, Text = defaultOption});
            }

            if (defaultId != 0)
            {
                if (items.Any(x => x.Value == defaultId.ToString()))
                    items.FirstOrDefault(x => x.Value == defaultId.ToString()).Selected = true;
            }

            return items;
        }
    }
}