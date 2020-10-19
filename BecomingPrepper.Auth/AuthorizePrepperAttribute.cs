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
            var loginRepository = context.HttpContext.RequestServices.GetService<IRepository<Login>>();

            context.HttpContext.Request.Cookies.TryGetValue("AccountId", out var accountId);
            context.HttpContext.Request.Cookies.TryGetValue("Email", out var email);
            var filter = Builders<Login>.Filter.Where(l => l.AccountId == accountId);

            //fetch token from database
            var login = loginRepository.Get(filter);
            if (login == null)
            {
                logManager.LogInformation($"Unauthorized attempt to access Service");
                context.Result = new UnauthorizedResult();
            }

            //check if auth is expired, if expired, check last login stamp. If last login stamp is greater than 8 hours, return unauthorized - redirect to login page
            var isAuthorized = tokenManager.IsAuthorized(login?.AccessToken);
            var eightHoursAgo = DateTime.Now.AddHours(-8);
            if (isAuthorized && tokenManager.IsTokenExpired && login?.LoginStamp <= eightHoursAgo)
            {
                logManager.LogInformation($"Expired Token and LoginStamp is greater than 8 Hours; Redirecting to login page.");
                context.HttpContext.Response.Headers.Add("User-ReAuthentication", "ReAuthentication needed");
                context.Result = new UnauthorizedResult();
            }
            //if expired but loginStamp was less than 8 hours, refresh access token and return
            if (isAuthorized && tokenManager.IsTokenExpired && login?.LoginStamp >= eightHoursAgo)
            {
                var newAccessToken = tokenManager.Generate(accountId, email);
                var updatedLogin = login;
                updatedLogin.LoginStamp = DateTime.Today.AddMinutes(15);
                updatedLogin.AccessToken = newAccessToken;
                filter = Builders<Login>.Filter.Where(l => l.AccountId == accountId);
                var updateFilter = Builders<Login>.Update.Set(l => l, updatedLogin);
                loginRepository.Update(filter, updateFilter);
            }
            //if not expired, return
        }
    }
}
