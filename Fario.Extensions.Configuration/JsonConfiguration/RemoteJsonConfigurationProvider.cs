using Microsoft.Extensions.Configuration.Json;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fario.Extensions.Configuration.JsonConfiguration
{
    public class RemoteJsonConfigurationProvider: JsonStreamConfigurationProvider
    {
        public RemoteJsonConfigurationProvider(IJSInProcessRuntime jsRuntime, string path): 
            base(new RemoteJsonConfigurationSource(jsRuntime, path))
        {

        }
    }
}
