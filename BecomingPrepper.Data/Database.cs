using System;
using BecomingPrepper.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Data
{
    public class Database : IDatabase
    {
        public MongoClient Connect(IConfiguration configuration, string dbConnectionString)
        {
            var mongoClientConnectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection($"Databases").GetSection(dbConnectionString).Value;

            try
            {
                var mongoClient = new MongoClient(database);
                return mongoClient;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to connect to {database}" + e.StackTrace);
            }
        }

        public bool IsAlive(MongoClient mongoClient, string dbConnectionString)
        {
            if (!mongoClient.GetDatabase(dbConnectionString).RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(10000))
            {
                throw new MongoClientException($"Failed to connect to {nameof(dbConnectionString)}");
            }
            return true;
        }
    }
}
