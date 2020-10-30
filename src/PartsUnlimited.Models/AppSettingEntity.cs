namespace PartsUnlimited.Models
{
    public class AppSettingEntity : IAppSettingEntity
    {
        public KeysEntity Keys { get; set; }

        public AzureKeyValueEntity AzureKeyVault { get; set; }

        public ConnectionStringEntity ConnectionStrings { get; set; }
    }
}
