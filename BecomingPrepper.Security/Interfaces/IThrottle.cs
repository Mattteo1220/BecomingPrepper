namespace BecomingPrepper.Security.Interfaces
{
    public interface IThrottle
    {
        bool ShouldRequestBeThrottled();
    }
}
