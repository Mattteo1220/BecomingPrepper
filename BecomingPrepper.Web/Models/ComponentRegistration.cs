using System;
using BecomingPrepper.Data.Entities;
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
        public IRepository<FoodStorageInventoryEntity> FoodStorageInventory { get; set; }
        public IExceptionLogger ExceptionLogger { get; set; }
        public ISecureService SecureService { get; set; }

        public void Register(ref IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection("Database").GetSection("Dev").Value;
            var users = configuration.GetSection("Collections").GetSection("UsersCollection").Value;
            var prepGuides = configuration.GetSection("Collections").GetSection("PrepGuidesCollection").Value;
            var recommendedQuantities = configuration.GetSection("Collections").GetSection("RecommendedQuantityCollection").Value;
            var foodStorageInventory = configuration.GetSection("Collections").GetSection("FoodStorageInventoryCollection").Value;
            var exceptionLogs = configuration.GetSection("Collections").GetSection("ExceptionLogsCollection").Value;
            var mongoDatabase = new MongoClient(connectionString).GetDatabase(database);

            //ExceptionLogger
            var logger = new LoggerConfiguration()
                .WriteTo.MongoDB(connectionString, collectionName: exceptionLogs, period: TimeSpan.Zero)
                .MinimumLevel.Debug()
                .CreateLogger();
            var exceptionLogger = new ExceptionLogger(logger);

            //Secure Service
            var secureService = new SecureService(new HashingOptions());

            //Collections
            var usersCollections = mongoDatabase.GetCollection<UserEntity>(users);
            var prepGuidesCollection = mongoDatabase.GetCollection<PrepGuideEntity>(prepGuides);
            var recommendedQuantitiesCollection = mongoDatabase.GetCollection<RecommendedQuantityAmountEntity>(recommendedQuantities);
            var foodStorageInventoryCollection = mongoDatabase.GetCollection<FoodStorageInventoryEntity>(foodStorageInventory);

            var usersRepository = new UserRepository(usersCollections, exceptionLogger);
            var prepGuidesRepository = new PrepGuideRepository(prepGuidesCollection, exceptionLogger);
            var recommendedQuantitiesRepository = new RecommendedQuantityRepository(recommendedQuantitiesCollection, exceptionLogger);
            var foodStorageInventoryRepository = new FoodStorageInventoryRepository(foodStorageInventoryCollection, exceptionLogger);

            //Add To Services
            services.Add(new ServiceDescriptor(typeof(IComponentRegistration), new ComponentRegistration()
            {
                MongoDatabase = mongoDatabase,
                Users = usersRepository,
                PrepGuides = prepGuidesRepository,
                RecommendedQuantities = recommendedQuantitiesRepository,
                FoodStorageInventory = foodStorageInventoryRepository,
                ExceptionLogger = exceptionLogger,
                SecureService = secureService
            }));
        }
    }
}
