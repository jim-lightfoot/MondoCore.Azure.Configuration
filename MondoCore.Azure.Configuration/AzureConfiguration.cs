using System;
using System.Collections;
using System.Collections.Generic;
using Azure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MondoCore.Common;

namespace MondoCore.Azure.Configuration
{
    /// <summary>
    /// Access configuration entries in Azure App Configuration
    /// </summary>
    public class AzureConfiguration : ISettings, IConfiguration, IReadOnlyDictionary<string, object>
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Connect to Azure App Configuration using a url and managed indenity for app running in Azure
        /// </summary>
        /// <param name="azureConfigUrl"></param>
        public AzureConfiguration(string azureConfigUrl)
        {
            var builder    = new ConfigurationBuilder();
            var credential = new DefaultAzureCredential();

            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(azureConfigUrl), credential);

                options.ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(credential);
                });
            });

            _config = builder.Build();
        }

        /// <summary>
        /// Connect to Azure App Configuration using a connection string and tenant id. For debugging and testing locally only.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tenantId"></param>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        public AzureConfiguration(string connectionString, string tenantId, string clientId, string secret)
        {
            var builder = new ConfigurationBuilder();

            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                       .ConfigureKeyVault(kv =>
                        {
                            kv.SetCredential(new ClientSecretCredential(tenantId, clientId, secret));
                        });
            });

            _config = builder.Build();
        }

        #region ISettings

        public string Get(string key)
        {
            return _config[key]!;
        }

        #endregion

        #region IReadOnlyDictionary

        public object this[string key] => this.Get(key);

        public bool TryGetValue(string key, out object value)
        {
            try
            {
                value = this.Get(key);
                return value != null;
            }
            catch
            {
              #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                value = null;
              #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                return false;
            }
        }

        #region Not Supported

        public IEnumerable<string> Keys => throw new NotSupportedException();

        public IEnumerable<object> Values => throw new NotSupportedException();

        public int Count => throw new NotSupportedException();

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        public bool Remove(string key)
        {
            throw new NotSupportedException();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        #endregion

        #endregion

        #region IConfiguration

        string? IConfiguration.this[string key] 
        { 
            get => _config[key]; 
            set => throw new NotSupportedException(); 
        }

        public IConfigurationSection GetSection(string key)
        {
            return _config.GetSection(key);
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _config.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return _config.GetReloadToken();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
