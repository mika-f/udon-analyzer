// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;
using NatsunekoLaboratory.UdonAnalyzer.TestCodeGenerator.Models;

return await ConsoleHost.Create<GenerateDiagnosticTestParameters>(args => args.GenerateTestCodeAsync())
                        .RunAsync(args);