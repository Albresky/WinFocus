using System.ComponentModel.DataAnnotations;
using SixLabors.ImageSharp;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Helpers;
using WinFocus.Core.Models;

namespace WinFocus.Core.Services;
public class VideoDataService : IImageDataService
{
    private List<VideoItem> _allVideoDetail;
    private readonly string LOCAL_IMAGE_DIR = "D:\\VideoCache";

    public VideoDataService()
    {
    }
    
    private IEnumerable<VideoItem> AllVideo()
    {
        var videos = new List<VideoItem>();
        var videoFullPath = Directory.GetFiles(LOCAL_IMAGE_DIR);
        foreach (var path in videoFullPath)
        {
           var resolution=VideoHelp
            var videoItem = new VideoItem
            {
                VideoName = $"Video {i}",
                VideoHeight
            };

            //var bitmap = Image.Load(img);
            //var info = new FileInfo(img);
            //var image = new ImageItem
            //{
            //    ImageName = $"Image {i}",
            //    ImageResolution = $"{bitmap.Width} x {bitmap.Height}",
            //    ImagePath = img,
            //    ImageSrcName = info.Name,
            //    ImageSize = info.Length,
            //    ImageDescription = $"This is a description for {i}",
            //    ImageAuthor = "Albrecht",
            //    ImageDate = info.LastWriteTime.ToString()
            //};
            //images.Add(image);
        }
        return videos;
    }

    public async Task<IEnumerable<VideoItem>> GetImageGridDataAsync()
    {
        _allVideoDetail ??= new List<VideoItem>(AllVideo());

        await Task.CompletedTask;
        return _allVideoDetail;
    }
}
