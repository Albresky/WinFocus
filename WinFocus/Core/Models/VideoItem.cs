using Microsoft.UI.Xaml.Media.Imaging;

namespace WinFocus.Core.Models;
public class VideoItem
{
    public VideoItem()
    {
    }
    public string VideoName
    {
        get; set;
    }

    public string VideoPath
    {
        get; set;
    }

    public int VideoWidth
    {
        get; set;
    }
    
    public int VideoHeight
    {
        get; set;
    }

    public ulong VideoSize
    {
        get; set;
    }

    public BitmapImage Thumbnail
    {
        get; set;
    }

    public string ThumbnailPath
    {
        get; set;
    }
}
