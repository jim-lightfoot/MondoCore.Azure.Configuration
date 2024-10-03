using Newtonsoft.Json;
using System.Reflection;

using MondoCore.Common;
using Microsoft.Extensions.Configuration;

namespace MondoCore.Azure.Configuration.FunctionalTests
{
    [TestClass]
    public class AzureConfigurationTests
    { 
        [TestMethod]
        public void AzureConfiguration_Get()
        {
            var config = CreateConfig();

            Assert.AreEqual("Chevy",        config.Get("Make"));
            Assert.AreEqual("Corvette" ,    config.Get("Model"));
            Assert.AreEqual("Black",        config.Get("Color"));
            Assert.AreEqual("1964",         config.Get("Year"));
        }

        [TestMethod]
        public void AzureConfiguration_GetAsDict()
        {
            var config = CreateConfigAsDict();

            Assert.AreEqual("Chevy",        config["Make"]);
            Assert.AreEqual("Corvette" ,    config["Model"]);
            Assert.AreEqual("Black",        config["Color"]);
            Assert.AreEqual("1964",         config["Year"]);
        }

        [TestMethod]
        public void AzureConfiguration_GetAsConfig()
        {
            var config = CreateConfigAsConfig();

            Assert.AreEqual("Chevy",        config["Make"]);
            Assert.AreEqual("Corvette" ,    config["Model"]);
            Assert.AreEqual("Black",        config["Color"]);
            Assert.AreEqual("1964",         config["Year"]);
        }

        #region Helpers

        private ISettings CreateConfig(string folder = "")
        { 
            var config = TestConfiguration.Load();

            return new AzureConfiguration(config.ConfigConnectionString);
        }

        private IDictionary<string, object> CreateConfigAsDict(string folder = "")
        { 
            var config = TestConfiguration.Load();

            return new AzureConfiguration(config.ConfigConnectionString);
        }

        private IConfiguration CreateConfigAsConfig(string folder = "")
        { 
            var config = TestConfiguration.Load();

            return new AzureConfiguration(config.ConfigConnectionString);
        }

        public static class TestConfiguration
        {
            public static Configuration Load()
            { 
                var path = Assembly.GetCallingAssembly().Location.SubstringBefore("\\bin");
                var json = File.ReadAllText(Path.Combine(path, "localhost.json"));

                return JsonConvert.DeserializeObject<Configuration>(json)!;
            }
        }

        public class Configuration
        {
            public string InstrumentationKey        { get; set; } = "";
            public string ConnectionString          { get; set; } = "";
            public string DataLakeConnectionString  { get; set; } = "";
            public string ConfigConnectionString    { get; set; } = "";
                                                                   
            public string KeyVaultUri               { get; set; } = "";
            public string KeyVaultTenantId          { get; set; } = "";
            public string KeyVaultClientId          { get; set; } = "";
            public string KeyVaultClientSecret      { get; set; } = "";    
        }

        #endregion
    }
}