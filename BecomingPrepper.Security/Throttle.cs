using System;
using BecomingPrepper.Data.Entities.Endpoint;
using BecomingPrepper.Security.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace BecomingPrepper.Security
{
    public class Throttle : IThrottle
    {
        public IMemoryCache MemoryCache { get; set; }
        private readonly EndpointEntity _endpoint;
        public static int RequestsRemaining { get; private set; }
        public static DateTime RequestResetDate { get; private set; }

        public Throttle(EndpointEntity endpoint)
        {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

            if (endpoint.RequestLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(endpoint.RequestLimit));
            }

            if (endpoint.Timeout <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(endpoint.Timeout));
            }
        }

        public bool ShouldRequestBeThrottled()
        {
            var cache = MemoryCache.TryGetValue(_endpoint.Id, out ThrottleInfo throttleInfo);

            if (!cache || throttleInfo.ExpiresAt <= DateTime.Now)
            {
                throttleInfo = new ThrottleInfo()
                {
                    ExpiresAt = DateTime.Now.AddSeconds(_endpoint.Timeout),
                    RequestCount = 0
                };

                MemoryCache.Set(_endpoint.Id, throttleInfo);
            };
            RequestResetDate = throttleInfo.ExpiresAt;
            RequestsRemaining = Math.Max(_endpoint.RequestLimit - throttleInfo.RequestCount, 0);

            throttleInfo.RequestCount++;
            return (throttleInfo.RequestCount > _endpoint.RequestLimit);

        }

        public class ThrottleInfo
        {
            public DateTime ExpiresAt { get; set; }
            public int RequestCount { get; set; }
        }
    }
}
