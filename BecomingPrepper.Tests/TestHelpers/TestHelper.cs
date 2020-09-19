using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using BecomingPrepper.Data;
using BecomingPrepper.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using Serilog;

namespace BecomingPrepper.Tests
{
    public class TestHelper
    {
        private const string Environment = "Dev";
        private const string LogCollection = "ExceptionLogs";
        private const string Database = "BecomingPrepper_Dev";
        public const string RecommendedQuantityId = "5f59291f65554c3ddaa060b3";
        public static TestConfiguration GetTestContext()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Runsettings.json");
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<TestConfiguration>(json);
        }

        public static IConfiguration GetMockConfiguration(bool isInvalidConnString = false)
        {
            var testContext = GetTestContext();
            var mongoClient = isInvalidConnString ? "TestConnString" : testContext.MongoClient;

            IConfigurationSection mockDatabase = Mock.Of<IConfigurationSection>
            (
                section => section.Key == "Database" && section.Value == testContext.Database
            );

            IConfigurationSection mockMongoClient = Mock.Of<IConfigurationSection>
            (
                section => section.Key == "Connection" && section.Value == mongoClient
            );

            IConfiguration mockConfiguration = Mock.Of<IConfiguration>
            (
                config => config.GetSection("MongoClient").GetSection("Connection") == mockMongoClient &&
                          config.GetSection("Database").GetSection("Dev") == mockDatabase
            );

            return mockConfiguration;
        }

        public static Mock<IDatabase> GetMockDatabase()
        {
            var mockConfiguration = GetMockConfiguration();
            var mockMongoDatabase = new Mock<IMongoDatabase>();
            var mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(db => db.Connect(mockConfiguration, Environment)).Returns(new MongoClient());
            mockDatabase.Setup(db => db.GetDatabase(mockDatabase.Object.MongoClient, Database)).Returns(mockMongoDatabase.Object);
            mockDatabase.SetupGet(db => db.MongoClient).Returns(new MongoClient());
            mockDatabase.SetupGet(db => db.MongoDatabase).Returns(mockMongoDatabase.Object);
            mockDatabase.Object.Connect(mockConfiguration, Environment);
            mockDatabase.Object.GetDatabase(mockDatabase.Object.MongoClient, Database);

            return mockDatabase;
        }

        public static IMongoDatabase GetDatabase()
        {
            var mockConfiguration = GetMockConfiguration();
            var database = new TestDatabaseHelper();
            database.Connect(mockConfiguration, Environment);
            return database.MongoDatabase;
        }

        public static Serilog.Core.Logger GetLogger()
        {
            var testContext = GetTestContext();

            var loggerConfig = new LoggerConfiguration()
                .WriteTo.MongoDB(testContext.MongoClient, collectionName: LogCollection, period: TimeSpan.FromSeconds(5), batchPostingLimit: 1)
                .MinimumLevel.Debug()
                .CreateLogger();
            return loggerConfig;
        }

        public class TestConfiguration
        {
            public string MongoClient { get; set; }
            public string Database { get; set; }
        }

        public static void WaitUntil(Func<bool> func, TimeSpan timeToRetry)
        {
            var stopWatch = Stopwatch.StartNew();
            var passed = false;
            while (!passed && stopWatch.Elapsed <= timeToRetry)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                passed = func();
            }
        }
    }
}
