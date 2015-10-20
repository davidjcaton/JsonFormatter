using System;
using System.Collections.Generic;

namespace GlowingBrain.Json.Infrastructure
{
    internal static class NullableExtensions
    {
        public static bool HasValueOf<TValue>(this TValue? nullable, TValue value) where TValue : struct
        {
            return nullable.HasValue && EqualityComparer<TValue>.Default.Equals(nullable.Value, value);
        }

        public static void WhenHasValue<TValue>(this TValue? nullable, Action<TValue> action) where TValue : struct
        {
            if (nullable.HasValue)
            {
                action(nullable.Value);
            }
        }

        public static void WhenValueEquals<TValue>(this TValue? nullable, TValue value, Action action) where TValue : struct
        {
            if (nullable.HasValueOf(value))
            {
                action();
            }
        }
    }
}