using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Fario.Extensions.Configuration
{
    internal static class JSFunctions 
    {
        /// <remarks>
        /// It would be ideal to run the document.getElementsByTagName() function and do the rest of the parsing
        /// in the C# runtime, but the HTMLCollection returned by that function does not serialize well, and nor does the
        /// DOM node objects themselves. So only POJO objects are taken from the IJSRuntime, which serializes correctly.
        /// </remarks>
        internal static string GetMetatagsJS(string key, string value) 
        {
            return "Array.from(document.getElementsByTagName(\"meta\")).map(x => x.attributes).map(x => { return { key: x[\"" + key + "\"], value: x[\"" + value + "\"]}}).filter(x => x.key != undefined).map(x => { return { key: x.key.value, value: x.value.value } })";
        }

        /// <remarks>
        /// It would be ideal to use the <c>HttpClient</c> built into Blazor, and use that to do the Http Request,
        /// but making synchronous requests with the HttpClient during startup seems to stall Blazor startup.
        /// </remarks>
        internal static string GetFileJS(string path)
        {
            return "var req = new XMLHttpRequest(); req.overrideMimeType(\"application/json\"); req.open(\"GET\", \"" + path + "\", false); req.send(null); if (req.status === 200) { req.responseText } else { null }";
        }
        
        internal static JsonElement GetKeyValueMetatags(this IJSInProcessRuntime jsRuntime, string key, string value)
        {
            return jsRuntime.Invoke<JsonElement>("eval", GetMetatagsJS(key, value));
        }

        internal static JsonElement GetFileJS(this IJSInProcessRuntime jsRuntime, string path)
        {
            return jsRuntime.Invoke<JsonElement>("eval", GetFileJS(path));
        }
    }
}
