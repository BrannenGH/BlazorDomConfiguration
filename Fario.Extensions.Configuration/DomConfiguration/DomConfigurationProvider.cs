using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fario.Extensions.Configuration.DomConfiguration
{
    /// <summary>
    /// Uses an instance of IJSInProcessRuntime to 
    /// </summary>
    public class DomConfigurationProvider: ConfigurationProvider
    {
        private readonly IJSInProcessRuntime jsRuntime;
        private readonly string keyLabel;
        private readonly string valueLabel;

        /// <summary>
        /// Configures a configuration that can be loaded from the DOM accessed by the IJSRuntime through the "window.document" field.
        /// </summary>
        /// <param name="jsRuntime">The Javascript runtime corresponding to the DOM to be accessed.</param>
        /// <param name="key">The name for the "key" on the metatag.</param>
        /// <param name="value">The name for the "value" on the metatag.</param>
        public DomConfigurationProvider(IJSInProcessRuntime jsRuntime, string key, string value)
        {
            keyLabel = key;
            valueLabel = value;
            this.jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Uses the IJSRuntime to load the metatags, and then parse out the key value pairs.
        /// </summary>
        public override void Load()
        {
            Data = ParseMetadata(jsRuntime.GetKeyValueMetatags(keyLabel, valueLabel));
        }

        /// <summary>
        /// Parse the data retrieved from the IJSRuntime into a dictionary.
        /// </summary>
        /// <param name="jsonRepsonse">The result from the IJSRuntime.</param>
        /// <returns>A dictionary with key-value pairs representing the configuration.</returns>
        private Dictionary<string, string> ParseMetadata(JsonElement jsonRepsonse)
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            foreach (var pair in jsonRepsonse.EnumerateArray())
            {
                string? key = pair.GetOptionalProperty("key")?.ToString();
                string? value = pair.GetOptionalProperty("value")?.ToString();

                if (key != null && value != null)
                {
                    config[key] = value;
                }
            }

            return config;
        }
    }

    internal static class JsonElementExtensions
    {
        public static JsonElement? GetOptionalProperty(this JsonElement element, string property)
        {
            try
            {
                return element.GetProperty(property);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
