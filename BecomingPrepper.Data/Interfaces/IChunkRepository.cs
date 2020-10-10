using System.Threading.Tasks;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using MongoDB.Bson;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IChunkRepository
    {
        Task<ChunkEntity> GetInventoryImageChunksAsync(ObjectId id);
    }
}
