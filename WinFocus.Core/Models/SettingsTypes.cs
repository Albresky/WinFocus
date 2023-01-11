using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFocus.Core.Models;
public static class SettingsTypes
{
    public enum ImageSizeType
    {
        /// <summary>
        /// 3840*2160
        /// </summary>
        S_2160P,
        /// <summary>
        /// 1920*1200
        /// </summary>
        S_1200P,
        /// <summary>
        /// 1920*1080
        /// </summary>
        S_1080P,
        /// <summary>
        /// 1366*768
        /// </summary>
        S_720P
    }

    public enum PathType
    {
        FocusImagePath,
        BingWallpaperImagePath,
        LiveWallpaperVideoPath
    }

    public enum ContainerType
    {
        GlobalSettings,
        ImageInfo
    }
}
