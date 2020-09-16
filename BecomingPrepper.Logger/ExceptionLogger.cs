using System;
using Serilog;

namespace BecomingPrepper.Logger
{
    public class ExceptionLogger : IExceptionLogger
    {
        private ILogger _logger;
        public ExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void LogError(Exception exception)
        {

            _logger.Error(exception, exception.Message);
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogWarning(Exception exception)
        {
            _logger.Warning(exception, exception.Message);
        }
    }
}
