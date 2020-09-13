using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IDatabaseCollection<T> : IDisposable
    {
        public IMongoCollection<T> Collection { get; set; }
        Task Add(T t);
        Task<T> Get(FilterDefinition<T> queryFilter);
        Task Update(FilterDefinition<T> queryFilter, UpdateDefinition<T> updateFilter);
        Task Delete(FilterDefinition<T> deleteFilter);

    }
}
