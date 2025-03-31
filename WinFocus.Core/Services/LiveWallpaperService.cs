using System;
using System.Diagnostics;
using Vanara.PInvoke;
using Windows.System.Profile;
using static Vanara.PInvoke.User32;


namespace WinFocus.Core.Services;
public static class LiveWallpaperService
{
    private static readonly HWND progman = HWND.NULL;
    private static HWND m_workerw = HWND.NULL;
    private static readonly HWND m_livewallpaper_window = HWND.NULL;
    public static bool IsSet = false;

    /*
     * Windows 24H1 build number: 26100
     * Official Release Build Number:
     * https://learn.microsoft.com/en-us/windows/release-health/windows11-release-information#windows-11-release-history
     */
    private static readonly ulong Windows24H1BuildNumber = 26100;
    // Is current windows OS 24H1 or newer
    private static readonly bool IsWindows24H1OrNewer = GetIsWindows24H1OrNewer();


    // Official C# guidance for getting Windows OS version
    private static string GetOSVersionString()
    {
        try
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong major = (version & 0xFFFF000000000000L) >> 48;
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            ulong minor = (version & 0x0000FFFF00000000L) >> 32;
            ulong revision = (version & 0x000000000000FFFFL);
            return $"{major}.{minor}.{build}.{revision}";
        }
        catch (Exception ex)
        {
            Trace.TraceError($"Error in obtaining windows OS version: {ex.Message}");
            return "Error in GetOSVersionString";
        }
    }

    private static bool GetIsWindows24H1OrNewer()
    {
        try
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;

            Trace.TraceInformation($"Windows Build:{build}");

            return build >= Windows24H1BuildNumber;
        }
        catch (Exception ex)
        {
            Trace.TraceError($"Error in obtaining windows OS version: {ex.Message}");
            return false;
        }
    }


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
            Trace.TraceError("Error in LiveWallpaperService.cs");
            return false;
        }
        return true;
    }

    private static bool FindWorkW()
    {
        m_workerw = HWND.NULL;
        EnumWindows(new EnumWindowsProc((tophandle, topparamhandle) =>
        {
            var p = FindWindowEx(tophandle, HWND.NULL, "SHELLDLL_DefView", string.Empty);

            if (IntPtr.Zero != p)
            {
                if (IsWindows24H1OrNewer)
                {
                    m_workerw = FindWindowEx(progman, HWND.NULL, "WorkerW", null);
                }
                else
                {
                    m_workerw = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                }
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
            if (child_hwnd == HWND.NULL)
            {
                Trace.TraceError("Invalid child HWND from WinUI3.");
                return;
            }

            // 检查获取到的 WorkerW 窗口句柄是否有效
            if (m_workerw == HWND.NULL)
            {
                Trace.TraceError("Invalid child HWND of WorkerW");
                return;
            }
            IsSet = true;
            /// <summary>
            // In WinFocus, the child_hwnd is the HWND of a 'new Window()',
            // which needs to be 'Activate()'.
            /// </summary>
            SetForegroundWindow(child_hwnd);
            HWND result = SetParent(child_hwnd, m_workerw);

            if (result == HWND.NULL)
            {
                Trace.TraceError($"SetParent fail, errorcode: {Kernel32.GetLastError()}");
                IsSet = false;
            }
            else
            {
                Trace.TraceInformation("Set LiveWallpaper Success.");
                IsSet = true;
            }
        }
    }

    public static void StopLiveWallpaper()
    {
        IsSet = false;
    }
}
