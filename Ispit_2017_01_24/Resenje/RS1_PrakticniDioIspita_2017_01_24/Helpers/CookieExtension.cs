using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RS1_PrakticniDioIspita_2017_01_24.Helpers
{
    public static class CookieExtension
    {
        public static T GetCookieJson<T>(this HttpRequest reqeuest, string key)
        {
            string strValue = reqeuest.Cookies[key];

            return strValue == null?default(T):JsonConvert.DeserializeObject<T>(strValue);
        }


        public static void SetCookieJson(this HttpResponse response, string key, string value, int? expireTime = null)
        {
            if (value == null)
            {
                response.Cookies.Delete(key);
                return;
            }

            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddDays(7);
            }

            string strValue = JsonConvert.SerializeObject(value);

            response.Cookies.Append(key,strValue,option);
        }

        public static void RemoveCookie(this HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }
    }
}