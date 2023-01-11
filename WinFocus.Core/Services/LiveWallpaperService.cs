using System;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;


namespace WinFocus.Core.Services;
public static class LiveWallpaperService
{
    private static readonly HWND progman = HWND.NULL;
    private static HWND m_workerw = HWND.NULL;
    private static readonly HWND m_livewallpaper_window = HWND.NULL;
    public static bool IsSet = false;

    static LiveWallpaperService()
    {
        progman = FindWindow("Progman", "Program Manager");
    }

    private static bool GetWorkerW()
    {
        if (FindWorkW())
        {
            return true;
        }
        var result = IntPtr.Zero;
        SendMessageTimeout(progman, 0x052C, new IntPtr(0), IntPtr.Zero, 0, 1000, ref result);
        if (!FindWorkW())
        {
            Console.WriteLine("Error in LiveWallpaperService.cs");
            return false;
        }
        return true;
    }

    private static bool FindWorkW()
    {
        EnumWindows(new EnumWindowsProc((tophandle, topparamhandle) =>
        {
            var p = FindWindowEx(tophandle, HWND.NULL, "SHELLDLL_DefView", string.Empty);

            if (IntPtr.Zero != p)
            {
                m_workerw = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                return false;
            }
            return true;
        }), IntPtr.Zero);
        if (m_workerw != HWND.NULL)
        {
            return true;
        }
        return false;
    }

    public static void SetLiveWallpaper(HWND child_hwnd)
    {
        if (!IsSet && GetWorkerW())
        {
            IsSet = true;
            /// <summary>
            // In WinFocus, the child_hwnd is the HWND of a 'new Window()',
            // which needs to be 'Activate()'.
            /// </summary>
            SetParent(child_hwnd, m_workerw);
        }
    }

    //public static void StopLiveWallpaper()
    //{
    //    if (m_livewallpaper_window != HWND.NULL)
    //    {

    //    }
    //}
}
