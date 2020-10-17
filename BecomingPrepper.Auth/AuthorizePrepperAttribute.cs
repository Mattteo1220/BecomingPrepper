using BecomingPrepper.Api.Authentication;
using BecomingPrepper.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BecomingPrepper.Auth
{
    public class AuthorizePrepperAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenExists = context.HttpContext.Request.Cookies.TryGetValue("Token", out var token);
            var tokenManager = context.HttpContext.RequestServices.GetService<ITokenManager>();
            var logManager = context.HttpContext.RequestServices.GetService<ILogManager>();
            if (!tokenManager.Authorize(token))
            {
                logManager.LogInformation($"UnAuthorized Attempt to access Service with accountId {tokenManager.AccountIdUsedForAuthorization}");
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
