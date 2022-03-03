// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.Models;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using static NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Syntax.SyntaxFactory2;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;

public static class UdonSharpAnalyzerGenerator
{
    public static CompilationUnitSyntax CreateGeneratedAnalyzerCode(string name, string? runtimeMinVersion, string? runtimeMaxVersion, string? compilerMinVersion, string? compilerMaxVersion)
    {
        var members = new List<MemberDeclarationSyntax>
        {
            NamespaceDeclaration(ParseName("NatsunekoLaboratory.UdonAnalyzer.UdonSharp"))
                .WithMembers(
                    CreateSyntaxList<MemberDeclarationSyntax>(
                        ClassDeclaration(name, runtimeMinVersion, runtimeMaxVersion, compilerMinVersion, compilerMaxVersion)
                    )
                )
        };

        return CompilationUnit(
            Empty<ExternAliasDirectiveSyntax>(),
            UsingDirectives(),
            Empty<AttributeListSyntax>(),
            CreateSyntaxList(members));
    }


    private static SyntaxList<UsingDirectiveSyntax> UsingDirectives()
    {
        var usings = new List<UsingDirectiveSyntax>
        {
            UsingDirective(ParseName("Microsoft.CodeAnalysis")),
            UsingDirective(ParseName("Microsoft.CodeAnalysis.CSharp")),
            UsingDirective(ParseName("Microsoft.CodeAnalysis.CSharp.Syntax")),
            UsingDirective(ParseName("Microsoft.CodeAnalysis.Diagnostics")).WithNewLineTrivia(),
            UsingDirective(ParseName("NatsunekoLaboratory.UdonAnalyzer.Attributes")),
            UsingDirective(ParseName("NatsunekoLaboratory.UdonAnalyzer.Internal"))
        };

        return CreateSyntaxList(usings);
    }

    private static ClassDeclarationSyntax ClassDeclaration(string name, string? runtimeMinVersion, string? runtimeMaxVersion, string? compilerMinVersion, string? compilerMaxVersion)
    {
        var members = new List<MemberDeclarationSyntax>
        {
            SupportedDiagnosticOverrideProperty(name),
            InitializeOverrideMethod()
        };

        return SyntaxFactory.ClassDeclaration(name)
                            .WithModifiers(new SyntaxTokenList(Token(SyntaxKind.PublicKeyword)))
                            .WithAttributeLists(DescribeAttributes(runtimeMinVersion, runtimeMaxVersion, compilerMinVersion, compilerMaxVersion))
                            .WithBaseList(BaseClasses())
                            .WithMembers(CreateSyntaxList(members));
    }

    private static SyntaxList<AttributeListSyntax> DescribeAttributes(string? minRuntimeVersion, string? maxRuntimeVersion, string? minCompilerVersion, string? maxCompilerVersion)
    {
        var attributes = new List<AttributeSyntax>
        {
            DescribeDiagnosticAnalyzerAttribute(),
            DescribeRequireUdonVersionAttribute(minRuntimeVersion, maxRuntimeVersion),
            DescribeRequireUdonSharpVersionAttribute(minCompilerVersion, maxCompilerVersion)
        };

        return CreateSyntaxList(Create(AttributeList, attributes.ToArray()));
    }

    private static AttributeSyntax DescribeDiagnosticAnalyzerAttribute()
    {
        var args = new List<ExpressionSyntax>
        {
            ParseExpression("LanguageNames.CSharp")
        };

        return Attribute("DiagnosticAnalyzer", args.ToArray());
    }

