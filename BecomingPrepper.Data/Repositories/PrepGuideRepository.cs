using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class PrepGuideRepository : IDatabaseCollection<PrepGuideEntity>
    {
        public Task Add(PrepGuideEntity t)
        {
            throw new NotImplementedException();
        }

        public Task Delete(FilterDefinition<PrepGuideEntity> deleteFilter)
        {
            throw new NotImplementedException();
        }

        public Task<PrepGuideEntity> Get(FilterDefinition<PrepGuideEntity> queryFilter)
        {
            throw new NotImplementedException();
        }

        public IMongoCollection<PrepGuideEntity> Init(IMongoDatabase database)
        {
            throw new NotImplementedException();
        }

        public Task Update(FilterDefinition<PrepGuideEntity> queryFilter, UpdateDefinition<PrepGuideEntity> updateFilter)
        {
            throw new NotImplementedException();
        }
    }
}
