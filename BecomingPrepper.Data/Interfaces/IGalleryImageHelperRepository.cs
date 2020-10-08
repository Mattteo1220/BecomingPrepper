using System.Collections.Generic;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using MongoDB.Bson;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IGalleryImageHelperRepository
    {
        byte[] GetImage(ObjectId id);

        IEnumerable<GalleryImageEntity> GetChunks();
    }
}
