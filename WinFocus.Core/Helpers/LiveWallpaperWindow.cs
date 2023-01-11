using System;
using System.Diagnostics;
using Vanara.PInvoke;

namespace WinFocus.Core.Helpers;
public static class LiveWallpaperWindow
{
    public static void RemoveTitleBar(HWND hwnd)
    {
        var wStyle = (uint)User32.GetWindowLong(hwnd, User32.WindowLongFlags.GWL_STYLE);
        if (wStyle != 0)
        {
            wStyle &= ~((uint)User32.WindowStyles.WS_DLGFRAME
                        | (uint)User32.WindowStyles.WS_BORDER
                        | (uint)User32.WindowStyles.WS_MINIMIZEBOX
                        | (uint)User32.WindowStyles.WS_MAXIMIZEBOX);
            if(IntPtr.Zero!=User32.SetWindowLong(hwnd, User32.WindowLongFlags.GWL_STYLE, IntPtr.Parse(wStyle.ToString())))
            { 
                if(User32.SetWindowPos(hwnd,IntPtr.Zero,0,0,0,0,
                User32.SetWindowPosFlags.SWP_NOMOVE|
                User32.SetWindowPosFlags.SWP_NOSIZE|
                User32.SetWindowPosFlags.SWP_NOZORDER|
                User32.SetWindowPosFlags.SWP_FRAMECHANGED))
                {
                    Trace.WriteLine("Succeeded in removing titlebar.");
                }
            }
        }
    }
}
