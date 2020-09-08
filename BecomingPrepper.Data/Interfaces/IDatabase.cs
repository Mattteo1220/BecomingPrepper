using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IDatabase
    {
        MongoClient Connect(IConfiguration configuration, string environment);
        bool IsAlive(MongoClient mongoClient, string database);
    }
}
