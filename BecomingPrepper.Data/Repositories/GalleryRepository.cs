using System;
using System.Collections.Generic;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class GalleryRepository : IRepository<InventoryImageFileInfoEntity>, IGallery
    {
        public IMongoCollection<InventoryImageFileInfoEntity> Collection { get; set; }
        private ILogManager _logger;
        public GalleryRepository(IMongoCollection<InventoryImageFileInfoEntity> collection, ILogManager logger)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add(InventoryImageFileInfoEntity t)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(FilterDefinition<InventoryImageFileInfoEntity> deleteFilter)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public InventoryImageFileInfoEntity Get(FilterDefinition<InventoryImageFileInfoEntity> queryFilter)
        {
            throw new NotImplementedException();
        }

        public List<InventoryImageFileInfoEntity> GetAllImages()
        {
            try
            {
                return Collection.Find("{}").ToList();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void Update(FilterDefinition<InventoryImageFileInfoEntity> queryFilter, UpdateDefinition<InventoryImageFileInfoEntity> updateFilter)
        {
            throw new System.NotImplementedException();
        }
    }
}
