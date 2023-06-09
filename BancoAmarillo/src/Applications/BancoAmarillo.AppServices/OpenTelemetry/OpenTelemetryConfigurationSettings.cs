﻿using System.Diagnostics.CodeAnalysis;

namespace OpenTelemetry
{
    [ExcludeFromCodeCoverage]
    public class OpenTelemetryConfigurationSettings
    {
        public bool IsEnabled { get; set; }

        public bool AddEntityFramework { get; set; }

        public string ZipEndPoint { get; set; }
    }
}