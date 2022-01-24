// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;

using ConsoleCore.Tests.Entities;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

using Xunit;

namespace ConsoleCore.Tests;

public class CommandLineParserTest
{
    private (T?, IReadOnlyCollection<IErrorMessage>) Parse<T>(string[] args, bool hasSubCommand = false) where T : class
    {
        var parser = new CommandLineParser(args, hasSubCommand);
        if (parser.TryParse(typeof(T), out var obj, out var errors))
            return (obj as T, errors);
        return (null, errors);
    }

    [Theory]
    [InlineData("test", new[] { "-vtest" })]
    [InlineData("test", new[] { "-v", "test" })]
    [InlineData("hile", new[] { "-v", "hile" })]
    [InlineData("hile", new[] { "-vhile" })]
    public void ParseOptionsWithShortName(string expected, string[] args)
    {
        var (obj, errors) = Parse<ParseStringTestEntity>(args);

        Assert.Equal(expected, obj?.Value);
        Assert.Equal(0, errors.Count);
    }

    [Theory]
    [InlineData("str", 1, new[] { "str", "1" })]
    [InlineData("hello", 5000, new[] { "hello", "5000" })]
    [InlineData("foobar", -250, new[] { "foobar", "-250" })]
    public void ParseOrderedOptions(string expectedStr, int expectedInt, string[] args)
    {
        var (obj, errors) = Parse<ParseOrderedOptionsEntity>(args);

        Assert.Equal(expectedStr, obj?.StrValue);
        Assert.Equal(expectedInt, obj?.IntValue);
        Assert.Equal(0, errors.Count);
    }

    [Theory]
    [InlineData("test", new[] { "commit", "-vtest" })]
    [InlineData("test", new[] { "commit", "-v", "test" })]
    [InlineData("hile", new[] { "commit", "-v", "hile" })]
    [InlineData("hile", new[] { "commit", "-vhile" })]
    public void ParseOptionsWithShortNameInVerbs(string expected, string[] args)
    {
        var (obj, errors) = Parse<ParseStringTestEntity>(args, true);

        Assert.Equal(expected, obj?.Value);
        Assert.Equal(0, errors.Count);
    }

    [Fact]
    public void ParseRepeatedOptionsWithShortNameInVerbs()
    {
        var (obj, errors) = Parse<ParseStringTestEntity>(new[] { "commit", "-v", "https://example.com", "-v", "https://example.com" }, true);

        Assert.Null(obj?.Value);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public void ParseOptions()
    {
        var (obj, errors) = Parse<ParseStringTestEntity>(new[] { "--stringValue", "strValue" });

        Assert.Equal("strValue", obj?.Value);
        Assert.Equal(0, errors.Count);
    }

    [Fact(Skip = "NotImplementation")]
    public void ParseOptionsWithDoubleDash()
    {
        var (obj, errors) = Parse<ParseOptionsWithDoubleDashEntity>(new[] { "--stringValue", "strValue", "--", "20", "---aaa", "-b", "--ccc", "30" });

        Assert.Equal("strValue", obj?.StrValue);
        Assert.Equal(20, obj?.IntValue);
        Assert.Equal("---aaa", obj?.StrValue1);
        Assert.Equal("-b", obj?.StrValue2);
        Assert.Equal("--ccc", obj?.StrValue3);
        Assert.Equal(30L, obj?.LongValue);
        Assert.Equal(0, errors.Count);
    }


    [Fact]
    public void ParseRepeatedOptionsWithLongName()
    {
        var (obj, errors) = Parse<ParseStringTestEntity>(new[] { "--strValue", "https://example.com", "--strValue", "https://example.com" });

        Assert.Null(obj?.Value);
        Assert.Equal(1, errors.Count);
    }

    [Fact]
    public void ParseRepeatedOptionsWithShortName()
    {
        var (obj, errors) = Parse<ParseStringTestEntity>(new[] { "-v", "https://example.com", "-v", "https://example.com" });

        Assert.Null(obj?.Value);
        Assert.Equal(1, errors.Count);
    }
}