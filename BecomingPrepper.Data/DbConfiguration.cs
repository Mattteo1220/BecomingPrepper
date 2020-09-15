using Unity;

namespace BecomingPrepper.Data
{
    public class DbConfiguration : IDbConfiguration
    {
        private UnityContainer _container;
        public DbConfiguration(UnityContainer container)
        {
            _container = container;
        }
        public void GetMongoClient()
        {
            var mongoDatabase = _container.Resolve<MongoDatabase>();
            mongoDatabase.Connect("");

        }
    }
}
