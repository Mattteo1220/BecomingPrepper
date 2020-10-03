using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class UserRepository : IRepository<UserEntity>
    {
        private bool _disposed = false;
        private IMongoCollection<UserEntity> _collection;
        private IMongoDatabase _mongoContext;
        private ILogManager _logger;

        public UserRepository(IMongoDatabase mongoContext, ILogManager logger)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _collection = _mongoContext.GetCollection<UserEntity>("Users");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add(UserEntity userEntity)
        {
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));
            try
            {
                _collection.InsertOne(userEntity);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public UserEntity Get(FilterDefinition<UserEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            UserEntity entity = null;
            try
            {
                entity = _collection.Find(queryFilter).Limit(1).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

            return entity;
        }

        public void Update(FilterDefinition<UserEntity> queryFilter, UpdateDefinition<UserEntity> updateFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));
            if (updateFilter == null) throw new ArgumentNullException(nameof(updateFilter));

            try
            {
                _collection.FindOneAndUpdate(queryFilter, updateFilter);
            }
            catch (Exception e)
            {

                _logger.LogError(e);
                throw;
            }
        }

        public void Delete(FilterDefinition<UserEntity> deleteFilter)
        {
            if (deleteFilter == null) throw new ArgumentNullException(nameof(deleteFilter));

            try
            {
                _collection.FindOneAndDelete(deleteFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}
