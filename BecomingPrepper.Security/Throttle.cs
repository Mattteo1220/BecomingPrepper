using System;
using BecomingPrepper.Security.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace BecomingPrepper.Security
{
    public class Throttle : IThrottle
    {
        public IMemoryCache MemoryCache { get; set; }
        private readonly int _requestLimit;
        private readonly string _key;
        private readonly int _timeoutInSeconds;
        public static int RequestsRemaining { get; private set; }
        public static DateTime RequestResetDate { get; private set; }

        public Throttle(string key, int requestLimit, int timeoutInSeconds)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (requestLimit <= 0) throw new ArgumentOutOfRangeException(nameof(requestLimit));
            if (timeoutInSeconds <= 0) throw new ArgumentOutOfRangeException(nameof(timeoutInSeconds));
            _key = key;
            _requestLimit = requestLimit;
            _timeoutInSeconds = timeoutInSeconds;
        }

        public bool ShouldRequestBeThrottled()
        {
            var cache = MemoryCache.TryGetValue(_key, out ThrottleInfo throttleInfo);

            if (!cache || throttleInfo.ExpiresAt <= DateTime.Now)
            {
                throttleInfo = new ThrottleInfo()
                {
                    ExpiresAt = DateTime.Now.AddSeconds(_timeoutInSeconds),
                    RequestCount = 0
                };

                MemoryCache.Set(_key, throttleInfo);
            };
            RequestResetDate = throttleInfo.ExpiresAt;
            RequestsRemaining = Math.Max(_requestLimit - throttleInfo.RequestCount, 0);

            throttleInfo.RequestCount++;
            return (throttleInfo.RequestCount > _requestLimit);

        }

        public class ThrottleInfo
        {
            public DateTime ExpiresAt { get; set; }
            public int RequestCount { get; set; }
        }
    }
}
