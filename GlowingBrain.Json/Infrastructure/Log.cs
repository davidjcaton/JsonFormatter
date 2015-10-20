using System;

namespace GlowingBrain.Json.Infrastructure
{
    internal static class Log
    {
        static Log()
        {
#if DEBUG
            WriteLogMessage = message => System.Diagnostics.Debug.WriteLine(message);
#else
            WriteLogMessage = _ => {};
#endif
        }

        public static Action<string> WriteLogMessage { get; set; }

        public static void Debug(string format, params object[] args)
        {
            WriteToLog("DEBUG", string.Format(format, args));
        }

        private static void WriteToLog(string level, string message)
        {
            WriteLogMessage(string.Format("[{0}] - {1}", level, message));
        }
    }
}