using Fario.Extensions.Configuration.JsonConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fario.Extensions.Configuration
{
    public static class RemoteJsonConfigurationExtensions
    {

        public static IConfigurationBuilder AddRemoteJsonConfiguration(
            this IConfigurationBuilder builder,
            IJSInProcessRuntime jsRuntime,
            string path
        )
        {
            return builder.Add(new RemoteJsonConfigurationSource(jsRuntime, path));
        }
    }
}
