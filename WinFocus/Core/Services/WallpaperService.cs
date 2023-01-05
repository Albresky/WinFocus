using System.Runtime.InteropServices;
using Microsoft.Win32;

public enum WallpaperStyle
{
    Fill,
    Fit,
    Stretch,
    Tile,
    Center,
    Span,
}

#pragma warning disable CA1416 // Validate platform compatibility

public static class WallpaperService
{
    private const string DESKTOP_REG_PATH = @"Control Panel\Desktop";
    private const string HISTORY_REG_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers";
    private const string WALLPAPER_STYLE_REG_PATH = "WallpaperStyle";
    private const string TILE_WALLPAPER_REG_PATH = "TileWallpaper";

    private const int HISTORY_MAX_ENTRIES = 5;

    const int SPI_SETDESKWALLPAPER = 20;
    const int SPIF_UPDATEINIFILE = 0x01;
    const int SPIF_SENDWININICHANGE = 0x02;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    private static State? _backupState;
    private static bool _historyRestored;

    private struct Config
    {
        public int Style;
        public bool IsTile;
    }

    private struct State
    {
        public Config Config;
        public string[] History;
        public string Wallpaper;
    }

    private static RegistryKey Openkey(string REG_PATH)
    {
        return Registry.CurrentUser.OpenSubKey(REG_PATH, true);
    }

    private static int GetRegistryValue(RegistryKey key, string name, int defaultValue)
    {
        return int.Parse((string)key.GetValue(name) ?? defaultValue.ToString());
    }

    private static bool GetRegistryValue(RegistryKey key, string name, bool defaultValue)
    {
        return ((string)key.GetValue(name) ?? (defaultValue ? "1" : "0")) == "1";
    }

    private static void SetRegistryValue(RegistryKey key, string name, int value)
    {
        key.SetValue(name, value.ToString());
    }

    private static void SetRegistryValue(RegistryKey key, string name, bool value)
    {
        key.SetValue(name, value ? "1" : "0");
    }

    private static Config GetWallpaperConfig()
    {
        RegistryKey key = Openkey(DESKTOP_REG_PATH);

        return new Config
        {
            Style = GetRegistryValue(key, WALLPAPER_STYLE_REG_PATH, 0),
            IsTile = GetRegistryValue(key, TILE_WALLPAPER_REG_PATH, false),
        };
    }

    private static void SetWallpaperConfig(Config value)
    {
        RegistryKey key = Openkey(DESKTOP_REG_PATH);
        SetRegistryValue(key, WALLPAPER_STYLE_REG_PATH, value.Style);
        SetRegistryValue(key, TILE_WALLPAPER_REG_PATH, value.IsTile);
    }

    private static void SetStyle(WallpaperStyle style)
    {
        switch (style)
        {
            case WallpaperStyle.Fill:
                SetWallpaperConfig(new Config { Style = 10, IsTile = false });
                break;
            case WallpaperStyle.Fit:
                SetWallpaperConfig(new Config { Style = 6, IsTile = false });
                break;
            case WallpaperStyle.Stretch:
                SetWallpaperConfig(new Config { Style = 2, IsTile = false });
                break;
            case WallpaperStyle.Tile:
                SetWallpaperConfig(new Config { Style = 0, IsTile = true });
                break;
            case WallpaperStyle.Center:
                SetWallpaperConfig(new Config { Style = 0, IsTile = false });
                break;
            case WallpaperStyle.Span: // Windows 8 or newer only
                SetWallpaperConfig(new Config { Style = 22, IsTile = false });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(style));
        }
    }

    private static void ChangeWallpaper(string filename)
    {
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
    }

    private static void RestoreHistory()
    {
        if (_historyRestored) return;

        if (!_backupState.HasValue)
            throw new Exception("You must call BackupState() before.");

        var backupState = _backupState.Value;

        using (var key = Openkey(HISTORY_REG_PATH))
        {
            for (var i = 0; i < HISTORY_MAX_ENTRIES; i++)
                if (backupState.History[i] != null)
                    key.SetValue($"BackgroundHistoryPath{i}", backupState.History[i], RegistryValueKind.String);
        }

        _historyRestored = true;
    }

    /// <summary>
    /// Backups the current wallpaper state (style and history).
    /// </summary>
    public static void BackupState()
    {
        var history = new string[HISTORY_MAX_ENTRIES];

        using (var key = Openkey(HISTORY_REG_PATH))
        {
            for (var i = 0; i < history.Length; i++)

                history[i] = (string)key.GetValue($"BackgroundHistoryPath{i}");
        }


        _backupState = new State
        {
            Config = GetWallpaperConfig(),
            History = history,
            Wallpaper = history[0],
        };

        _historyRestored = false;
    }

    /// <summary>
    /// Restores the state (style, wallpaper and history) before any Set() method.
    /// </summary>
    public static void RestoreState()
    {
        if (!_backupState.HasValue)
            throw new Exception("You must call BackupState() before.");

        SetWallpaperConfig(_backupState.Value.Config);
        ChangeWallpaper(_backupState.Value.Wallpaper);
        RestoreHistory();

        _backupState = null;
    }

    /// <summary>
    /// Sets the wallpaper without changing its style.
    /// </summary>
    public static void Set(string filename)
    {
        BackupState();
        ChangeWallpaper(filename);
    }

    /// <summary>
    /// Sets the wallpaper with the given style.
    /// </summary>
    public static void Set(string filename, WallpaperStyle style)
    {
        BackupState();
        SetStyle(style);
        ChangeWallpaper(filename);
    }

    /// <summary>
    /// Sets the wallpaper without changing its style nor the history in Windows settings.
    /// </summary>
    public static void SilentSet(string filename)
    {
        Set(filename);
        RestoreHistory();
    }

    /// <summary>
    /// Sets the wallpaper with the given style without changing the history in Windows settings.
    /// </summary>
    public static void SilentSet(string filename, WallpaperStyle style)
    {
        Set(filename, style);
        RestoreHistory();
    }
}

#pragma warning restore CA1416 // Validate platform compatibility
