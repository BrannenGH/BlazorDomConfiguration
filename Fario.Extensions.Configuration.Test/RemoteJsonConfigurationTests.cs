using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Fario.Extensions.Configuration.Test
{
    public class RemoteJsonConfigurationTests: ConfigurationTestBase
    {
        private readonly string path = "";
        
        private readonly Mock<IJSInProcessRuntime> mockJsRuntime;
        private readonly JsonElement nullResult = JsonDocument.Parse("null").RootElement;
        private readonly JsonElement notJsonResult = JsonDocument.Parse("\"User-agent: *\"").RootElement;
        private readonly JsonElement jsonResult = JsonDocument.Parse("\"{ \\\"fruit\\\": \\\"orange\\\" }\"").RootElement;

        public RemoteJsonConfigurationTests()
        {
            mockJsRuntime = new Mock<IJSInProcessRuntime>();
        }

        [Fact]
        public void AddRemoteJsonConfiguration_Null_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, nullResult);

            Assert.Throws<FileNotFoundException>(() => BuildConfiguration(mockJsRuntime.Object));
        }

        [Fact]
        public void AddRemoteJsonConfiguration_NotJson_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, notJsonResult);

            Assert.ThrowsAny<JsonException>(() => BuildConfiguration(mockJsRuntime.Object));
        }

        [Fact]
        public void AddRemoteJsonConfiguration_ValidJson_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, jsonResult);

            var configuration = BuildConfiguration(mockJsRuntime.Object);

            Assert.Equal(
                configuration.AsEnumerable().OrderByDescending(x => x.Key),
                SingleEntryResult.OrderByDescending(x => x.Key)
            );
        }

        private IConfiguration BuildConfiguration(IJSInProcessRuntime jsInProcessRuntime)
        {
            return new ConfigurationBuilder().AddRemoteJsonConfiguration(jsInProcessRuntime, path).Build();
        }

        private void SetupRuntime(Mock<IJSInProcessRuntime> runtime, JsonElement returnValue)
        {
            runtime.Setup((runtime) => runtime.Invoke<JsonElement>(
                It.IsAny<string>(),
                JSFunctions.GetFileJS(path)))
            .Returns(returnValue);
        }

    }
}
