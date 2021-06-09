using System;
using System.Globalization;
using BecomingPrepper.Data.Entities.Endpoint;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Security.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;

namespace BecomingPrepper.Security
{
    public class ThrottleFilter : ResultFilterAttribute
    {
        private readonly Endpoint _endPoint;
        public ThrottleFilter(Endpoint endpoint)
        {
            _endPoint = endpoint;
        }

        public void OnActionExecuted(ActionExecutedContext executedContext)
        {
            throw new NotImplementedException();
        }

        public override void OnResultExecuting(ResultExecutingContext executingContext)
        {
            var endpointRepository = executingContext.HttpContext.RequestServices.GetService<IRepository<EndpointEntity>>();
            var filter = Builders<EndpointEntity>.Filter.Where(e => e.Id == Convert.ToInt32(_endPoint));
            var endpoint = endpointRepository.Get(filter);
            var throttle = new Throttle(endpoint);
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
