// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

/// <summary>
///     GenericVersion supports x.x.x.x (semantic) and x.x.x.x.x (datetime) versioning
/// </summary>
public class GenericVersion : IComparable<GenericVersion>, IEquatable<GenericVersion>
{
    private static readonly Regex ValidVersionPattern = new(@"^([1-9]\d*|0)(\.\d+(\.\d+(-(alpha|beta|rc|canary))?(\.\d+(\.\d+)?)?)?)?$", RegexOptions.Compiled);
    private static readonly Regex ValidPreReleaseTagPattern = new(@"^\d-(alpha|beta|rc|canary)$", RegexOptions.Compiled);

    public int Major { get; }
    public int Minor { get; } = -1;
    public int Build { get; } = -1;
    public int MajorRevision { get; } = -1;
    public int MinorRevision { get; } = -1;

    public GenericVersion(int major)
    {
        if (major < 0)
            throw new ArgumentOutOfRangeException(nameof(major));

        Major = major;
    }

    public GenericVersion(int major, int minor)
    {
        if (major < 0)
            throw new ArgumentOutOfRangeException(nameof(major));

        if (minor < 0)
            throw new ArgumentOutOfRangeException(nameof(minor));

        Major = major;
        Minor = minor;
    }

    public GenericVersion(int major, int minor, int build)
    {
        if (major < 0)
            throw new ArgumentOutOfRangeException(nameof(major));

        if (minor < 0)
            throw new ArgumentOutOfRangeException(nameof(minor));

        if (build < 0)
            throw new ArgumentOutOfRangeException(nameof(build));

        Major = major;
        Minor = minor;
        Build = build;
    }

    public GenericVersion(int major, int minor, int build, int revision)
    {
        if (major < 0)
            throw new ArgumentOutOfRangeException(nameof(major));

        if (minor < 0)
            throw new ArgumentOutOfRangeException(nameof(minor));

        if (build < 0)
            throw new ArgumentOutOfRangeException(nameof(build));


        Major = major;
        Minor = minor;
        Build = build;
        MajorRevision = revision;
    }

    public GenericVersion(int major, int minor, int build, int majorRevision, int minorRevision)
    {
        if (major < 0)
            throw new ArgumentOutOfRangeException(nameof(major));

        if (minor < 0)
            throw new ArgumentOutOfRangeException(nameof(minor));

        if (build < 0)
            throw new ArgumentOutOfRangeException(nameof(build));


        Major = major;
        Minor = minor;
        Build = build;
        MajorRevision = majorRevision;
        MinorRevision = minorRevision;
    }

    public GenericVersion(string version)
    {
        var v = Parse(version);

        Major = v.Major;
        Minor = v.Minor;
        Build = v.Build;
        MajorRevision = v.MajorRevision;
        MinorRevision = v.MinorRevision;
    }

    public int CompareTo(GenericVersion other)
    {
        if (Major != other.Major)
            if (Major > other.Major)
                return 1;
            else
                return -1;

        if (Minor != other.Minor)
            if (Minor > other.Minor)
                return 1;
            else
                return -1;

        if (Build != other.Build)
            if (Build > other.Build)
                return 1;
            else
                return -1;

        if (MajorRevision != other.MajorRevision)
            if (MajorRevision > other.MajorRevision)
                return 1;
            else
                return -1;

        if (MinorRevision != other.MinorRevision)
            if (MinorRevision > other.MinorRevision)
                return 1;
            else
                return -1;

        return 0;
    }

    public bool Equals(GenericVersion other)
    {
        return Major == other.Major && Minor == other.Minor && Build == other.Build && MajorRevision == other.MajorRevision && MinorRevision == other.MinorRevision;
    }

    public override string ToString()
    {
        if (Minor == -1)
            return ToString(1);
        if (Build == -1)
            return ToString(2);
        if (MajorRevision == -1)
            return ToString(3);
        if (MinorRevision == -1)
            return ToString(4);
        return ToString(5);
    }

    private string ToString(int fields)
    {
        var sb = new StringBuilder();

        switch (fields)
        {
            case 1:
                sb.Append(Major);
                break;

            case 2:
                sb.AppendFormat("{0}.{1}", Major, Minor);
                break;

            case 3:
                sb.AppendFormat("{0}.{1}.{2}", Major, Minor, Build);
                break;

            case 4:
                sb.AppendFormat("{0}.{1}.{2}.{3}", Major, Minor, Build, MajorRevision);
                break;

            case 5:
                sb.AppendFormat("{0}.{1}.{2}.{3}.{4}", Major, Minor, Build, MajorRevision, MinorRevision);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(fields));
        }

        return sb.ToString();
    }

    public static GenericVersion Parse(string version)
    {
        if (TryParse(version, out var result))
            return result;

        throw new FormatException("invalid version format string");
    }

    public static bool TryParse(string version, [NotNullWhen(true)] out GenericVersion? result)
    {
        var r = new GenericVersionResult();
        var b = TryParseVersion(version, ref r);

        result = r.ParsedVersion;
        return b;
    }

    private static bool TryParseVersion(string version, ref GenericVersionResult result)
    {
        if (!ValidVersionPattern.IsMatch(version))
            return false;

        var components = version.Split('.');
        var parsedComponentsLength = 0;
        var hasTag = false;
        var tag = "";
        int major = 0, minor = 0, build = 0, majorRevision = 0, minorRevision = 0;

        if (components.Length >= 5)
        {
            if (!int.TryParse(components[4], out minorRevision))
                return false;

            parsedComponentsLength++;
        }

        if (components.Length >= 4)
        {
            if (!int.TryParse(components[3], out majorRevision))
                return false;

            parsedComponentsLength++;
        }

        if (components.Length >= 3)
        {
            if (!int.TryParse(components[2], out build))
            {
                if (!ValidPreReleaseTagPattern.IsMatch(components[2]))
                    return false;

                var c = components[2].Split('-');
                if (!int.TryParse(c[0], out build))
                    return false;
                tag = c[1];
                hasTag = true;
            }

            parsedComponentsLength++;
        }

        if (components.Length >= 2)
        {
            if (!int.TryParse(components[1], out minor))
                return false;

            parsedComponentsLength++;
        }

        if (components.Length >= 1)
        {
            if (!int.TryParse(components[0], out major))
                return false;

            parsedComponentsLength++;
        }

        if (hasTag)
            // 1.0.1 > 1.0.1-rc.1 > 1.0.1-beta.1 > 1.0.1-alpha.2 > 1.0.1-alpha.1 > 1.0.1-canary.1 > 1.0.0
            switch (tag)
            {
                case "canary":
                    majorRevision -= 100;
                    break;

                case "alpha":
                    majorRevision -= 99;
                    break;

                case "beta":
                    majorRevision -= 98;
                    break;

                case "rc":
                    majorRevision -= 97;
                    break;
            }

        result.ParsedVersion = parsedComponentsLength switch
        {
            1 => new GenericVersion(major),
            2 => new GenericVersion(major, minor),
            3 => new GenericVersion(major, minor, build),
            4 => new GenericVersion(major, minor, build, majorRevision),
            5 => new GenericVersion(major, minor, build, majorRevision, minorRevision),
            _ => result.ParsedVersion
        };

        return true;
    }

    private struct GenericVersionResult
    {
        public GenericVersion? ParsedVersion { get; set; }
    }
}