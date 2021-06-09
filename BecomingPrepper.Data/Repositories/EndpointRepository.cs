using System;
using BecomingPrepper.Data.Entities.Endpoint;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class EndpointRepository : IRepository<EndpointEntity>
    {
        private IMongoDatabase _mongoContext;
        private IMongoCollection<EndpointEntity> _collection;
        private ILogManager _logManager;
        public EndpointRepository(IMongoDatabase mongoContext, ILogManager logManager)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
            _collection = _mongoContext.GetCollection<EndpointEntity>("Endpoints");
        }

        public void Add(EndpointEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _collection.InsertOne(entity);
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                throw;
            }
        }

        public EndpointEntity Get(FilterDefinition<EndpointEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            EndpointEntity entity = null;
            try
            {
                entity = _collection.Find(queryFilter).Limit(1).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                throw;
            }

            return entity;
        }

        public void Update(FilterDefinition<EndpointEntity> queryFilter, UpdateDefinition<EndpointEntity> updateFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));
            if (updateFilter == null) throw new ArgumentNullException(nameof(updateFilter));

            try
            {
                _collection.FindOneAndUpdate(queryFilter, updateFilter);
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                throw;
            }
        }

        public void Delete(FilterDefinition<EndpointEntity> deleteFilter)
        {
            if (deleteFilter == null) throw new ArgumentNullException(nameof(deleteFilter));

            try
            {
                _collection.FindOneAndDelete(deleteFilter);
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                throw;
            }
        }
    }
}
