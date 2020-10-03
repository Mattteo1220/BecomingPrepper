using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Core.UserUtility.Interfaces;
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
    public class ComponentRegistration
    {
        public void Register(ref IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection("Database").GetSection("Dev").Value;
            var mongoContext = new MongoClient(connectionString).GetDatabase(database);


            //LogManager
            var logger = new LoggerConfiguration()
                .WriteTo.MongoDB(connectionString, collectionName: "ExceptionLogs", period: TimeSpan.Zero)
                .MinimumLevel.Debug()
                .CreateLogger();

            services.AddSingleton(x => mongoContext);
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

            services.AddSingleton<IPrepGuide, PrepGuide>();
            services.AddSingleton<IInventoryUtility, InventoryUtility>();
            services.AddSingleton<ILogin, Login>();
            services.AddSingleton<IRegister, RegisterService>();
            services.AddSingleton<IServiceAccount, ServiceAccount>();
        }
    }
}