using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using WinFocus.Core.Helpers;
using WinFocus.Core.Models;
using WinFocus.Core.Services;
using WinFocus.ViewModels;

namespace WinFocus.Views;

public sealed partial class LiveWallpaperGalleryPage : Page
{
    LiveWallpaperPage liveWallpaperPage;

    public void setThumbnail(BitmapImage bitmapImage)
    {
        thumbnail.Source = bitmapImage;
    }

    public void SetGridViewSource(ObservableCollection<VideoItem> source) => VideoGridView.ItemsSource = source;

    public LiveWallpaperGalleryViewModel ViewModel
    {
        get;
    }


    public LiveWallpaperGalleryPage()
    {
        ViewModel = App.GetService<LiveWallpaperGalleryViewModel>();
        ViewModel.SetCurrentPage(this);
        this.InitializeComponent();
        liveWallpaperPage = new();
    }

    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        //VideoGridView.GetBindingExpression(GridView.ItemsSourceProperty).UpdateSource();
        thumbnail.Source = ViewModel.Source.ElementAt(1).Thumbnail;
        var index = VideoGridView.SelectedIndex;
        liveWallpaperPage.SetWindowSize(3840, 2160);
        liveWallpaperPage.Activate();
        liveWallpaperPage.CenterOnScreen();
        liveWallpaperPage.VideoFile = ViewModel.Source.ElementAt(index).VideoPath;
        liveWallpaperPage.Play();
        LiveWallpaperService.SetLiveWallpaper(liveWallpaperPage.GetWindowHandle());
        LiveWallpaperWindow.RemoveTitleBar(liveWallpaperPage.GetWindowHandle());
        Trace.WriteLine("Button Clicked.");
    }
}
