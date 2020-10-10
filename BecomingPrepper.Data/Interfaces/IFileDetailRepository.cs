using System.Threading.Tasks;
using BecomingPrepper.Data.Entities.InventoryImageFiles;

namespace BecomingPrepper.Data.Repositories
{
    public interface IFileDetailRepository
    {
        Task<FileDetailEntity> GetInventoryImageFilesAsync(string itemId);
    }
}