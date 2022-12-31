using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace WinFocus.Core.Services;
public static class LiveWallpaperService
{
    private static readonly HWND progman = HWND.NULL;
    private static HWND m_workerw = HWND.NULL;

    static LiveWallpaperService()
    {
        progman = FindWindow("Progman", "Program Manager");
    }

    private static HWND GetWorkerW()
    {
        if (FindWorkw())
        {
            return m_workerw;
        }
        IntPtr result = IntPtr.Zero;
        SendMessageTimeout(progman, 0x052C, new IntPtr(0), IntPtr.Zero, 0, 1000, ref result);
        if (!FindWorkw())
        {
            Console.WriteLine("Error in LiveWallpaperService.cs");
            return HWND.NULL;
        }
        return m_workerw;
    }
    
    private static bool FindWorkw()
    {
        EnumWindows(new EnumWindowsProc((tophandle, topparamhandle) =>
        {
            HWND p = FindWindowEx(tophandle, HWND.NULL, "SHELLDLL_DefView", string.Empty);

            if (IntPtr.Zero != p)
            {
                m_workerw = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
            }
            return true;
        }), IntPtr.Zero);
        return false;
    }
}
