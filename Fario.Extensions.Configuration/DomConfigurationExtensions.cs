using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fario.Extensions.Configuration
{
    /// <summary>
    /// Extension method for adding an instance of <c>DomConfigurationProvider</c>
    /// to an <c>IConfigurationBuilder</c>.
    /// </summary>
    public static class DomConfigurationExtensions
    {
        /// <summary>
        /// Adds an instance of <c>DomConfigurationProvider</c> to the <c>IConfigurationBuilder</c>.
        /// </summary>
        /// <param name="jsRuntime">
        /// An instance of the Javascript Runtime, which is used to access the DOM.
        /// 
        /// The JS runtime needs to be an instance of IJSInProcessRuntime so that it can run JavaScript synchronously.
        /// All calls to IJSRuntime.InvokeAsync<T>().Result cause the application to hang while it is starting.
        /// </param>
        /// <param name="key">
        /// The name of the 'key' on the meta tag. Defaults to "data-name".
        /// </param>
        /// <param name="value">
        /// The name of the 'value' on the meta tag. Defaults to "data-value".
        /// </param>
        /// <returns>An <c>IConfiguraitonBuilder</c> with key-value pairs retrieved from the DOM.</returns>
        public static IConfigurationBuilder AddDomConfiguration(
            this IConfigurationBuilder builder,
            IJSInProcessRuntime jsRuntime,
            string key = "data-name", 
            string value = "data-value"
        )
        {
            return builder.Add(new DomConfigurationSource(jsRuntime, key, value));
        }
    }
}
