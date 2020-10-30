using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartsUnlimited.WebsiteConfiguration
{
    public class ConfigurationApplicationInsightsTelemetryChannel : ITelemetryChannel
    {
        public ConfigurationApplicationInsightsTelemetryChannel(IConfiguration config)
        {
            var telemetry = config.GetSection(ConfigurationPath.Combine("TelemetryChannel"));
            DeveloperMode = bool.Parse(telemetry[nameof(DeveloperMode)] ?? "false");
            EndpointAddress = telemetry[nameof(EndpointAddress)];
        }

        public bool? DeveloperMode { get; set; }
        //
        // Summary:
        //     Gets or sets the endpoint address of the channel.
        public string EndpointAddress { get; set; }

    }
}
