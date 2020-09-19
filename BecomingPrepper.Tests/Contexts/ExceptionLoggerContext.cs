using System;
using BecomingPrepper.Logger;
using Serilog;

namespace BecomingPrepper.Tests.Contexts
{
    public class ExceptionLoggerContext
    {
        public ILogger Logger { get; set; }
        public IExceptionLogger ExceptionLogger { get; set; }
        public Exception Exception { get; set; }
    }
}
