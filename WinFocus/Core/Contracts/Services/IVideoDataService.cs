using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using WinFocus.Core.Models;

namespace WinFocus.Core.Contracts.Services;
public interface IVideoDataService
{
    Task<IEnumerable<VideoItem>> GetVideoDataAsync();
    Task<IEnumerable<uint>> GetResolutionAsync(StorageFile videoFile);
    Task<ImageStream> GetThumbnailFromVideoAsync(StorageFile videoFile,int width,int height);
    Task<VideoItem> CreateVideoItemAsync(string path);
}
