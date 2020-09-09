using System.Threading.Tasks;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IDatabaseCollection<T>
    {
        IMongoCollection<T> Init(IMongoDatabase database);
        Task Add(T t);
        Task<T> Get(FilterDefinition<T> queryFilter);
        Task Update(FilterDefinition<T> queryFilter, UpdateDefinition<T> updateFilter);
        Task Delete(FilterDefinition<T> deleteFilter);

    }
}
