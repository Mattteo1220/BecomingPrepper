using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Core.RecommenedQuantitiesUtility;
using BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;

namespace BecomingPrepper.Web.Models
{
    public class ComponentRegistration : IComponentRegistration
    {
        public IMongoDatabase MongoDatabase { get; set; }
        public IRepository<UserEntity> Users { get; set; }
        public IRepository<PrepGuideEntity> PrepGuides { get; set; }
        public IRepository<RecommendedQuantityAmountEntity> RecommendedQuantities { get; set; }
        public IRepository<FoodStorageEntity> FoodStorageInventory { get; set; }
        public ILogManager LogManager { get; set; }
        public ISecureService SecureService { get; set; }
        public IRecommendService RecommendService { get; set; }
        public IInventoryUtility InventoryUtility { get; set; }
        public IGalleryImageHelperRepository GalleryImageHelperRepository { get; set; }
        public IGalleryFileHelperRepository GalleryFileHelperRepository { get; set; }

        public void Register(ref IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection("Database").GetSection("Dev").Value;
            var users = configuration.GetSection("Collections").GetSection("UsersCollection").Value;
            var prepGuides = configuration.GetSection("Collections").GetSection("PrepGuidesCollection").Value;
            var recommendedQuantities = configuration.GetSection("Collections").GetSection("RecommendedQuantityCollection").Value;
            var foodStorageInventory = configuration.GetSection("Collections").GetSection("FoodStorageInventoryCollection").Value;
            var exceptionLogs = configuration.GetSection("Collections").GetSection("ExceptionLogsCollection").Value;
            var imageFiles = configuration.GetSection("Collections").GetSection("ImageFiles").Value;
            var images = configuration.GetSection("Collections").GetSection("Images").Value;
            var mongoDatabase = new MongoClient(connectionString).GetDatabase(database);


            //LogManager
            var logger = new LoggerConfiguration()
                .WriteTo.MongoDB(connectionString, collectionName: exceptionLogs, period: TimeSpan.Zero)
                .MinimumLevel.Debug()
                .CreateLogger();
            var exceptionLogger = new LogManager(logger);

            //Collections
            var usersCollections = mongoDatabase.GetCollection<UserEntity>(users);
            var prepGuidesCollection = mongoDatabase.GetCollection<PrepGuideEntity>(prepGuides);
            var recommendedQuantitiesCollection = mongoDatabase.GetCollection<RecommendedQuantityAmountEntity>(recommendedQuantities);
            var foodStorageInventoryCollection = mongoDatabase.GetCollection<FoodStorageEntity>(foodStorageInventory);
            var galleryCollection = mongoDatabase.GetCollection<GalleryFileInfoEntity>(imageFiles);
            var gallery = mongoDatabase.GetCollection<GalleryImageEntity>(images);

            var usersRepository = new UserRepository(usersCollections, exceptionLogger);
            var prepGuidesRepository = new PrepGuideRepository(prepGuidesCollection, exceptionLogger);
            var recommendedQuantitiesRepository = new RecommendedQuantityRepository(recommendedQuantitiesCollection, exceptionLogger);
            var foodStorageInventoryRepository = new FoodStorageInventoryRepository(foodStorageInventoryCollection, exceptionLogger);
            var galleryFileHelperRepository = new GalleryFileHelperRepository(galleryCollection);
            var galleryImageHelperRepository = new GalleryImageHelperRepository(gallery);

            //Secure Service and Core
            var secureService = new SecureService(new HashingOptions());
            var recommendService = new RecommendService(recommendedQuantitiesRepository, LogManager);
            var inventoryUtility = new InventoryUtility(foodStorageInventoryRepository, galleryFileHelperRepository, galleryImageHelperRepository, LogManager);

            //Add To Services
            services.Add(new ServiceDescriptor(typeof(IComponentRegistration), new ComponentRegistration()
            {
                MongoDatabase = mongoDatabase,
                Users = usersRepository,
                PrepGuides = prepGuidesRepository,
                RecommendedQuantities = recommendedQuantitiesRepository,
                FoodStorageInventory = foodStorageInventoryRepository,
                LogManager = exceptionLogger,
                SecureService = secureService,
                RecommendService = recommendService,
                InventoryUtility = inventoryUtility,
                GalleryImageHelperRepository = GalleryImageHelperRepository,
                GalleryFileHelperRepository = GalleryFileHelperRepository
            }));
        }
    }
}
