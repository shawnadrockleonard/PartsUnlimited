namespace PartsUnlimited.Models
{
    public interface IAppSettingEntity
    {
        ConfigKeysEntity Keys { get; set; }

        ConfigApplicationInsights ApplicationInsights { get; set; }

        ConfigAzureKeyValueEntity AzureKeyVault { get; set; }

        ConfigConnectionStringEntity ConnectionStrings { get; set; }
    }
}
