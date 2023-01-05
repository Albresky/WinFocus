using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Storage;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;

namespace WinFocus.Core.Services;
public class VideoDataService : IVideoDataService
{
    private List<VideoItem> _allVideoDetail;
    private readonly string LOCAL_IMAGE_DIR = "D:\\VideoCache";

    public VideoDataService()
    {
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

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(imageStream);
            videoItem.Thumbnail = bitmapImage;

            ////generate bitmap 
            //var writableBitmap = new WriteableBitmap(videoItem.VideoWidth, videoItem.VideoHeight);
            //writableBitmap.SetSource(imageStream);


            ////generate some random name for file in PicturesLibrary
            //var saveAsTarget = await KnownFolders.PicturesLibrary.CreateFileAsync("IMG" + Guid.NewGuid().ToString().Substring(0, 4) + ".jpg");
            ////get stream from bitmap

            //var stream = writableBitmap.PixelBuffer.AsStream();
            //byte[] pixels = new byte[(uint)stream.Length];
            //await stream.ReadAsync(pixels, 0, pixels.Length);

            //using (var writeStream = await saveAsTarget.OpenAsync(FileAccessMode.ReadWrite))
            //{
            //    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, writeStream);
            //    encoder.SetPixelData(
            //        BitmapPixelFormat.Bgra8,
            //        BitmapAlphaMode.Premultiplied,
            //        (uint)writableBitmap.PixelWidth,
            //        (uint)writableBitmap.PixelHeight,
            //        96,
            //        96,
            //        pixels);
            //    await encoder.FlushAsync();

            //    using (var outputStream = writeStream.GetOutputStreamAt(0))
            //    {
            //        await outputStream.FlushAsync();
            //    }
            //}

            videos.Add(videoItem);
        }
        return videos;
    }

    public async Task<IList<VideoItem>> GetVideoDataAsync()
    {
        _allVideoDetail ??= new List<VideoItem>(await AllVideo());

        await Task.CompletedTask;
        return _allVideoDetail;
    }

    public async Task<IEnumerable<uint>> GetResolutionAsync(StorageFile videoFile)
    {
        var encodingPropertiesToRetrieve = new List<string>();
        encodingPropertiesToRetrieve.Add("System.Video.FrameHeight");
        encodingPropertiesToRetrieve.Add("System.Video.FrameWidth");
        var encodingProperties = await videoFile.Properties.RetrievePropertiesAsync(encodingPropertiesToRetrieve);
        var frameHeight = (uint)encodingProperties["System.Video.FrameHeight"];
        var frameWidth = (uint)encodingProperties["System.Video.FrameWidth"];
        var resolution = new List<uint>();
        resolution.Add(frameWidth);
        resolution.Add(frameHeight);
        return resolution;
    }

    public async Task<ImageStream> GetThumbnailFromVideo(StorageFile videoFile, int width, int height)
    {
        TimeSpan timeOfFrame = new TimeSpan(0, 0, 1);
        var clip = await MediaClip.CreateFromFileAsync(videoFile);
        var composition = new MediaComposition();
        composition.Clips.Add(clip);
        var thumbnail = await composition.GetThumbnailAsync(timeOfFrame, width, height, VideoFramePrecision.NearestFrame);
        await Task.CompletedTask;
        return thumbnail;
    }
}
