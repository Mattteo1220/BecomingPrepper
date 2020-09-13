using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class UserRepository : IDatabaseCollection<UserEntity>
    {
        private const string UserCollectionString = "UserCollection";
        private bool _disposed = false;
        public IMongoCollection<UserEntity> Collection { get; set; }

        public UserRepository(IMongoDatabase mongoDatabase)
        {
            if (mongoDatabase == null) throw new ArgumentNullException(nameof(mongoDatabase));
            Collection = mongoDatabase.GetCollection<UserEntity>(UserCollectionString);
        }

        public async Task Add(UserEntity userEntity)
        {
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));
            try
            {
                await Collection.InsertOneAsync(userEntity);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to Add Entity 'UserEntity': {userEntity.Account.Username}" + e.StackTrace);
            }
        }

        public async Task<UserEntity> Get(FilterDefinition<UserEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            UserEntity entity = null;
            try
            {
                using IAsyncCursor<UserEntity> asyncCursor = await Collection.FindAsync(queryFilter);
                while (await asyncCursor.MoveNextAsync())
                {
                    foreach (var cursor in asyncCursor.Current)
                    {
                        entity = cursor;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to Get 'Entity': 'UserEntity': {queryFilter.ToJson()}");
            }

            return entity;
        }

        public async Task Update(FilterDefinition<UserEntity> queryFilter, UpdateDefinition<UserEntity> updateFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));
            if (updateFilter == null) throw new ArgumentNullException(nameof(updateFilter));

            try
            {
                await Collection.FindOneAndUpdateAsync(queryFilter, updateFilter);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update Entity 'UserEntity' : {queryFilter.ToJson()}");
            }
        }

        public async Task Delete(FilterDefinition<UserEntity> deleteFilter)
        {
            if (deleteFilter == null) throw new ArgumentNullException(nameof(deleteFilter));

            try
            {
                await Collection.FindOneAndDeleteAsync(deleteFilter);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update Entity 'UserEntity' : {deleteFilter.ToJson()}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Collection = null;
                _disposed = true;
            }
        }
    }
}
