// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Extensions;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using static NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Syntax.SyntaxFactory2;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;

public static class UdonSharpAnalyzerTestGenerator
{
    public static CompilationUnitSyntax CreateGeneratedTestCode(string id, string title, string category)
    {
        Contract.Assert(category is "Udon" or "UdonSharp");

        var members = new List<MemberDeclarationSyntax>
        {
            NamespaceDeclaration(ParseName($"Analyzers.Tests.{category}"))
                .WithMembers(
                    CreateSyntaxList<MemberDeclarationSyntax>(
                        ClassDeclaration(title, category)
                    )
                )
        };

        return CompilationUnit(
            Empty<ExternAliasDirectiveSyntax>(),
            UsingDirectives(category),
            Empty<AttributeListSyntax>(),
            CreateSyntaxList(members)
        );
    }

    private static SyntaxList<UsingDirectiveSyntax> UsingDirectives(string category)
    {
        var usings = new List<UsingDirectiveSyntax>
        {
            UsingDirective(ParseName("System.Threading.Tasks")).WithNewLineTrivia(),
            UsingDirective(ParseName("NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes")),
            UsingDirective(ParseName("NatsunekoLaboratory.UdonAnalyzer.Testing")),
            UsingDirective(ParseName($"NatsunekoLaboratory.UdonAnalyzer.{category}")).WithNewLineTrivia(),
            UsingDirective(ParseName("Xunit"))
        };

        return CreateSyntaxList(usings);
    }

    private static ClassDeclarationSyntax ClassDeclaration(string title, string category)
    {
        var members = new List<MemberDeclarationSyntax>
        {
            TestMethodWithBody()
        };

        return SyntaxFactory.ClassDeclaration($"{title}Test")
                            .WithModifiers(new SyntaxTokenList(Token(SyntaxKind.PublicKeyword)))
                            .WithAttributeLists(DescribeAttribute(title, category))
                            .WithBaseList(BaseClasses(title))
                            .WithMembers(CreateSyntaxList(members));
    }

    private static SyntaxList<AttributeListSyntax> DescribeAttribute(string id, string category)
    {
        var args = new List<ExpressionSyntax>
        {
            ParseExpression($"typeof({id})"),
            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(category == "UdonSharp" ? "VSC" : "VRC"))
        };

        return CreateSyntaxList(Create(AttributeList, Attribute("Describe", args.ToArray())));
    }

    private static BaseListSyntax BaseClasses(string id)
    {
        var typeArgs = TypeArgumentList(CreateSeparatedSyntaxList(ParseTypeName(id)));
        return BaseList(CreateSeparatedSyntaxList<BaseTypeSyntax>(SimpleBaseType(GenericName(Identifier("UdonSharpDiagnosticVerifier"), typeArgs))));
    }

    private static MemberDeclarationSyntax TestMethodWithBody()
    {
        return MethodDeclaration(ParseTypeName("Task"), "TestDiagnostic_XXX")
               .WithAttributeLists(CreateSyntaxList(Create(AttributeList, Attribute("Fact"), Attribute("Example"))))
               .WithModifiers(new SyntaxTokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.AsyncKeyword)))
               .WithBody(TestMethodBody());
    }

    private static BlockSyntax TestMethodBody()
    {
        var statements = new List<StatementSyntax>
        {
            ExpressionStatement(AwaitExpression(ParseExpression("VerifyAnalyzerAsync(@\"\")"))),
            ThrowStatement(ParseExpression("new System.NotImplementedException()"))
        };

        return Block(statements.ToArray());
    }
}