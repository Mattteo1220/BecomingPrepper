using System;
using System.Collections.Generic;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class GalleryFileHelperRepository : IGalleryFileHelperRepository
    {
        private IMongoDatabase _mongoContext;
        private IMongoCollection<GalleryFileInfoEntity> _collection;
        public GalleryFileHelperRepository(IMongoDatabase mongoContext)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _collection = _mongoContext.GetCollection<GalleryFileInfoEntity>("InventoryImages.files");
        }

        public List<GalleryFileInfoEntity> GetFiles()
        {
            return _collection.Find("{}").ToList();
        }
    }
}
