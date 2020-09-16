using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class ExceptionLogRepository : IRepository<ExceptionLogEntity>
    {
        public IMongoCollection<ExceptionLogEntity> Collection { get; set; }

        public ExceptionLogRepository(IMongoCollection<ExceptionLogEntity> collection)
        {
            Collection = collection;
        }

        public Task Add(ExceptionLogEntity exceptionLogEntity)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(FilterDefinition<ExceptionLogEntity> deleteFilter)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<ExceptionLogEntity> Get(FilterDefinition<ExceptionLogEntity> queryFilter)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(FilterDefinition<ExceptionLogEntity> queryFilter, UpdateDefinition<ExceptionLogEntity> updateFilter)
        {
            throw new System.NotImplementedException();
        }
    }
}
