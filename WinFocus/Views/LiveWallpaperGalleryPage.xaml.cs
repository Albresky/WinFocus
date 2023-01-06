using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Windowing;
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
    LiveWallpaperPage? liveWallpaperPage = null;

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
        Trace.WriteLine("LiveWallpaperGalleryPage() called.");
        ViewModel = App.GetService<LiveWallpaperGalleryViewModel>();
        ViewModel.SetCurrentPage(this);
        this.InitializeComponent();
        InitLiveWallpaperPage();
    }

    private void InitLiveWallpaperPage()
    {
        liveWallpaperPage = new();
        var screenSize = DisplayArea.Primary.OuterBounds;
        liveWallpaperPage.SetWindowSize(screenSize.Width, screenSize.Height);
        liveWallpaperPage.Activate();
        liveWallpaperPage.Hide();
        liveWallpaperPage.CenterOnScreen();
        LiveWallpaperWindow.RemoveTitleBar(liveWallpaperPage.GetWindowHandle());
    }

    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        var index = VideoGridView.SelectedIndex;
        if (liveWallpaperPage != null)
        {
            liveWallpaperPage.VideoFile = ViewModel.Source.ElementAt(index).VideoPath;
            liveWallpaperPage.Play();
            liveWallpaperPage.Show();
            LiveWallpaperService.SetLiveWallpaper(liveWallpaperPage.GetWindowHandle());
        }

        Trace.WriteLine("Button Clicked.");
    }
}
