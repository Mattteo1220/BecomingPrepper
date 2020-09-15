using BecomingPrepper.Logger;

namespace BecomingPrepper.Web.Models
{
    public class LogConfig : ILogConfig
    {
        public string MongoClientConnectionString { get; set; }
    }
}
