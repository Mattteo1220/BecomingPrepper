using System;
using Serilog;

namespace BecomingPrepper.Logger
{
    public class DataLogger
    {
        private Serilog.Core.Logger _log;
        private const string Collection = "DataLogCollection";

        public DataLogger(ILogConfig logConfig)
        {
            var container = new ComponentRegistration();
            container.Register();
            _log = new LoggerConfiguration()
                .WriteTo.MongoDB(logConfig.MongoClientConnectionString, collectionName: Collection, period: TimeSpan.Zero)
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        public void LogException(string thrownInLocation, string message, string stackTrace)
        {
            _log.Error($"Data Error: Exception thrown in '{thrownInLocation}'; Message: {message}; Stack: {stackTrace}");
        }
    }
}
