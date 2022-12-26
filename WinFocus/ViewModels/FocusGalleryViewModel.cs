using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinFocus.Contracts.Services;
using WinFocus.Contracts.ViewModels;
using WinFocus.Controls;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;
using WinFocus.Views;

namespace WinFocus.ViewModels;

public class FocusGalleryViewModel : ObservableRecipient, INavigationAware
{
    private FocusGalleryPage page;
    private readonly INavigationService _navigationService;
    private readonly IImageDataService _imageDataService;


    public void SetCurrentPage(FocusGalleryPage page)
    {
        this.page = page;
    }

    public ICommand ItemClickCommand
    {
        get;
    }
    
    public ObservableCollection<ImageItem> Source { get; } = new ObservableCollection<ImageItem>();

    public FocusGalleryViewModel(INavigationService navigationService, IImageDataService imageDataService)
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
            DisplayToDoDialog();
            return;
            // ToDo
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(FocusGalleryDetailViewModel).FullName!, clickedItem.ImageName);
        }
    }

    private async void DisplayToDoDialog()
    {
        ContentDialog dialog = new ContentDialog();
        dialog.XamlRoot = page.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "ToDo";
        dialog.Content = "The detail page of these images is not avaliable right now.";
        dialog.CloseButtonText = "I'll be waiting!";
        await dialog.ShowAsync();
    }
}
