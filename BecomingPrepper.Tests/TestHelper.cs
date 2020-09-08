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
            var connectionString = isInvalidConnString ? "TestConnString" : testContext.MongoClient;

            IConfigurationSection mockDatabaseName = Mock.Of<IConfigurationSection>
            (
                section => section.Value == testContext.Database
            );

            IConfigurationSection mockConnectionString = Mock.Of<IConfigurationSection>
            (
                section => section.Value == connectionString
            );

            IConfiguration mockConfiguration = Mock.Of<IConfiguration>
            (
                config => config.GetSection(testContext.ConnectionStringConfigurationSection) == mockConnectionString &&
                          config.GetSection(testContext.DatabaseConfigurationSection) == mockDatabaseName
            );

            return mockConfiguration;
        }

        public class TestConfiguration
        {
            public string MongoClient { get; set; }
            public string Database { get; set; }
            public string ConnectionStringConfigurationSection { get; set; }
            public string DatabaseConfigurationSection { get; set; }
        }
    }
}
