using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using WinFocus.Contracts.Services;
using WinFocus.Contracts.ViewModels;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;
using WinFocus.Helpers;
using WinFocus.Views;

namespace WinFocus.ViewModels;

public class LiveWallpaperGalleryViewModel : ObservableRecipient, INavigationAware
{
    private Page? page;
    private readonly INavigationService _navigationService;
    private readonly IVideoDataService _videoDataService;


    public void SetCurrentPage(Page page)
    {
        this.page = page;
    }

    public ObservableCollection<VideoItem> Source { get; } = new ObservableCollection<VideoItem>();

    public LiveWallpaperGalleryViewModel(INavigationService navigationService, IVideoDataService videoDataService)
    {
        TraceHelper.TraceClass(this);
        _navigationService = navigationService;
        _videoDataService = videoDataService;
        OnNavigatedTo(null);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Trace.WriteLine($"OnNavigatedTo:{GetType().Name}");
        Source.Clear();
        var data = await _videoDataService.GetVideoDataAsync();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
        Trace.WriteLine($"OnNavigatedFrom:{GetType().Name}");
    }

    public async Task AddVideoItemAsync(IReadOnlyList<StorageFile> videoItemsPath)
    {
        var l=new List<VideoItem>(videoItemsPath.Count);
        foreach(var path in videoItemsPath)
        {
            var item = await _videoDataService.CreateVideoItemAsync(path.Path);
            l.Add(item);
        }
        foreach(var item in l)
        {
            Source.Insert(Source.Count - 1, item);
        }
    }
}
