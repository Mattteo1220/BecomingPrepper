using System.Collections.Generic;
using BecomingPrepper.Data.Entities.InventoryImageFiles;

namespace BecomingPrepper.Data.Repositories
{
    public interface IGallery
    {
        List<InventoryImageFileInfoEntity> GetAllImages();
    }
}