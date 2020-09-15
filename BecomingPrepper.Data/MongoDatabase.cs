using MongoDB.Driver;

namespace BecomingPrepper.Data
{
    public class MongoDatabase
    {
        private MongoClient _mongoClient;
        public MongoDatabase(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public IMongoDatabase Connect(string database)
        {
            return _mongoClient.GetDatabase(database);
        }
    }
}
