## OpenAI.OpenAPI.Core

To use NSwag to generate C# code from an OpenAPI (Swagger) definition file in your .csproj file, follow these steps:

### Prerequisites

1. **Install .NET SDK**: Ensure you have the .NET SDK installed. You can download it from the [.NET website](https://dotnet.microsoft.com/download).

2. **Install NSwag**: You can install NSwag globally via NuGet or use it as a .NET CLI tool.

### Step-by-Step Guide

#### 1. Install NSwag CLI Tool

You can install the NSwag CLI tool globally using the following command:

```sh
dotnet tool install -g NSwag.ConsoleCore
```

Alternatively, you can add NSwag to your project as a package:

```sh
dotnet add package NSwag.MSBuild
```

#### 2. Add NSwag Configuration to .csproj

You need to configure NSwag in your .csproj file to generate the C# client code from your OpenAPI.yaml file. Here is how you can do it:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework> <!-- Change this to your target framework -->
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="NSwag.MSBuild" Version="13.15.10" /> <!-- Use the latest version -->
  </ItemGroup>

  <Target Name="NSwag" BeforeTargets="Build">
    <Exec Command="dotnet tool restore" />
    <Exec Command="nswag run /variables:Configuration=$(Configuration)" />
  </Target>

</Project>
```

#### 3. Create NSwag Configuration File

Create an NSwag configuration file, for example `nswag.json`, in your project directory. This file will specify the details for generating the C# code from your OpenAPI.yaml file. Here's an example configuration:

```json
{
  "runtime": "NetCore31",
  "defaultVariables": null,
  "swaggerGenerator": {
    "fromFile": {
      "json": "OpenAPI.yaml",
      "output": null
    }
  },
  "codeGenerators": {
    "openApiToCSharpClient": {
      "clientBaseClass": null,
      "configurationClass": null,
      "generateClientClasses": true,
      "generateClientInterfaces": false,
      "injectHttpClient": true,
      "disposeHttpClient": true,
      "generateExceptionClasses": true,
      "clientClassAccessModifier": "public",
      "useBaseUrl": true,
      "generateBaseUrlProperty": true,
      "generateSyncMethods": false,
      "exposeJsonSerializerSettings": false,
      "clientBaseInterface": null,
      "protectedMethods": [],
      "generateOptionalParameters": false,
      "parameterDateTimeFormat": "s",
      "generateUpdateJsonSerializerSettingsMethod": true,
      "useRequestAndResponseSerializationSettings": false,
      "serializeTypeInformation": false,
      "queryNullValue": "",
      "className": "{controller}Client",
      "operationGenerationMode": "SingleClientFromOperationId",
      "additionalNamespaceUsages": [],
      "additionalContractNamespaceUsages": [],
      "generateResponseClasses": true,
      "responseClass": "SwaggerResponse",
      "wrapResponses": false,
      "wrapResponseMethods": [],
      "generateResponseInterfaces": false,
      "responseClassAccessModifier": "public",
      "exceptionClass": "SwaggerException",
      "useHttpClientCreationMethod": false,
      "httpClientType": "System.Net.Http.HttpClient",
      "useHttpRequestMessageCreationMethod": false,
      "useBaseUrlAsStatic": false,
      "generateDtoTypes": true,
      "generateOptionalParametersMethods": false,
      "clientContractsName": "{controller}Client",
      "generateClientInterfacesName": "I{controller}Client",
      "operationGenerationModeName": "ClientPerOperation",
      "operationGenerationModeNameWithTag": "ClientPerTag",
      "generateDefaultPropertyValues": false,
      "generateOptionalPropertiesAsNullable": true,
      "generateDefaultValues": false,
      "output": "GeneratedClient.cs",
      "newLineBehavior": "Auto",
      "generateDataAnnotations": false,
      "excludedTypeNames": [],
      "excludedParameterNames": []
    }
  }
}
```

#### 4. Run NSwag to Generate C# Code

With the configuration in place, you can now generate the C# client code by running the following command in your project directory:

```sh
dotnet build
```

The `dotnet build` command will execute the `NSwag` target before building the project, generating the C# client code based on your `OpenAPI.yaml` file.

#### 5. Use the Generated Code

The generated C# client code will be saved in the specified output file (`GeneratedClient.cs` in the example). You can include this file in your project and use the generated client classes to interact with your API.

### Summary

1. Install NSwag CLI tool.
2. Configure NSwag in your .csproj file.
3. Create an NSwag configuration file (e.g., `nswag.json`).
4. Run the build command to generate the C# client code.
5. Use the generated client code in your project.

This setup ensures that your C# client code is always up to date with the latest OpenAPI specification whenever you build your project.