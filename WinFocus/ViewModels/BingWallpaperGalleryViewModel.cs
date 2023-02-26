using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using WinFocus.Contracts.Services;
using WinFocus.Contracts.ViewModels;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;
using WinFocus.Views;

namespace WinFocus.ViewModels;

public class BingWallpaperGalleryViewModel : ObservableRecipient, INavigationAware
{
    private BingWallpaperPage? page;
    private readonly INavigationService _navigationService;
    private readonly IImageDataService _imageDataService;


    public void SetCurrentPage(BingWallpaperPage page)
    {
        this.page = page;
    }



    public ICommand ItemClickCommand
    {
        get;
    }


    public ObservableCollection<ImageItem> Source { get; } = new ObservableCollection<ImageItem>();

    public BingWallpaperGalleryViewModel(INavigationService navigationService, IImageDataService imageDataService)
    {
        _navigationService = navigationService;
        _imageDataService = imageDataService;

        ItemClickCommand = new RelayCommand<ImageItem>(OnItemClick);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _imageDataService.GetImageGridDataAsync();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnItemClick(ImageItem? clickedItem)
    {
        if (clickedItem != null)
        {
            /* ToDo */
            //DisplayToDoDialog();
            //return;
            // ToDo
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            //_navigationService.NavigateTo(typeof(FocusGalleryDetailViewModel).FullName!, clickedItem.ImagePath);
        }
    }

}
