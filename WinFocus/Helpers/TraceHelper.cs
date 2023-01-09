using System.Diagnostics;

namespace WinFocus.Helpers;
public static class TraceHelper
{
    public static void TraceClass(object obj)
    {
        if (obj != null)
        {
            Trace.WriteLine($"{obj.GetType().Name} called.");
        }
    }
}
