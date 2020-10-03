using MongoDB.Driver;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T t);
        T Get(FilterDefinition<T> queryFilter);
        void Update(FilterDefinition<T> queryFilter, UpdateDefinition<T> updateFilter);
        void Delete(FilterDefinition<T> deleteFilter);

    }
}
