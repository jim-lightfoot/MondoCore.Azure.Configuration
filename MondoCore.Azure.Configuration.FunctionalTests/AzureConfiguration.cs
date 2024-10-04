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
            ISettings config = CreateConfig();

            Assert.AreEqual("Chevy",        config.Get("Make"));
            Assert.AreEqual("Corvette" ,    config.Get("Model"));
            Assert.AreEqual("Black",        config.Get("Color"));
            Assert.AreEqual("1964",         config.Get("Year"));
        }

        [TestMethod]
        public void AzureConfiguration_GetAsDict()
        {
            IReadOnlyDictionary<string, object> config = CreateConfig();

            Assert.AreEqual("Chevy",        config["Make"]);
            Assert.AreEqual("Corvette" ,    config["Model"]);
            Assert.AreEqual("Black",        config["Color"]);
            Assert.AreEqual("1964",         config["Year"]);
        }

        [TestMethod]
        public void AzureConfiguration_GetAsDict_TryGetValue_false()
        {
            IReadOnlyDictionary<string, object> config = CreateConfig();

            Assert.IsFalse(config.TryGetValue("Makexx", out object val));
        }

        [TestMethod]
        public void AzureConfiguration_GetAsDict_TryGetValue_true()
        {
            IReadOnlyDictionary<string, object> config = CreateConfig();

            Assert.IsTrue(config.TryGetValue("Make", out object val));
        }

        [TestMethod]
        public void AzureConfiguration_GetAsConfig()
        {
            IConfiguration config = CreateConfig();

            Assert.AreEqual("Chevy",        config["Make"]);
            Assert.AreEqual("Corvette" ,    config["Model"]);
            Assert.AreEqual("Black",        config["Color"]);
            Assert.AreEqual("1964",         config["Year"]);
        }

        #region Helpers

        private AzureConfiguration CreateConfig(string folder = "")
        { 
            var config = TestConfiguration.Load();

            return new AzureConfiguration(config.ConfigConnectionString, config.KeyVaultTenantId, config.KeyVaultClientId, config.KeyVaultClientSecret);
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