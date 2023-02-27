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

    public static ImageSizeType ImageSizeTypeConverterStrToEnum(object type)
    {
        if (type is string strType)
        {
            return strType switch
            {
                "2160P" => ImageSizeType.S_2160P,
                "1200P" => ImageSizeType.S_1200P,
                "1080P" => ImageSizeType.S_1080P,
                "720P" => ImageSizeType.S_720P,
                _ => ImageSizeType.S_1080P,
            };
        }
        else
        {
            throw new ArgumentException("ExceptionStringToEnumConverterValueMustBeAnEnum");
        }
    }

    public static string ImageSizeTypeConverterEnumToStr(ImageSizeType type)
    {
        return type switch
        {
            ImageSizeType.S_2160P => "2160P",
            ImageSizeType.S_1200P => "1200P",
            ImageSizeType.S_1080P => "1080P",
            ImageSizeType.S_720P => "720P",
            _ => "1080P",
        };
    }
}
