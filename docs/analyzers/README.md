# List of Analyzers

UdonAnalyzer supports the following two types of Analyzers and CodeFixes.

- Udon (`VRC`) : Assigned to Diagnostics by Udon Virtual Machine limitations.
- UdonSharp (`VSC`) : Assigned to Diagnostics by UdonSharp Compiler limitations.

## Categories

All analyzers belongs to one of the following categories.

| Category Name   | Description                          |
| --------------- | ------------------------------------ |
| Compiler        | An analyzer for Compiler limitations |
| Maintainability | An analyzer for code maintainability |
| Security        | An analyzer for security issues      |
| Unexpected      | An analyzer for unexpected behaviour |
| Usage           | An analyzer for API usages           |

## Compiler (Language) Version

Udon Analyzer is designed to work with a C# version that matches the Unity's C# version.  
The current VRChat requires Unity 2019.x, which can use C# 7.3.  
However, the Roslyn Compiler (`Microsoft.CodeAnalysis.CSharp`) used by UdonSharp is 2.6.0, which supports language features up to C# 7.2.
