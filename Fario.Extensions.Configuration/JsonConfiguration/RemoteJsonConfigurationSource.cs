using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Fario.Extensions.Configuration.JsonConfiguration
{
    public class RemoteJsonConfigurationSource: JsonStreamConfigurationSource
    {
        private readonly IJSInProcessRuntime jsRuntime;

        public string Path { get; set; }

        public RemoteJsonConfigurationSource(IJSInProcessRuntime jsRuntime, string path): base() 
        {
            this.jsRuntime = jsRuntime;
            Path = path;
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            string? json = ParseResponse(jsRuntime.GetFileJS(Path));

            if (json == null)
            {
                throw new Exception("The path could not be loaded from the remote server.");
            }

            Stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            return base.Build(builder);
        }

        private string? ParseResponse(JsonElement response)
        {
            if (response.ValueKind != JsonValueKind.String)
            {
                return null;
            }

            return response.GetString();
        }
    }
}
