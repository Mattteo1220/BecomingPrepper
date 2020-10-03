using System;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class GalleryImageHelperRepository : IGalleryImageHelperRepository
    {
        private IMongoDatabase _mongoContext;
        private IMongoCollection<GalleryImageEntity> _collection;
        public GalleryImageHelperRepository(IMongoDatabase mongoContext)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _collection = _mongoContext.GetCollection<GalleryImageEntity>("InventoryImages.chunks");
        }

        public byte[] GetImage(ObjectId id)
        {
            var filter = Builders<GalleryImageEntity>.Filter.Where(im => im.files_id == id);
            var result = _collection.Find(filter).Limit(1).FirstOrDefault();
            return result.data;
        }
    }
}
