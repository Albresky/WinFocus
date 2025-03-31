using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Storage;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;

namespace WinFocus.Core.Services;
public class VideoDataService : IVideoDataService
{
    private List<VideoItem>? _allVideoDetail;
    private readonly String AssetsVideoPath = Path.Combine(AppContext.BaseDirectory, "Assets\\Video");

    private string LOCAL_VIDEO_DIR()
    {
        return Core.CoreEngine.Current.AppSetting.GetAssetsPath(Models.SettingsTypes.PathType.LiveWallpaperVideoPath); 
    }
    private string LOCAL_THUMBNAIL_CACHE_DIR()
    {
        return $"{LOCAL_VIDEO_DIR()}\\thumbnails";
    }

    public VideoDataService()
    {
        Trace.WriteLine("LOCAL_VIDEO_DIR: " + LOCAL_VIDEO_DIR());
        if (!Directory.Exists(LOCAL_VIDEO_DIR()))
        {
            Trace.WriteLine("[VideoDataService] " + LOCAL_VIDEO_DIR() + " does not exist, creating it.");
            Directory.CreateDirectory(LOCAL_VIDEO_DIR());
        }
        if (!Directory.Exists(LOCAL_THUMBNAIL_CACHE_DIR()))
        {
            Trace.WriteLine("[VideoDataService] " + LOCAL_THUMBNAIL_CACHE_DIR() + " does not exist, creating it.");
            Directory.CreateDirectory(LOCAL_THUMBNAIL_CACHE_DIR());
        }
    }

    public async Task<VideoItem> CreateVideoItemAsync(string path)
    {
        var videoFile = await StorageFile.GetFileFromPathAsync(path);
        var videoFileProperties = await videoFile.GetBasicPropertiesAsync();
        var resolution = await GetResolutionAsync(videoFile);
        var videoItem = new VideoItem
        {
            VideoName = $"Video {videoFile.Name}",
            VideoPath = path,
            VideoWidth = (int)resolution.ElementAt(0),
            VideoHeight = (int)resolution.ElementAt(1),
            VideoSize = videoFileProperties.Size
        };
        var imageStream = await GetThumbnailFromVideoAsync(videoFile, videoItem.VideoWidth, videoItem.VideoHeight);

        /// <summary>
        /// Save thumbnail of the video to a file
        /// </summary>
        var imgName = await SaveThumbnailAsync(imageStream, videoItem.VideoWidth, videoItem.VideoHeight, videoFile.Name);
        videoItem.ThumbnailPath = $"{LOCAL_THUMBNAIL_CACHE_DIR()}\\{imgName}";
        return videoItem;
    }

    private async Task<IEnumerable<VideoItem>> AllVideo()
    {
        var videos = new List<VideoItem>();
        var videoFullPath = Directory.GetFiles(LOCAL_VIDEO_DIR());
        Trace.WriteLine($"videoFullPath: {videoFullPath}");
        if(videoFullPath.Length == 0)
        {
            Trace.TraceWarning("[W]No video found in the directory.");
            videoFullPath = Directory.GetFiles(AssetsVideoPath);
        }
        if(videoFullPath.Length == 0)
        {
            Trace.TraceError("No video found in the assets directory.");
            return videos;
        }
        foreach (var path in videoFullPath)
        {
            var item = await CreateVideoItemAsync(path);
            videos.Add(item);
        }
        return videos;
    }

    public static async Task<string> SaveThumbnailAsync(ImageStream imageStream, int width, int height, string fileName)
    {
        var imgName = $"thumbnail_{fileName.Split('.')[0]}.jpg";
        var thumbnailCacheDir = new VideoDataService().LOCAL_THUMBNAIL_CACHE_DIR();
        if (File.Exists($"{thumbnailCacheDir}\\{imgName}"))
        {
            return imgName;
        }
        var writableBitmap = new WriteableBitmap(width, height);

        var T_SaveFolder = CreateFileAsync(imgName, thumbnailCacheDir);
        var T_ReadStream = ReadStreamAsync(writableBitmap, imageStream, width, height);

        var SaveTarget = await T_SaveFolder;
        using var writeStream = await SaveTarget.OpenAsync(FileAccessMode.ReadWrite);
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, writeStream);

        var pixels = await T_ReadStream;
        encoder.SetPixelData(
            BitmapPixelFormat.Bgra8,
            BitmapAlphaMode.Premultiplied,
            (uint)writableBitmap.PixelWidth,
            (uint)writableBitmap.PixelHeight,
            96,
            96,
            pixels);
        await encoder.FlushAsync();
        using var outputStream = writeStream.GetOutputStreamAt(0);
        await outputStream.FlushAsync();

        return imgName;
    }

    private static async Task<StorageFile> CreateFileAsync(string FileName, string Directory)
    {
        var saveFolder = await StorageFolder.GetFolderFromPathAsync(Directory);
        return await saveFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
    }

    private static async Task<byte[]> ReadStreamAsync(WriteableBitmap writableBitmap, ImageStream imageStream, int Width, int Height)
    {
        writableBitmap.SetSource(imageStream);
        var stream = writableBitmap.PixelBuffer.AsStream();
        var pixels = new byte[(uint)stream.Length];
        await stream.ReadAsync(pixels);
        return pixels;
    }

    public async Task<IEnumerable<VideoItem>> GetVideoDataAsync()
    {
        _allVideoDetail ??= new List<VideoItem>(await AllVideo());
        return _allVideoDetail;
    }

    public async Task<IEnumerable<uint>> GetResolutionAsync(StorageFile videoFile)
    {
        var encodingPropertiesToRetrieve = new List<string>
        {
            "System.Video.FrameWidth",
            "System.Video.FrameHeight"
        };
        var encodingProperties = await videoFile.Properties.RetrievePropertiesAsync(encodingPropertiesToRetrieve);
        return new List<uint>
        {
            (uint)encodingProperties["System.Video.FrameWidth"],
            (uint)encodingProperties["System.Video.FrameHeight"]
        };
    }

    public async Task<ImageStream> GetThumbnailFromVideoAsync(StorageFile videoFile, int width, int height)
    {
        var timeOfFrame = new TimeSpan(0, 0, 1);
        var clip = await MediaClip.CreateFromFileAsync(videoFile);
        var composition = new MediaComposition();
        composition.Clips.Add(clip);
        return await composition.GetThumbnailAsync(timeOfFrame, width, height, VideoFramePrecision.NearestFrame);
    }
}
