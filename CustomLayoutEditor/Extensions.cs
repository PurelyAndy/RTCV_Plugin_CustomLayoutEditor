using System;
using System.Linq;

namespace CustomLayoutEditor;

public static class Extensions
{
    /// <summary>
    /// Checks if (sourceValue & compareValue) == compareValue. Intended for use with Flags enums.
    /// </summary>
    /// <param name="source">The source enum value to check against.</param>
    /// <param name="compare">The enum value to check source contains.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>Whether the source contains the compare value.</returns>
    public static bool Contains<T>(this T source, T compare) where T : Enum
    {
        if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
            throw new InvalidOperationException($"Enum {typeof(T).Name} is not marked with FlagsAttribute.");
        
        var sourceValue = Convert.ToInt32(source);
        var compareValue = Convert.ToInt32(compare);
        return (sourceValue & compareValue) == compareValue;
    }
}