    private static AttributeSyntax DescribeRequireUdonVersionAttribute(string? minRuntimeVersion, string? maxRuntimeVersion)
    {
        var args = new List<ExpressionSyntax>();
        if (!string.IsNullOrWhiteSpace(minRuntimeVersion) && string.IsNullOrWhiteSpace(maxRuntimeVersion)) // min only
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(new VersionRangeGreaterThanOrEquals(minRuntimeVersion).ToRangeString())));
        if (string.IsNullOrWhiteSpace(minRuntimeVersion) && !string.IsNullOrWhiteSpace(maxRuntimeVersion)) // max only
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(new VersionRangeLessThanOrEquals(maxRuntimeVersion).ToRangeString())));
        if (!string.IsNullOrWhiteSpace(minRuntimeVersion) && !string.IsNullOrWhiteSpace(maxRuntimeVersion)) // max only
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(new VersionRangeGreaterThanOrEqualsAndLessThanOrEquals(minRuntimeVersion, maxRuntimeVersion).ToRangeString())));
        if (string.IsNullOrWhiteSpace(minRuntimeVersion) && string.IsNullOrWhiteSpace(maxRuntimeVersion)) // not provided
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("[2021.11.24.16.19,)")));

        return Attribute("RequireUdonVersion", args.ToArray());
    }

    private static AttributeSyntax DescribeRequireUdonSharpVersionAttribute(string? minCompilerVersion, string? maxCompilerVersion)
    {
        var args = new List<ExpressionSyntax>();
        if (!string.IsNullOrWhiteSpace(minCompilerVersion) && string.IsNullOrWhiteSpace(maxCompilerVersion)) // min only
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(new VersionRangeGreaterThanOrEquals(minCompilerVersion).ToRangeString())));
        if (string.IsNullOrWhiteSpace(minCompilerVersion) && !string.IsNullOrWhiteSpace(maxCompilerVersion)) // max only
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(new VersionRangeLessThanOrEquals(maxCompilerVersion).ToRangeString())));
        if (!string.IsNullOrWhiteSpace(minCompilerVersion) && !string.IsNullOrWhiteSpace(maxCompilerVersion)) // max only
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(new VersionRangeGreaterThanOrEqualsAndLessThanOrEquals(minCompilerVersion, maxCompilerVersion).ToRangeString())));
        if (string.IsNullOrWhiteSpace(minCompilerVersion) && string.IsNullOrWhiteSpace(maxCompilerVersion)) // not provided
            args.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("[0.20.3,)")));


        return Attribute("RequireUdonSharpCompilerVersion", args.ToArray());
    }

    private static BaseListSyntax BaseClasses()
    {
        return BaseList(CreateSeparatedSyntaxList<BaseTypeSyntax>(SimpleBaseType(IdentifierName("BaseDiagnosticAnalyzer"))));
    }

    private static MemberDeclarationSyntax SupportedDiagnosticOverrideProperty(string name)
    {
        return PropertyDeclaration(ParseTypeName("DiagnosticDescriptor"), "SupportedDiagnostic")
               .WithModifiers(new SyntaxTokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
               .WithExpressionBody(ArrowExpressionClause(ParseExpression($"DiagnosticDescriptors.{name.Substring(0, name.LastIndexOf("Analyzer", StringComparison.InvariantCulture))}")))
               .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
               .WithNewLineTrivia();
    }

    private static MemberDeclarationSyntax InitializeOverrideMethod()
    {
        var parameter = Parameter(Identifier("context")).WithType(ParseTypeName("AnalysisContext"));
        var arguments = new List<ExpressionSyntax>
        {
            IdentifierName("context")
        };
        var body = new List<StatementSyntax>
        {
            ExpressionStatement(InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, BaseExpression(), IdentifierName("Initialize")), Create(ArgumentList, arguments.Select(Argument).ToArray())))
        };

        return MethodDeclaration(ParseTypeName("void"), "Initialize")
               .WithModifiers(new SyntaxTokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
               .WithParameterList(Create(ParameterList, parameter))
               .WithBody(Block(body.ToArray()));
    }

    public static FieldDeclarationSyntax CreateGeneratedDescriptorCode(string id, string name, string title, string messageFormat, string category, DiagnosticSeverity severity, string description)
    {
        var arguments = new List<ExpressionSyntax>
        {
            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(id)).WithNewLineTrivia(),
            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(title)).WithNewLineTrivia(),
            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(messageFormat)).WithNewLineTrivia(),
            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DiagnosticCategories"), IdentifierName(category)).WithNewLineTrivia(),
            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DiagnosticSeverity"), IdentifierName(severity.ToString())).WithNewLineTrivia()
        };

        if (messageFormat != description)
        {
            arguments.Add(LiteralExpression(SyntaxKind.TrueLiteralExpression).WithNewLineTrivia());
            arguments.Add(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(description)).WithNewLineTrivia());
        }

        var expression = InvocationExpression(
            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DiagnosticDescriptorFactory"), IdentifierName("Create"))
        ).WithArgumentList(Create(ArgumentList, arguments.Select(Argument).ToArray()));

        var variable = VariableDeclarator(Identifier(name.Substring(0, name.LastIndexOf("Analyzer", StringComparison.InvariantCulture)))).WithInitializer(EqualsValueClause(expression));

        return FieldDeclaration(VariableDeclaration(ParseTypeName("DiagnosticDescriptor"), CreateSeparatedSyntaxList(variable)))
               .WithModifiers(Modifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword))
               .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
               .WithNewLineTrivia();
    }
}