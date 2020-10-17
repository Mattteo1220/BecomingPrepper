using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace BecomingPrepper.Security
{
    public class ThrottleFilter : ResultFilterAttribute
    {
        private readonly string _key;
        private readonly int _requestLimit;
        private readonly int _timeoutInSeconds;
        public ThrottleFilter(string key, int requestLimit, int timeoutInSeconds)
        {
            _key = key;
            _requestLimit = requestLimit;
            _timeoutInSeconds = timeoutInSeconds;
        }

        public void OnActionExecuted(ActionExecutedContext executedContext)
        {
            throw new NotImplementedException();
        }

        public override void OnResultExecuting(ResultExecutingContext executingContext)
        {
            var throttle = new Throttle(_key, _requestLimit, _timeoutInSeconds);
            throttle.MemoryCache = executingContext.HttpContext.RequestServices.GetService<IMemoryCache>();
            if (throttle.ShouldRequestBeThrottled())
            {
                AddHeaders(executingContext);
                executingContext.Result = new StatusCodeResult(429);
            }
            AddHeaders(executingContext);
        }

        private void AddHeaders(ResultExecutingContext executingContext)
        {
            if (!executingContext.HttpContext.Response.Headers.TryGetValue("X-RateLimit-Reset",
                    out StringValues expiresAt)
                && !executingContext.HttpContext.Response.Headers.TryGetValue("X-RateLimit-Remaining",
                    out StringValues requestsRemaining))
            {
                executingContext.HttpContext.Response.Headers.Add("X-RateLimit-Reset", Throttle.RequestResetDate.ToString(CultureInfo.InvariantCulture));
                executingContext.HttpContext.Response.Headers.Add("X-RateLimit-Remaining", Throttle.RequestsRemaining.ToString());
            }

        }
    }
}
