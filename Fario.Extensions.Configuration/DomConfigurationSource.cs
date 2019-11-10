using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.JSInterop;

namespace Fario.Extensions.Configuration
{
    /// <summary>
    /// Represents a DOM which can be used to retrieve a configuration from the meta tags in a document head.
    /// </summary>
    public class DomConfigurationSource : IConfigurationSource
    {
        private readonly IJSInProcessRuntime jsRuntime;
        
        public string Key { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Configures a configuration that can be loaded from the DOM accessed by the IJSRuntime through the "window.document" field.
        /// </summary>
        /// <param name="jsRuntime">
        /// The Javascript runtime corresponding to the DOM to be accessed.
        /// 
        /// The JS runtime needs to be an instance of IJSInProcessRuntime so that it can run JavaScript synchronously.
        /// All calls to IJSRuntime.InvokeAsync<T>().Result cause the application to hang while it is starting.        
        /// </param>
        /// <param name="key">The name for the "key" on the metatag.</param>
        /// <param name="value">The name for the "value" on the metatag.</param>
        public DomConfigurationSource(IJSInProcessRuntime jsRuntime, string key = "data-name", string value = "data-value")
        {
            Key = key;
            Value = value;
            this.jsRuntime = jsRuntime;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DomConfigurationProvider(jsRuntime, Key, Value);
        }
    }
}
