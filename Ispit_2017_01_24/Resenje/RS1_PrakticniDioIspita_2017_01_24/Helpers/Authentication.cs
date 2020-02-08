using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RS1_PrakticniDioIspita_2017_01_24.EF;
using RS1_PrakticniDioIspita_2017_01_24.Models;

namespace RS1_PrakticniDioIspita_2017_01_24.Helpers
{
    public static class Authentication
    {
        public const string LogiraniKorisnik = "logirani_korisnik";

        public static async Task SetLoggedInUser(this HttpContext context, User user, bool saveInCookie = false)
        {
            var _dbContext = context.RequestServices.GetService<MojContext>();

            string oldToken = context.Request.GetCookieJson<string>(LogiraniKorisnik);

            if (!string.IsNullOrEmpty(oldToken))
            {
                var tokenToDelete = await _dbContext.AuthorizationTokens.FirstOrDefaultAsync(x => x.Value == oldToken);

                if (tokenToDelete != null)
                {
                    _dbContext.AuthorizationTokens.Remove(tokenToDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }

            var oldToken2 = context.Session.Get<string>(LogiraniKorisnik);

            if (!string.IsNullOrEmpty(oldToken2))
            {
                var tokenToDelete = await _dbContext.AuthorizationTokens
                    .FirstOrDefaultAsync(x => x.Value == oldToken2);

                if (tokenToDelete != null)
                {
                    _dbContext.AuthorizationTokens.Remove(tokenToDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (user != null)
            {
                var token = Guid.NewGuid().ToString();
                await _dbContext.AuthorizationTokens.AddAsync(new AuthorizationToken
                {
                    Value = token,
                    UserId = user.Id,
                    DateCreated = DateTime.Now
                });

                await _dbContext.SaveChangesAsync();

                context.Session.Set<string>(LogiraniKorisnik,token);
                if (saveInCookie)
                    context.Response.SetCookieJson(LogiraniKorisnik, token);
            }
        }


        public static string GetCurrentAuthToken(this HttpContext context)
        {
            var token = context.Session.Get<string>(LogiraniKorisnik);

            if (string.IsNullOrEmpty(token))
                token = context.Request.GetCookieJson<string>(LogiraniKorisnik);

            return token;
        }


        public static async Task<User> GetLoggedInUser(this HttpContext context)
        {
            var _dbContext = context.RequestServices.GetService<MojContext>();

            var token = GetCurrentAuthToken(context);

            if (string.IsNullOrEmpty(token))
                return null;

            return await _dbContext.AuthorizationTokens
                .Where(x => x.Value == token)
                .Select(x => x.User)
                .SingleOrDefaultAsync();
        }


        public static async Task RemoveAuthTokensCurrentUser(this HttpContext context, HttpResponse response)
        {
            var user = await context.GetLoggedInUser();

            if (user == null)
                return;

            var _dbContext = context.RequestServices.GetService<MojContext>();

            response.Cookies.Delete(LogiraniKorisnik);

            context.Session.Remove(LogiraniKorisnik);

            var authTokens = _dbContext.AuthorizationTokens.Where(x => x.UserId == user.Id);

            _dbContext.RemoveRange(authTokens);
            await _dbContext.SaveChangesAsync();
        }


    }
}