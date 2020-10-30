using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;

namespace PartsUnlimited.WebJobs.ProcessOrder
{
    public static class AzureKeyVaultBuilderExtensions
    {
        private const string AzureKeyVaultKey = "AzureKeyVault";
        private const string AzureKeyVaultUrlKey = "Vault";

        /// <summary>
        /// if Azure Key Value is available, reads configuration values from the Azure KeyVault.
        /// </summary>
        /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
        public static IConfigurationBuilder AddAzureKeyVaultIfAvailable(this IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var Configuration = builder.Build();
            var keyVaultSection = Configuration.GetSection(AzureKeyVaultKey);
            string clientId = keyVaultSection["ClientId"];
            string vaultAddress = keyVaultSection[AzureKeyVaultUrlKey];

            if (string.IsNullOrEmpty(vaultAddress))
            {
                return builder;
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                // Try to access the Key Vault utilizing the Managed Service Identity of the running resource/process
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var vaultClient =
                    new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                builder.AddAzureKeyVault(vaultAddress, vaultClient, new DefaultKeyVaultSecretManager());
            }
            else
            {
                // Allow to override the MSI or for local dev
                builder.AddAzureKeyVault(vaultAddress, clientId, keyVaultSection["ClientSecret"]);
            }

            return builder;
        }

    }
}
