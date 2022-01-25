// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

internal abstract class VersionRange
{
    protected GenericVersion MinVersion { get; }

    protected GenericVersion MaxVersion { get; }

    protected VersionRange(string min, string max)
    {
        MinVersion = GenericVersion.Parse(min);
        MaxVersion = GenericVersion.Parse(max);
    }

    public abstract bool IsFulfill(string version);

    public static VersionRange Parse(string str)
    {
        return TryParse(str, out var r) ? r : new VersionRangeInvalid();
    }

    public static bool TryParse(string str, [NotNullWhen(true)] out VersionRange? result)
    {
        var r = new VersionRangeResult();
        var b = TryParseVersionRange(str, ref r);

        result = r.ParsedVersionRange;
        return b;
    }

    private static bool TryParseVersionRange(string str, ref VersionRangeResult result)
    {
        // 1.0 meaning x >= 1.0
        if (GenericVersion.TryParse(str, out _))
        {
            result.ParsedVersionRange = new VersionRangeGreaterThanOrEquals(str);
            return true;
        }

        var range = $"{str[0]}{str[^1]}";
        var versions = str[1..^1].Split(',');

        switch (range)
        {
            // [1.0] meaning x == 1.0
            case "[]" when versions.Length == 1 && GenericVersion.TryParse(versions[0], out _):
                result.ParsedVersionRange = new VersionRangeEquals(versions[0]);
                return true;

            // (,1.0] meaning x <= 1.0
            case "(]" when versions.Length == 2 && versions[0] == "" && GenericVersion.TryParse(versions[1], out _):
                result.ParsedVersionRange = new VersionRangeLessThanOrEquals(versions[1]);
                return true;

            // [1.0,) meaning 1.0 <= x
            case "[)" when versions.Length == 2 && GenericVersion.TryParse(versions[0], out _) && versions[1] == "":
                result.ParsedVersionRange = new VersionRangeGreaterThanOrEquals(versions[0]);
                return true;

            // [1.0,1.1] meaning 1.0 <= x <= 1.1
            case "[]" when versions.Length == 2 && versions.All(w => GenericVersion.TryParse(w, out _)):
                result.ParsedVersionRange = new VersionRangeGreaterThanOrEqualsAndLessThanOrEquals(versions[0], versions[1]);
                return true;

            // [1.0,1.1) meaning 1.0 <= x < 1.1
            case "[)" when versions.Length == 2 && versions.All(w => GenericVersion.TryParse(w, out _)):
                result.ParsedVersionRange = new VersionRangeGreaterThanOrEqualsAndLessThan(versions[0], versions[1]);
                return true;

            default:
                return false;
        }
    }

    public override string ToString()
    {
        if (MinVersion == MaxVersion)
            return MinVersion.ToString();
        if (MaxVersion.ToString() == "0")
            return $"{MinVersion} ~ latest";
        if (MinVersion.ToString() == "0")
            return $"initial ~ {MaxVersion}";
        return $"{MinVersion} ~ {MaxVersion}";
    }

    private struct VersionRangeResult
    {
        public VersionRange ParsedVersionRange { get; set; }
    }
}