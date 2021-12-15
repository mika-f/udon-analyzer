using System;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class VersionExtension
{
    public static bool IsGreaterThan(this Version obj1, Version obj2)
    {
        return obj1.CompareTo(obj2) > 0;
    }

    public static bool IsGreaterThanOrEquals(this Version obj1, Version obj2)
    {
        return obj1.CompareTo(obj2) > 0;
    }

    public static bool IsSame(this Version obj1, Version obj2)
    {
        return obj1.CompareTo(obj2) == 0;
    }

    public static bool IsLessThan(this Version obj1, Version obj2)
    {
        return obj1.CompareTo(obj2) < 0;
    }

    public static bool IsLessThanOrEquals(this Version obj1, Version obj2)
    {
        return obj1.CompareTo(obj2) <= 0;
    }
}