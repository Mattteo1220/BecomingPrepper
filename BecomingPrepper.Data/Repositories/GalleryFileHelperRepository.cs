using System;
using System.Collections.Generic;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class GalleryFileHelperRepository : IGalleryFileHelperRepository
    {
        public IMongoCollection<GalleryFileInfoEntity> Collection { get; set; }
        public GalleryFileHelperRepository(IMongoCollection<GalleryFileInfoEntity> collection)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public List<GalleryFileInfoEntity> GetFiles()
        {
            try
            {
                return Collection.Find("{}").ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
