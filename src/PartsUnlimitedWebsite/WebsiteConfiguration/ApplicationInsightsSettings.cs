// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using PartsUnlimited.Models;

namespace PartsUnlimited.WebsiteConfiguration
{
    public class ConfigurationApplicationInsightsSettings : IApplicationInsightsSettings
    {
        public ConfigurationApplicationInsightsSettings(IConfiguration config)
        {
            InstrumentationKey = config[nameof(InstrumentationKey)];
            TelemetryChannel = new ConfigurationApplicationInsightsTelemetryChannel(config);
        }

        public string InstrumentationKey { get; }

        public ITelemetryChannel TelemetryChannel { get; }
    }
}