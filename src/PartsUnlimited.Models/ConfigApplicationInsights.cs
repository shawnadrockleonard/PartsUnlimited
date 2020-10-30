namespace PartsUnlimited.Models
{
    public class ConfigApplicationInsights
    {
        public ConfigTelemetryChannelEntity TelemetryChannel { get; set; }

        public string InstrumentationKey { get; set; }
    }

    public class ConfigTelemetryChannelEntity
    {
        public string EndpointAddress { get; set; }
    }
}