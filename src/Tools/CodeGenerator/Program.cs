// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Models;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

var compilerCommand = SubCommand.Create<GenerateCompilerAnalyzerParameters>(args => args.GenerateCompilerAnalyzerCode());
var runtimeCommand = SubCommand.Create<GenerateRuntimeAnalyzerParameters>(args => args.GenerateRuntimeAnalyzerCode());

var analyzerCommand = SubCommand.Create()
                                .AddCommand("compiler", compilerCommand)
                                .AddCommand("runtime", runtimeCommand);

var codeFixCommand = SubCommand.Create<GenerateCodeFixesParameters>(args => args.GenerateCodeFixCode());

return await ConsoleHost.Create()
                        .AddCommand("analyzer", analyzerCommand)
                        .AddCommand("codefix", codeFixCommand)
                        .RunAsync(args);