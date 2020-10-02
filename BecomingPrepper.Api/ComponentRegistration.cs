using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Core.RecommenedQuantitiesUtility;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Core.UserUtility.Interfaces;
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
    public class ComponentRegistration
    {
        public void Register(ref IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection("Database").GetSection("Dev").Value;
            var users = configuration.GetSection("Collections").GetSection("UsersCollection").Value;
            var prepGuides = configuration.GetSection("Collections").GetSection("PrepGuidesCollection").Value;
            var recommendedQuantities = configuration.GetSection("Collections").GetSection("RecommendedQuantityCollection").Value;
            var foodStorageInventory = configuration.GetSection("Collections").GetSection("FoodStorageCollection").Value;
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
            var recommendService = new RecommendService(recommendedQuantitiesRepository, exceptionLogger);
            var inventoryUtility = new InventoryUtility(foodStorageInventoryRepository, galleryFileHelperRepository, galleryImageHelperRepository, exceptionLogger);

            services.AddSingleton<IMongoClient, MongoClient>(x => (MongoClient)x.GetService((Type)mongoDatabase));
            services.AddSingleton(x => usersCollections);
            services.AddSingleton(x => foodStorageInventoryCollection);
            services.AddSingleton(x => prepGuidesCollection);
            services.AddSingleton(x => recommendedQuantitiesCollection);
            services.AddSingleton(x => galleryCollection);
            services.AddSingleton(x => gallery);
            services.AddSingleton<ILogger, Serilog.Core.Logger>(x => logger);
            services.AddSingleton<ILogManager, LogManager>();
            services.AddSingleton<HashingOptions>();
            services.AddSingleton<ISecureService, SecureService>();
            services.AddSingleton<IGalleryFileHelperRepository, GalleryFileHelperRepository>();
            services.AddSingleton<IGalleryImageHelperRepository, GalleryImageHelperRepository>();
            services.AddSingleton<IRepository<UserEntity>, UserRepository>();
            services.AddSingleton<IRepository<FoodStorageEntity>, FoodStorageInventoryRepository>();
            services.AddSingleton<IRepository<PrepGuideEntity>, PrepGuideRepository>();
            services.AddSingleton<IRepository<RecommendedQuantityAmountEntity>, RecommendedQuantityRepository>();

            services.AddSingleton<IInventoryUtility, InventoryUtility>();
            services.AddSingleton<ILogin, Login>();
            services.AddSingleton<IRegister, RegisterService>();
            services.AddSingleton<IServiceAccount, ServiceAccount>();
        }
    }
}