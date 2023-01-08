using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
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
    private static readonly string LOCAL_VIDEO_DIR = "D:\\VideoCache";
    private static readonly string LOCAL_THUMBNAIL_CACHE_DIR = $"{LOCAL_VIDEO_DIR}\\thumbnails";

    public VideoDataService()
    {
        if (!FileService.OpenFolder(LOCAL_THUMBNAIL_CACHE_DIR) || !FileService.OpenFolder(LOCAL_VIDEO_DIR))
        {
            Trace.WriteLine("Failed to open video directory");
        }
    }

    private async Task<IEnumerable<VideoItem>> AllVideo()
    {
        var videos = new List<VideoItem>();
        var videoFullPath = Directory.GetFiles(LOCAL_VIDEO_DIR);
        var i = 0;
        foreach (var path in videoFullPath)
        {
            StorageFile videoFile;
            videoFile = await StorageFile.GetFileFromPathAsync(path);
            var videoFileProperties= await videoFile.GetBasicPropertiesAsync();
            var resolution = await GetResolutionAsync(videoFile);
            var videoItem = new VideoItem
            {
                VideoName = $"Video {i++}",
                VideoPath = path,
                VideoWidth = (int)resolution.ElementAt(0),
                VideoHeight = (int)resolution.ElementAt(1),
                VideoSize = videoFileProperties.Size
            };
            var imageStream = await GetThumbnailFromVideoAsync(videoFile, videoItem.VideoWidth, videoItem.VideoHeight);

            /// <summary>
            /// Get bitmapImage from imageStream
            /// </summary>
            //var bitmapImage = new BitmapImage();
            //bitmapImage.SetSource(imageStream);
            //videoItem.Thumbnail = bitmapImage;

            /// <summary>
            /// Save thumbnail of the video to a file
            /// </summary>
            var imgName = await SaveThumbnailAsync(imageStream, videoItem.VideoWidth, videoItem.VideoHeight, videoFile.Name);
            videoItem.ThumbnailPath = $"{LOCAL_THUMBNAIL_CACHE_DIR}\\{imgName}";

            videos.Add(videoItem);
        }
        return videos;
    }

    public static async Task<string> SaveThumbnailAsync(ImageStream imageStream, int width, int height, string fileName)
    {
        var imgName = $"thumbnail_{fileName.Split('.')[0]}.jpg";
        if (File.Exists($"{LOCAL_THUMBNAIL_CACHE_DIR}\\{imgName}"))
        {
            return imgName;
        }
        var writableBitmap = new WriteableBitmap(width, height);

        var T_SaveFolder = CreateFileAsync(imgName, LOCAL_THUMBNAIL_CACHE_DIR);
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
