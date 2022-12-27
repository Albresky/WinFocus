using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WinFocus.Core.Services;
public class Wallpaper
{
    public enum PicturePosition
    {
        Tile,
        Center,
        Stretch,
        Fit,
        Fill
    }
    public static bool SetWallpaper(string imgFullPath,PicturePosition style)
    {
        try
        {
#pragma warning disable CA1416 // Validate platform compatibility
            using var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
#pragma warning restore CA1416 // Validate platform compatibility
            switch (style)
            {
                case PicturePosition.Tile:
#pragma warning disable CA1416 // Validate platform compatibility
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TitleWallpaper",1.ToString());
                        break;
                    case PicturePosition.Center:
                        key.SetValue(@"WallpaperStyle", 0.ToString());
                        key.SetValue(@"TitleWallpaper", 0.ToString());
                        break;
                    case PicturePosition.Stretch:
                        key.SetValue(@"WallpaperStyle", 2.ToString());
                        key.SetValue(@"TitleWallpaper", 0.ToString());
                        break;
                    case PicturePosition.Fit:
                        key.SetValue(@"WallpaperStyle", 6.ToString());
                        key.SetValue(@"TitleWallpaper", 0.ToString());
                        break;
                    case PicturePosition.Fill:
                        key.SetValue(@"WallpaperStyle", 10.ToString());
                        key.SetValue(@"TitleWallpaper", 0.ToString());
#pragma warning restore CA1416 // Validate platform compatibility
                    break;
                    default : { return false; }
            }
            return true;
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Specified registry key was not found.");
            return false;
        }
    }
}
