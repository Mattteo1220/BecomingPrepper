using System;
using BecomingPrepper.Data.Entities.Logins;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class LoginRepository : IRepository<Login>
    {
        private readonly IMongoDatabase _mongoContext;
        private readonly IMongoCollection<Login> _collection;
        private readonly ILogManager _logger;
        public LoginRepository(IMongoDatabase mongoContext, ILogManager logger)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _collection = _mongoContext.GetCollection<Login>("Logins");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add(Login login)
        {
            try
            {
                _collection.InsertOne(login);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        public void Delete(FilterDefinition<Login> deleteFilter)
        {
            throw new NotImplementedException();
        }

        public Login Get(FilterDefinition<Login> queryFilter)
        {
            try
            {
                var login = _collection.Find(queryFilter).FirstOrDefault();
                return login;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        public void Update(FilterDefinition<Login> queryFilter, UpdateDefinition<Login> updateFilter)
        {
            try
            {
                _collection.FindOneAndUpdate(queryFilter, updateFilter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}
