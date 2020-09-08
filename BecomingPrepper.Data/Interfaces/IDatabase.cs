using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IDatabase
    {
        MongoClient Connect(IConfiguration configuration, string dbConnectionString);
        bool IsAlive(MongoClient mongoClient, string dbConnectionString);
    }
}
