[![Build Status](https://dev.azure.com/fariarto/Fario.Extensions.Configuration/_apis/build/status/BrannenGH.BlazorDomConfiguration?branchName=master)](https://dev.azure.com/fariarto/Fario.Extensions.Configuration/_build/latest?definitionId=1&branchName=master)

> WARNING: Do not store sensitive data like connection strings or keys in the DOM, as any configuration done here is publicly available.

# Blazer DOM Configuration

This extension allows creating an [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) by placing `<meta>` tags in the header of the document the Blazor app is called from.

## Approach

While most ASP.NET applications support [importing a configuration at startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/), this is difficult to do in a client-side Blazor application. This is because the limited browser environment, which usually just consists of the DOM, your app assembly, and some CSS files, doesn't have a good place to put configuration key value pairs that can be accessed at application bootstrapping time. Currently, the approach most applications are taking is to put the configuration information into the assembly, but this means that any change in configuration requires creating a new assembly.

To avoid creating a new assembly, this approach bakes configuration values into the DOM. This still means that the document structure is tied to the configuration, but it is much easier to manage many different `index.html` files than it is to manage many different assemblies.

## How to Use

This library requires two steps — adding the configuration key-value pairs to the DOM, and then building the `IConfiguration`.

### How to Setup the DOM

In the `<head>` tag of your `index.html` document, you can add a configuration key-value pair in the Blazor `IConfiguration` by adding a tag like so:
```HTML
<meta data-name="Key" data-value="Value">
```

So that you end up with something like this:

```HTML
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width" />
    <title>My App</title>
    <base href="/" />
    <link href="css/site.css" rel="stylesheet" />
    <meta data-name="Key" data-value="Value">
</head>
<body>
    <app>Loading...</app>

    <script src="_framework/blazor.webassembly.js"></script>
</body>
</html>
```

The meta tag attributes are [prefixed by data](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/data-*) to ensure the resulting DOM is still HTML5 standards compliant.

### How to Setup the Blazor project

Add the [NuGet package](https://www.nuget.org/packages/Fario.Extensions.Configuration) to your csproj, and then add the following to `Startup.ConfigureServices`:

```C#
public void ConfigureServices(IServiceCollection services)
{
	// Other service logic


	services.AddSingleton<IConfiguration>(
		new ConfigurationBuilder().AddDomConfiguration(
			services.BuildServiceProvider().GetService<IJSRuntime>() as IJSInProcessRuntime ?? throw new Exception("No in process runtime could be found!")).Build()
		);
		
	// Other service logic
}
```

This library requires the use of the [IJSInProcessRuntime](https://docs.microsoft.com/en-us/dotnet/api/microsoft.jsinterop.ijsinprocessruntime) to access the DOM to extract the <meta> tags. This can normally be found in the IServiceCollection that gets passed to ConfigureServices at configuration time.

## Future Goals

* Add tests
* Support creating a configuration hierarchy, maybe by adding a "namespace" property to the key value pair
* Support loading a different file, i.e. an appsettings.json file from the server at startup to parse into a configuration.
