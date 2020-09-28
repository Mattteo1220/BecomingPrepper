using System;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class GalleryImageHelperRepository : IGalleryImageHelperRepository
    {
        public IMongoCollection<GalleryImageEntity> Collection { get; set; }
        public GalleryImageHelperRepository(IMongoCollection<GalleryImageEntity> collection)
        {
            Collection = collection;
        }

        public byte[] GetImage(ObjectId id)
        {
            var filter = Builders<GalleryImageEntity>.Filter.Where(im => im.files_id == id);
            try
            {
                var result = Collection.Find(filter).Limit(1).FirstOrDefault();
                return result.data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
