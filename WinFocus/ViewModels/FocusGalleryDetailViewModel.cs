using CommunityToolkit.Mvvm.ComponentModel;

using WinFocus.Contracts.ViewModels;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;

namespace WinFocus.ViewModels;

public class FocusGalleryDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly IImageDataService _imageDataService;
    private ImageItem? _item;

    public ImageItem? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    public FocusGalleryDetailViewModel(IImageDataService imageDataService)
    {
        _imageDataService = imageDataService;
    }
    
    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string ImagePath)
        {
            var data = await _imageDataService.GetImageGridDataAsync();
            Item = data.First(i => i.ImagePath == ImagePath);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
