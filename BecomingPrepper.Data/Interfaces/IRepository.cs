using System;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        public IMongoCollection<T> Collection { get; set; }
        void Add(T t);
        T Get(FilterDefinition<T> queryFilter);
        void Update(FilterDefinition<T> queryFilter, UpdateDefinition<T> updateFilter);
        void Delete(FilterDefinition<T> deleteFilter);

    }
}
