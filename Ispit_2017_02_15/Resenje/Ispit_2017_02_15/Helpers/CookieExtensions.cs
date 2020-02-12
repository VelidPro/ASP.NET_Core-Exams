using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Ispit_2017_02_15.Helpers
{
    public static class CookieExtensions
    {
        public static T GetCookieJson<T>(this HttpRequest request, string key)
        {
            var strValue = request.Cookies[key];
            return strValue == null ? default(T) : JsonConvert.DeserializeObject<T>(strValue);
        }

        public static void SetCookieJson(this HttpResponse response, string key, object value, int? expireTime = null)
        {
            if (value == null)
            {
                response.Cookies.Delete(key);
                return;
            }

            CookieOptions options = new CookieOptions();

            if (expireTime.HasValue)
            {
                options.Expires=DateTime.Now.AddMinutes(expireTime.Value);
            }
            else
            {
                options.Expires=DateTime.Now.AddDays(7);
            }

            var strValue = JsonConvert.SerializeObject(value);
            response.Cookies.Append(key,strValue,options);
        }

        public static void RemoveCookie(this HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }
    }
}
