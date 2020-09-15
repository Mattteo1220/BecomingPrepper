using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Driver;

namespace BecomingPrepper.Web.Models
{
    public class CoreSettings : ICoreSettings
    {
        public IMongoDatabase MongoDatabase { get; set; }
        public IRepository<UserEntity> Users { get; set; }
        public IRepository<PrepGuideEntity> PrepGuides { get; set; }
        public IRepository<RecommendedQuantityAmountEntity> RecommendedQuantities { get; set; }
        public IRepository<FoodStorageInventoryEntity> FoodStorageInventory { get; set; }
    }
}
