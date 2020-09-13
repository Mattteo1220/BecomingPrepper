using System;
using BecomingPrepper.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Data
{
    public class Database : IDatabase
    {
        public IMongoDatabase MongoDatabase { get; set; }
        public MongoClient MongoClient { get; set; }
        public MongoClient Connect(IConfiguration configuration, string environment)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(environment)) throw new ArgumentNullException(nameof(environment));

            var mongoClientConnectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection($"Database").GetSection(environment).Value;

            try
            {
                MongoClient = new MongoClient(mongoClientConnectionString);
                if (IsAlive(this.MongoClient, database))
                {
                    MongoDatabase = GetDatabase(MongoClient, database);
                    return MongoClient;
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to connect to {database}" + e.StackTrace);
            }
        }

        public bool IsAlive(MongoClient mongoClient, string database)
        {
            if (mongoClient == null) throw new ArgumentNullException(nameof(mongoClient));
            if (string.IsNullOrWhiteSpace(database)) throw new ArgumentNullException(nameof(database));

            if (!mongoClient.GetDatabase(database).RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(10000))
            {
                throw new MongoClientException($"Failed to connect to {nameof(database)}");
            }
            return true;
        }

        public IMongoDatabase GetDatabase(MongoClient mongoClient, string database)
        {
            if (mongoClient == null) throw new ArgumentNullException(nameof(mongoClient));
            if (string.IsNullOrWhiteSpace(database)) throw new ArgumentNullException(nameof(database));

            return mongoClient.GetDatabase(database);
        }
    }
}
