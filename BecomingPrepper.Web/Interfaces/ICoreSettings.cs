using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Driver;

namespace BecomingPrepper.Web.Models
{
    public interface ICoreSettings
    {
        IMongoDatabase MongoDatabase { get; set; }
        IRepository<UserEntity> Users { get; set; }
        IRepository<PrepGuideEntity> PrepGuides { get; set; }
        IRepository<RecommendedQuantityAmountEntity> RecommendedQuantities { get; set; }
        IRepository<FoodStorageInventoryEntity> FoodStorageInventory { get; set; }
    }
}
