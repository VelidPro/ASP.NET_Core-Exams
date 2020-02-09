using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RS1_Ispit_2017_06_21_v1.EF;
using RS1_Ispit_2017_06_21_v1.Models;
using SQLitePCL;

namespace RS1_Ispit_2017_06_21_v1.Helpers
{
    public static class Authentication
    {
        private const string LoggedInUser = "LoggedInUser";

        public static async Task SetLoggedInUser(this HttpContext context, User user, bool saveInCookie = false)
        {
            var _dbContext = context.RequestServices.GetService<MojContext>();

            string oldToken = context.Request.GetCookieJson<string>(LoggedInUser);

            if (!string.IsNullOrEmpty(oldToken))
            {
                var tokenToDelete = await _dbContext.AuthorizationTokens
                    .FirstOrDefaultAsync(x => x.Value == oldToken);

                if (tokenToDelete != null)
                {
                    _dbContext.Remove(tokenToDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (user != null)
            {
                var token = Guid.NewGuid().ToString();

                await _dbContext.AddAsync(new AuthorizationToken
                {
                    Value=token,
                    UserId = user.Id,
                    DateCreated = DateTime.Now
                });
                await _dbContext.SaveChangesAsync();

                context.Session.Set(LoggedInUser, token);

                if (saveInCookie)
                {
                    context.Response.SetCookieJson(LoggedInUser,token);
                }
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


        public static async Task LogoutUser(this HttpContext context)
        {
            var user =await GetLoggedInUser(context);

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
