using System;

namespace GlowingBrain.Json.Infrastructure
{
    internal static class FunctionalMixins
    {
        public static void Times(this int value, Action action)
        {
            value.Times(_ => action ());
        }

        public static void Times(this int value, Action<int> action)
        {
            for (var i = 0; i < value; i++)
            {
                action(i);
            }
        }

        public static void WhenTrue(this bool value, Action action)
        {
            if (value)
            {
                action();
            }
        }

        public static void WhenFalse(this bool value, Action action)
        {
            if (!value)
            {
                action();
            }
        }
    }
}