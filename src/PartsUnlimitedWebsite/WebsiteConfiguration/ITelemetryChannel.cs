// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PartsUnlimited.WebsiteConfiguration
{
    public interface ITelemetryChannel
    {
        string EndpointAddress { get; }

        bool? DeveloperMode { get; set; }
    }
}