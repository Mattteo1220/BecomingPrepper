using BecomingPrepper.Auth.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BecomingPrepper.Auth
{
    public class AuthorizePrepperAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenManager = context.HttpContext.RequestServices.GetService<ITokenManager>();

            context.HttpContext.Request.Cookies.TryGetValue("Key", out var key);
            var token = context.HttpContext.Session.GetString(key);

            if (!tokenManager.IsAuthorized(token))
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                var refreshedToken = tokenManager.RefreshToken();
                context.HttpContext.Session.SetString(key, refreshedToken);
            }
        }
    }
}
