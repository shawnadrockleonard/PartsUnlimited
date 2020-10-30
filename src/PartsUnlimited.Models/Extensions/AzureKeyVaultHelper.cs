using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace PartsUnlimited.Models.Extensions
{
    public class AzureKeyVaultHelper : IKeyVaultHelper
    {
        private readonly string vault;
        private readonly string clientId;
        private readonly string clientSecret;

        public AzureKeyVaultHelper(IAppSettingEntity appSettings)
        {
            if (appSettings == null)
            {
                throw new ArgumentException("Configuration is missing Application Settings", nameof(appSettings));
            }

            vault = appSettings.AzureKeyVault?.Vault;
            clientId = appSettings.AzureKeyVault?.ClientId;
            clientSecret = appSettings.AzureKeyVault?.ClientSecret;
        }


        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                var secret = await GetKeyVaultClient().GetSecretAsync(vault, secretName);

                return secret.Value;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public async Task SetSecretAsync(string secretName, string secretValue)
        {
            try
            {
                await GetKeyVaultClient().SetSecretAsync(vault, secretName, secretValue);
            }
            catch (Exception ex)
            {
            }
        }

        public static KeyVaultClient GetKeyVaultClientFromManagedIdentity()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            return new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        private KeyVaultClient GetKeyVaultClient()
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                return new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessTokenAsync));
            }

            return GetKeyVaultClientFromManagedIdentity();
        }

        private async Task<string> GetAccessTokenAsync(string authority, string resource, string scope)
        {
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var authenticationContext = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await authenticationContext.AcquireTokenAsync(resource, clientCredential);

            return result.AccessToken;
        }
    }
}
