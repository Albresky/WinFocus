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
    private readonly string LOCAL_IMAGE_DIR = "D:\\VideoCache";
    private static readonly string LOCAL_THUMBNAIL_CACHE_DIR = "D:\\VideoCache\\thumbnails";

    public VideoDataService()
    {
        if (!FileService.OpenFolder(LOCAL_THUMBNAIL_CACHE_DIR))
        {
            Trace.WriteLine("Failed to open thumbnail cache directory");
        }
    }

    private async Task<IEnumerable<VideoItem>> AllVideo()
    {
        var videos = new List<VideoItem>();
        var videoFullPath = Directory.GetFiles(LOCAL_IMAGE_DIR);
        var i = 0;
        foreach (var path in videoFullPath)
        {
            StorageFile videoFile;
            videoFile = await StorageFile.GetFileFromPathAsync(path);
            var resolution = await GetResolutionAsync(videoFile);
            var videoItem = new VideoItem
            {
                VideoName = $"Video {i++}",
                VideoPath = path,
                VideoWidth = (int)resolution.ElementAt(0),
                VideoHeight = (int)resolution.ElementAt(1),
                VideoSize = 12000
            };
            var imageStream = await GetThumbnailFromVideo(videoFile, videoItem.VideoWidth, videoItem.VideoHeight);
            
            //var bitmapImage = new BitmapImage();
            //bitmapImage.SetSource(imageStream);
            //videoItem.Thumbnail = bitmapImage;
            
            /// <summary>
            /// Save thumbnail of the video to a file
            /// </summary>
            var imgName = await SaveThumbnail(imageStream, videoItem.VideoWidth, videoItem.VideoHeight);
            videoItem.ThumbnailPath = $"{LOCAL_THUMBNAIL_CACHE_DIR}\\{imgName}";




            videos.Add(videoItem);
        }
        return videos;
    }

    public static async Task<string> SaveThumbnail(ImageStream imageStream, int width, int height)
    {
        // generate bitmap 
        var writableBitmap = new WriteableBitmap(width, width);
        writableBitmap.SetSource(imageStream);

        // generate some random name for file in PicturesLibrary
        var imgName = $"IMG_{Guid.NewGuid().ToString().Substring(0, 4)}.jpg";
        var saveFolder = await StorageFolder.GetFolderFromPathAsync(LOCAL_THUMBNAIL_CACHE_DIR);
        var saveAsTarget = await saveFolder.CreateFileAsync(imgName, CreationCollisionOption.ReplaceExisting);
        //var saveAsTarget = await KnownFolders.PicturesLibrary.CreateFileAsync(imgName);

        // get stream from bitmap
        var stream = writableBitmap.PixelBuffer.AsStream();
        var pixels = new byte[(uint)stream.Length];
        await stream.ReadAsync(pixels, 0, pixels.Length);

        using var writeStream = await saveAsTarget.OpenAsync(FileAccessMode.ReadWrite);
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, writeStream);
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

    public async Task<IEnumerable<VideoItem>> GetVideoDataAsync()
    {
        _allVideoDetail ??= new List<VideoItem>(await AllVideo());

        await Task.CompletedTask;
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

    public async Task<ImageStream> GetThumbnailFromVideo(StorageFile videoFile, int width, int height)
    {
        var timeOfFrame = new TimeSpan(0, 0, 1);
        var clip = await MediaClip.CreateFromFileAsync(videoFile);
        var composition = new MediaComposition();
        composition.Clips.Add(clip);
        return await composition.GetThumbnailAsync(timeOfFrame, width, height, VideoFramePrecision.NearestFrame);
    }
}
