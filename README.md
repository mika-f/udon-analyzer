# Udon Analyzer

A collection of [analyzers](./src/Analyzers/README.md), [refactorings](./src/Refactorings/README.md), and [code-fixes](./src/CodeFixes/README.md) for [UdonSharp](https://github.com/vrchat-community/UdonSharp), powered by [Roslyn](https://github.com/dotnet/roslyn).

## Features

Analyze your UdonSharp (or other C#/VB implementation of Udon) source code and report diagnostics such as compilation errors, unexpected behavior, and more.  
This extension is a feature of the Roslyn C# compiler and can be used with any type of editor.  
Officially supported editors are following:

- Visual Studio 2019 (Community, Pro, Enterprise)
- Visual Studio 2022 (Community, Pro, Enterprise)
- Visual Studio Code
  - Note: Extensions for Visual Studio Code is not included in this repository. Visit [here](https://github.com/natsuneko-laboratory/udon-analyzer-vscode).
- OmniSharp

You can also run it from outside of the editor. The following execution methods are supported:

- NuGet
- Command-Line Interface
- Unity Integration
  - Note: Unity Integration is not included in this repository. Visit [here](https://github.com/natsuneko-laboratory/udon-analyzer-unity).

## License

MIT by [@6jz](https://twitter.com/6jz)
