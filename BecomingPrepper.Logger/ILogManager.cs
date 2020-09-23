using System;

namespace BecomingPrepper.Logger
{
    public interface ILogManager
    {
        void LogError(Exception exception);

        void LogInformation(string message);

        void LogWarning(Exception exception);
    }
}