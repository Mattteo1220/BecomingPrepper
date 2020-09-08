using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;

namespace BecomingPrepper.Tests
{
    public class TestHelper
    {
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

        public class TestConfiguration
        {
            public string MongoClient { get; set; }
            public string Database { get; set; }
        }
    }
}
