using System.Diagnostics;
using System;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Extensions
{
    public static class ExceptionExtension
    {
        public static void LogError(this Exception e)
        {
            Trace.TraceError("{0:HH:mm:ss.fff} Exception {1}", DateTime.Now, e);
        }
        public static void LogInformation(this Exception e)
        {
            Trace.TraceInformation("{0:HH:mm:ss.fff} Exception {1}", DateTime.Now, e);
        }
        public static void LogWarning(this Exception e)
        {
            Trace.TraceWarning("{0:HH:mm:ss.fff} Exception {1}", DateTime.Now, e);
        }
    }
}
