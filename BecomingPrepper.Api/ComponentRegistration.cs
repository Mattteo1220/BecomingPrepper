using System;
using System.Text;
using System.Threading.Tasks;
using BecomingPrepper.Api.Authentication;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Core.GridFSHelper;
using BecomingPrepper.Core.ImageResourceHelper;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Core.ProgressTrackerProcessor;
using BecomingPrepper.Core.RecommenedQuantitiesUtility;
using BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces;
using BecomingPrepper.Core.TokenService;
using BecomingPrepper.Core.TokenService.Interface;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.Endpoint;
using BecomingPrepper.Data.Entities.Logins;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Serilog;

namespace BecomingPrepper.Web.Models
{
    public class ComponentRegistration
    {
        public void Register(ref IServiceCollection services, IConfiguration configuration, string environment)
        {
            var connectionString = configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = configuration.GetSection("Database").GetSection("Dev").Value;
            var mongoContext = new MongoClient(connectionString).GetDatabase(database);

            //LogManager
            var logger = new LoggerConfiguration()
                .WriteTo.MongoDB(connectionString, collectionName: "Logs", period: TimeSpan.Zero)
                .MinimumLevel.Debug()
                .CreateLogger();

            var tokenInfo = new TokenInfo()
            {
                Issuer = configuration.GetSection("TokenInformation").GetSection("Issuer").Value,
                Audience = configuration.GetSection("TokenInformation").GetSection("Audience").Value,
                ExpiryInMinutes = Convert.ToInt32(configuration.GetSection("TokenInformation").GetSection("ExpiryInMinutes").Value)
            };

            var key = Environment.GetEnvironmentVariable("Secret") ?? throw new NullReferenceException("Secret was null");

            var validationParams = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)),
                ValidIssuer = tokenInfo.Issuer,
                ValidAudience = tokenInfo.Audience,
                ClockSkew = TimeSpan.Zero
            };

            var events = new JwtBearerEvents()
            {
                // invoked when the token validation fails
                OnAuthenticationFailed = (context) =>
                {
                    //if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    //{
                    //    context.Response.Headers.Add("Token-Expired", "true");
                    //}
                    return Task.CompletedTask;
                },

                // invoked when a request is received
                OnMessageReceived = (context) => Task.CompletedTask,

                // invoked when token is validated
                OnTokenValidated = (context) => Task.CompletedTask
            };

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme
                        = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme
                        = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = validationParams;
                    options.Events = events;

                }).AddCookie();

            services.AddSingleton(x => mongoContext);
            services.AddSingleton<ILogger, Serilog.Core.Logger>(x => logger);
            services.AddSingleton<ILogManager, LogManager>();
            services.AddSingleton<HashingOptions>();
            services.AddSingleton<ISecureService, SecureService>();
            services.AddSingleton<IFileDetailRepository, FileDetailRepository>();
            services.AddSingleton<IChunkRepository, ChunkRepository>();
            services.AddSingleton<IRepository<UserEntity>, UserRepository>();
            services.AddSingleton<IRepository<FoodStorageEntity>, FoodStorageInventoryRepository>();
            services.AddSingleton<IRepository<PrepGuideEntity>, PrepGuideRepository>();
            services.AddSingleton<IRepository<RecommendedQuantityAmountEntity>, RecommendedQuantityRepository>();
            services.AddSingleton<IRepository<Login>, LoginDataRepository>();
            services.AddSingleton<IRepository<EndpointEntity>, EndpointRepository>();

            services.AddSingleton<IPrepGuide, PrepGuide>();
            services.AddSingleton<IInventoryUtility, InventoryUtility>();
            services.AddSingleton<ILoginUtility, LoginUtility>();
            services.AddSingleton<IRegisterService, RegisterService>();
            services.AddSingleton<IServiceAccount, ServiceAccount>();
            services.AddSingleton<IRecommendService, RecommendService>();
            services.AddSingleton<IProgressTracker, ProgressTracker>();
            services.AddSingleton<IGridFSHelper, GridFSHelper>();
            services.AddSingleton(x => tokenInfo);
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<ILoginDataService, LoginDataService>();
        }
    }
}