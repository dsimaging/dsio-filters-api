# .Net SDK

The dotnet SDK contains helpful libraries and sample code written in C#.

## The SDK Library
The `lib` folder contains library projects that implement types used in the API and a client library that wraps HTTP calls to the service. The `ServiceProxy` class in the SDK wraps HTTP calls and converts JSON data allowing users to make simple calls to retrieve data. The `ServiceProxy` class will throw exceptions for most errors and appropriate exception handling should be used to handle these cases.


## Sample Code
The `samples` folder contains sample applications that demonstrate the use of the .Net SDK libraries.

### WpfSample
The `Wpfsample` demonstrates use of the `ServiceProxy` to create Image resources and apply filters.

## Build
The SDK and samples were developed using the .Net SDK and Visual Studio Code. If you open this folder (sdk/dotnet) in Visual Studio Code, there are build and launch tasks available to build and run the source. A Powershell script, `build.ps1` is available that will use the dotnet CLI to build the SDK and samples. There is also a Visual Studio Solution available to build the SDK and Samples in Visual Studio 2019.

The SDK libraries are generated in the `lib/bin` folder. Here you will find the assemblies that you can use in your .Net code to help implement and consume the DSIO Filters Api.
