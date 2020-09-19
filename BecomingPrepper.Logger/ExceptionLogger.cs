using System;
using Serilog;

namespace BecomingPrepper.Logger
{
    public class ExceptionLogger : IExceptionLogger
    {
        private ILogger _logger;
        public ExceptionLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogError(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            try
            {
                _logger.Error(exception, exception.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void LogInformation(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _logger.Information(message);
        }

        public void LogWarning(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            _logger.Warning(exception, exception.Message);
        }
    }
}
