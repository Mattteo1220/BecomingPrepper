using System.Threading.Tasks;
using BecomingPrepper.Data.Entities.InventoryImageFiles;

namespace BecomingPrepper.Core.ImageResourceHelper
{
    public interface IGridFSHelper
    {
        Task<FileDetailEntity> GetFileDetails(string itemId);

        Task<ChunkEntity> GetChunks(string itemId);
    }
}
