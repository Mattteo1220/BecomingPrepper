using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IDatabase
    {
        IMongoDatabase MongoDatabase { get; set; }
        MongoClient Connect(IConfiguration configuration, string environment);
        bool IsAlive(MongoClient mongoClient, string database);
        IMongoDatabase GetDatabase(MongoClient mongoClient, string database);
    }
}
