using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Xunit;

namespace Fario.Extensions.Configuration.Test
{
    public class DomConfigurationTests: ConfigurationTestBase
    {
        private readonly string keyLabel = "data-name";
        private readonly string valueLabel = "data-value";
        
        private readonly Mock<IJSInProcessRuntime> mockJsRuntime;
        private readonly JsonElement emptyResult = JsonDocument.Parse("[]").RootElement;
        private readonly JsonElement singleResult = JsonDocument.Parse("[{\"key\": \"fruit\", \"value\":\"orange\"}]").RootElement;
        private readonly JsonElement internationalEntry = JsonDocument.Parse("[{\"key\": \"trái cây\", \"value\":\"cam\"}]").RootElement;
        private readonly JsonElement multipleEntry = JsonDocument.Parse("[{\"key\": \"fruit\", \"value\":\"orange\"}, {\"key\": \"vegetable\", \"value\":\"broccoli\"}]").RootElement;
        private readonly JsonElement duplicateEntry = JsonDocument.Parse("[{\"key\": \"fruit\", \"value\":\"orange\"}, {\"key\": \"fruit\", \"value\":\"strawberry\"}]").RootElement;
        public DomConfigurationTests()
        {
            mockJsRuntime = new Mock<IJSInProcessRuntime>();
        }

        [Fact]
        public void AddDomConfiguration_Empty_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, emptyResult);

            var configuration = BuildConfiguration(mockJsRuntime.Object);

            Assert.Empty(configuration.AsEnumerable());
        }

        [Fact]
        public void AddDomConfiguration_Single_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, singleResult);

            var configuration = BuildConfiguration(mockJsRuntime.Object);
            Assert.Equal(
                configuration.AsEnumerable().OrderByDescending(x => x.Key), 
                SingleEntryResult.OrderByDescending(x => x.Key)
            );
        }

        [Fact]
        public void AddDomConfiguration_NonASCII_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, internationalEntry);

            var configuration = BuildConfiguration(mockJsRuntime.Object);
            Assert.Equal(
                configuration.AsEnumerable().OrderByDescending(x => x.Key), 
                InternationalEntryResult.OrderByDescending(x => x.Key)
           );
        }

        [Fact]
        public void AddDomConfiguration_Multiple_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, multipleEntry);

            var configuration = BuildConfiguration(mockJsRuntime.Object);
            Assert.Equal(
                configuration.AsEnumerable().OrderByDescending(x => x.Key), 
                MultipleEntryResult.OrderByDescending(x => x.Key)
            );
        }

        [Fact]
        public void AddDomConfiguration_Duplicate_BuildConfiguration()
        {
            SetupRuntime(mockJsRuntime, duplicateEntry);

            var configuration = BuildConfiguration(mockJsRuntime.Object);
            Assert.Equal(
                configuration.AsEnumerable().OrderByDescending(x => x.Key), 
                DuplicateEntryResult.OrderByDescending(x => x.Key)
            );
        }


        private IConfiguration BuildConfiguration(IJSInProcessRuntime jsInProcessRuntime)
        {
            return new ConfigurationBuilder().AddDomConfiguration(jsInProcessRuntime, keyLabel, valueLabel).Build();
        }

        private void SetupRuntime(Mock<IJSInProcessRuntime> runtime, JsonElement returnValue)
        {
            runtime.Setup((runtime) => runtime.Invoke<JsonElement>(
                It.IsAny<string>(),
                JSFunctions.GetMetatagsJS(keyLabel, valueLabel)))
                .Returns(returnValue);
        }
    }
}
