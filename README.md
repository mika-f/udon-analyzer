# Udon Analyzer

[JP - 日本語](./README.ja-jp.md) | [EN - English](./README.md)

A collection of analyzers, refactorings, and code-fixes for [UdonSharp](https://github.com/vrchat-community/UdonSharp), powered by [Roslyn](https://github.com/dotnet/roslyn).

## Features

Analyze your UdonSharp (or other C#/VB implementation of Udon) source code and report diagnostics such as compilation errors, unexpected behavior, and more.  
This extension is a feature of the Roslyn C# compiler and can be used with any type of editor.  
Officially supported editors are following:

- Visual Studio 2019 (Community, Pro, Enterprise)
- Visual Studio 2022 (Community, Pro, Enterprise)
- Visual Studio Code
- OmniSharp

You can also run it from outside of the editor. The following execution methods are supported:

- NuGet
- Command-Line Interface
- Unity Integration

## License

MIT by [@6jz](https://twitter.com/6jz)

## Development

1. Clone the repository
2. Create the your changes
3. Test your changes
4. Submit a pull request

## Testing

1. Install the Unity 2022.3.22f1 via Unity Hub
2. Create a Unity Project that have VRChat SDK Worlds installed (requires >= 3.8.2)
3. Generate Solution by the Visual Studio Tools for Unity (VSTU), for Visual Studio (DO NOT CREATE IT FOR VISUAL STUDIO CODE)
4. Set the Environment Variable `UDON_ANALYZER_TARGET_PROJECT` for `<path to your Unity project>/Assembly-CSharp.csproj`
5. Test your changes

## Release

1. Build the solution
2. Run `bin/docgen.exe .`
