using System.IO;
using SixLabors.ImageSharp;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Helpers;
using WinFocus.Core.Models;

namespace WinFocus.Core.Services;
public class ImageDataService : IImageDataService
{
    private List<ImageItem> _allImagesDetail;
    private readonly string LOCAL_IMAGE_DIR = "D:\\ImageCache";

    public ImageDataService()
    {
    }

    private IEnumerable<ImageItem> AllImages()
    {
        var images = new List<ImageItem>();
        var imagesPath = FocusImageHelper.GetImgPath();
        var i = 1;
        foreach (var path in imagesPath)
        {
            var bitmap = Image.Load(path);
            FileInfo info = new FileInfo(path);
            try {
                if (FileService.OpenFolder(LOCAL_IMAGE_DIR))
                {
                    var imgCache = Directory.GetFiles(LOCAL_IMAGE_DIR);
                    if (!imgCache.Contains(info.Name)){
                        FileService.FileCopy(path, LOCAL_IMAGE_DIR, info.Name);
                    }
                }

            }
            catch (Exception)
            {
                return null;
            }

        }
        var imageCache = Directory.GetFiles(LOCAL_IMAGE_DIR);
        foreach(var img in imageCache){
            var bitmap = Image.Load(img);
            FileInfo info = new FileInfo(img);
            var image = new ImageItem
            {
                ImageName = "Image " + i,
                ImageResolution = bitmap.Width + "x" + bitmap.Height,
                ImagePath = img,
                ImageSrcName = info.Name,
                ImageSize = info.Length,
                ImageDescription = "This is a description for " + i,
                ImageAuthor = "Albrecht",
                ImageDate = info.LastWriteTime.ToString()
            };
            images.Add(image);
        }
        return images;
    }

    public async Task<IEnumerable<ImageItem>> GetImageGridDataAsync()
    {
        if (_allImagesDetail == null)
        {
            _allImagesDetail = new List<ImageItem>(AllImages());
        }

        await Task.CompletedTask;
        return _allImagesDetail;
    }
}
