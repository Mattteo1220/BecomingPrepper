using System;
using BecomingPrepper.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Data
{
    public class Database : IDatabase
    {
        public MongoClient Connect(IConfiguration configuration, string environment)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(environment)) throw new ArgumentNullException(nameof(environment));

            var mongoClientConnectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection($"Database").GetSection(environment).Value;

            try
            {
                var mongoClient = new MongoClient(mongoClientConnectionString);
                return mongoClient;
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
    }
}
