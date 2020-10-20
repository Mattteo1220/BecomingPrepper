using System;
using BecomingPrepper.Api.Authentication;
using BecomingPrepper.Data.Entities.Logins;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BecomingPrepper.Auth
{
    public class AuthorizePrepperAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenManager = context.HttpContext.RequestServices.GetService<ITokenManager>();
            var logManager = context.HttpContext.RequestServices.GetService<ILogManager>();
            var loginDataRepository = context.HttpContext.RequestServices.GetService<IRepository<Login>>();

            context.HttpContext.Request.Cookies.TryGetValue("AccountId", out var accountId);
            context.HttpContext.Request.Cookies.TryGetValue("Email", out var email);
            var filter = Builders<Login>.Filter.Where(l => l.AccountId == accountId);

            //fetch token from database
            var login = loginDataRepository.Get(filter);
            if (login == null)
            {
                logManager.LogInformation($"Unauthorized attempt to access Service");
                context.Result = new UnauthorizedResult();
            }

            //Check to see if AccountId exists in DB, if it doesn't. Then Return UnAuthorized
            var isAuthorized = tokenManager.IsAuthorized(login?.AccessToken);

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }

            //check if token is expired
            var localLoginStamp = DateTime.Parse(login?.LoginStamp.ToLocalTime().ToString("G") ?? string.Empty, null, System.Globalization.DateTimeStyles.RoundtripKind);
            if (tokenManager.IsTokenExpired)
            {
                var eightHoursAgo = DateTime.Now.AddHours(-8);
                if (localLoginStamp <= eightHoursAgo)
                {
                    //if expired, check last login stamp. If last login stamp is greater than 8 hours ago, return unauthorized - redirect to login page
                    logManager.LogInformation($"Expired Token and LoginStamp is greater than 8 Hours; Redirecting to login page.");
                    context.HttpContext.Response.Headers.Add("User-ReAuthentication", "True");
                    context.Result = new UnauthorizedResult();
                }

                if (localLoginStamp >= eightHoursAgo)
                {
                    //if expired but loginStamp was less than 8 hours, refresh access token and return
                    var newAccessToken = tokenManager.Generate(accountId, email);
                    filter = Builders<Login>.Filter.Where(l => l.AccountId == accountId);
                    var updateFilter = Builders<Login>.Update.Combine(Builders<Login>.Update.Set(l => l.AccessToken, newAccessToken), Builders<Login>.Update.Set(l => l.LoginStamp, DateTime.Now.AddMinutes(15)));
                    loginDataRepository.Update(filter, updateFilter);
                }
            }
        }
    }
}
