h-dependency [![Build status](https://ci.appveyor.com/api/projects/status/t4e3g5n0pju6bsdg/branch/master?svg=true)](https://ci.appveyor.com/project/itajaja/h-dependency/branch/master) [![NuGet Status](http://img.shields.io/nuget/v/Hylasoft.Dependency.svg)](https://www.nuget.org/packages/Hylasoft.Dependency)
============

Dead Simple dependency mechanism provider to achieve loose coupling and easy mocking.
It's a not a pure dependency injection provider, but it offers more flexibility in terms of where it can be used and initialized.

## Requirements

h-dependency works with .NET Framework 4.5 and higher

## Usage

To use the framework, initialize it during the bootstrapping of your application:
````C#
HDependency.Initialize();
````
After that you can register dependencies on it:
````C#
var provider = HDependency.Provider;
provider.Register<IServiceInterface>(new MyServiceImplementation);
````
To retrieve the previously registered interface:
````C#
var myInterface = provider.Get<IServiceInterface>();
````

### Testing
During testing you can tell the Dependency Provider that it's allowed to be re-initialized, by passing an optional `test` parameter to the `Initialize()` method set to true:
````C#
HDependency.Initialize(true);
````

## Build

You can build the project using Visual Studio or by running the grunt tasks for `msbuild`

## Contribute

This project uses [hylasoft/cs-boilerplate](https://github.com/hylasoft-usa/cs-boilerplate) to define tasks and stle guides. Please read the readme of the project to learn more about how to contribute.

You can contribute by opening a pull request. Make sure your code complies with the quality standards by running the following task:

    $ grunt test

## Nuget

A [nuget package](https://www.nuget.org/packages/Hylasoft.Dependency/) is available. To install `Hylasoft.Dependency`, run the following command in the Package Manager Console:

    PM> Install-Package Hylasoft.Dependency
