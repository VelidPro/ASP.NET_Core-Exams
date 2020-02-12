using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ispit_2017_02_15.EF;
using Ispit_2017_02_15.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ispit_2017_02_15.Helpers
{
    public static class Authentication
    {
        private const string LoggedInUser = "Current_User";

        public static async Task SetLoggedInUser(this HttpContext context,User user, bool saveInCookie=false)
        {
            if (user == null)
                return;

            var _dbContext = context.RequestServices.GetService<MojContext>();

            var oldToken = context.Request.GetCookieJson<string>(LoggedInUser);

            if (!string.IsNullOrEmpty(oldToken))
            {
                var tokenToDelete = await _dbContext.AuthorizationTokens.FirstOrDefaultAsync(x => x.Value == oldToken);

                if (tokenToDelete != null)
                {
                    _dbContext.Remove(tokenToDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }

            var token = Guid.NewGuid().ToString();

            await _dbContext.AddAsync(new AuthorizationToken
            {
                Value=token,
                IpAddress = context.Connection.RemoteIpAddress.ToString(),
                UserId = user.Id
            });
            await _dbContext.SaveChangesAsync();

            context.Session.Set(LoggedInUser,token);

            if (saveInCookie)
            {
                context.Response.SetCookieJson(LoggedInUser,token);
            }
        }


        public static string GetAuthTokenOfCurrentUser(this HttpContext context)
        {
            return context.Request.GetCookieJson<string>(LoggedInUser);
        }


        public static async Task<User> GetLoggedInUser(this HttpContext context)
        {
            var _dbContext = context.RequestServices.GetService<MojContext>();

            var token = context.Request.GetCookieJson<string>(LoggedInUser);

            if (token == null)
            {
                token = context.Session.Get<string>(LoggedInUser);

                if (token == null)
                    return null;
            }

            return await _dbContext.AuthorizationTokens
                .Where(x => x.Value == token)
                .Select(x => x.User)
                .SingleOrDefaultAsync();
        }

        public static async Task<bool> IsAuthenticated(this HttpContext context)
        {
            return await GetLoggedInUser(context) != null;
        }

        public static async Task LogoutUser(this HttpContext context)
        {
            var user = await GetLoggedInUser(context);

            if (user == null)
                return;

            context.Response.RemoveCookie(LoggedInUser);
            context.Session.Remove(LoggedInUser);

            var _dbContext = context.RequestServices.GetService<MojContext>();
            var tokens = _dbContext.AuthorizationTokens.Where(x => x.UserId == user.Id);

            _dbContext.RemoveRange(tokens);

            await _dbContext.SaveChangesAsync();
        }
    }
}
