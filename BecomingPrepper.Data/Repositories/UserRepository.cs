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
        public IMongoCollection<UserEntity> UserCollection { get; set; }

        public IMongoCollection<UserEntity> Init(IMongoDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            UserCollection = database.GetCollection<UserEntity>(UserCollectionString);
            return UserCollection;
        }

        public async Task Add(UserEntity userEntity)
        {
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));
            try
            {
                await UserCollection.InsertOneAsync(userEntity);
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
                using IAsyncCursor<UserEntity> asyncCursor = await UserCollection.FindAsync(queryFilter);
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
                await UserCollection.FindOneAndUpdateAsync(queryFilter, updateFilter);
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
                await UserCollection.FindOneAndDeleteAsync(deleteFilter);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update Entity 'UserEntity' : {deleteFilter.ToJson()}");
            }
        }
    }
}
