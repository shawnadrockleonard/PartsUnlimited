namespace PartsUnlimited.Models
{
    public class ConfigAppSettingEntity : IAppSettingEntity
    {
        public ConfigKeysEntity Keys { get; set; }

        public ConfigApplicationInsights ApplicationInsights { get; set; }

        public ConfigAzureKeyValueEntity AzureKeyVault { get; set; }

        public ConfigConnectionStringEntity ConnectionStrings { get; set; }
    }
}
