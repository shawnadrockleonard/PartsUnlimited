namespace PartsUnlimited.Models
{
    public interface IAppSettingEntity
    {
        KeysEntity Keys { get; set; }

        AzureKeyValueEntity AzureKeyVault { get; set; }

        ConnectionStringEntity ConnectionStrings { get; set; }
    }
}
