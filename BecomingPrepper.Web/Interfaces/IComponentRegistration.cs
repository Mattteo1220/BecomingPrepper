using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using MongoDB.Driver;

namespace BecomingPrepper.Web.Models
{
    public interface IComponentRegistration
    {
        IMongoDatabase MongoDatabase { get; set; }
        IRepository<UserEntity> Users { get; set; }
        IRepository<PrepGuideEntity> PrepGuides { get; set; }
        IRepository<RecommendedQuantityAmountEntity> RecommendedQuantities { get; set; }
        IRepository<FoodStorageInventoryEntity> FoodStorageInventory { get; set; }
        ILogManager LogManager { get; set; }
        ISecureService SecureService { get; set; }
    }
}
