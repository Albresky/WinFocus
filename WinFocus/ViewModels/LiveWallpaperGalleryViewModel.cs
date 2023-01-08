using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WinFocus.Contracts.Services;
using WinFocus.Contracts.ViewModels;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;
using WinFocus.Views;

namespace WinFocus.ViewModels;

public class LiveWallpaperGalleryViewModel : ObservableRecipient, INavigationAware
{
    private LiveWallpaperGalleryPage? page;
    private readonly INavigationService _navigationService;
    private readonly IVideoDataService _videoDataService;


    public void SetCurrentPage(LiveWallpaperGalleryPage page)
    {
        this.page = page;
    }

    public ObservableCollection<VideoItem> Source { get; } = new ObservableCollection<VideoItem>();

    public LiveWallpaperGalleryViewModel(INavigationService navigationService, IVideoDataService videoDataService)
    {
        _navigationService = navigationService;
        _videoDataService = videoDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await _videoDataService.GetVideoDataAsync();
        foreach (var item in data)
        {
            Source.Add(item);
        }
        //page?.SetGridViewSource(Source);
        //page?.setThumbnail(Source.ElementAt(0).Thumbnail);
    }

    public void OnNavigatedFrom()
    {
    }

    public async Task AddVideoItemAsync(string path)
    {
        var videoItem= await _videoDataService.CreateVideoItemAsync(path);
        Source.Add(videoItem);
    }
}
