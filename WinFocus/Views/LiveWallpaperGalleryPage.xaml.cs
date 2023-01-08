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
    private int gridview_index = 0;
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
        Init();
        this.UpdateLayout();
    }

    private void Init()
    {
        UpdateThumbnails();
        InitLiveWallpaperPage();
    }

    private void UpdateThumbnails()
    {
        Trace.WriteLine("UpdateThumbnails()...");
        if (ViewModel != null && ViewModel.Source.Count > 0)
        {
            //foreach(Image image in VideoGridView.Items)
            //{
            //    image.Source = new BitmapImage(new Uri(ViewModel.Source.ElementAt(gridview_index).ThumbnailPath));
            //}
        }
    }

    private void InitLiveWallpaperPage()
    {
        liveWallpaperPage = new();
        liveWallpaperPage.Hide();
        var screenSize = DisplayArea.Primary.OuterBounds;
        liveWallpaperPage.SetWindowSize(screenSize.Width, screenSize.Height);
        liveWallpaperPage.Activate();
        liveWallpaperPage.CenterOnScreen();
        LiveWallpaperWindow.RemoveTitleBar(liveWallpaperPage.GetWindowHandle());
    }

    private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var gv = sender as GridView;

        if (gv != null)
        {
            gridview_index = gv.SelectedIndex;
            UpdateVideoInfo(gridview_index);
        }
    }

    private void UpdateVideoInfo(int index)
    {
        if (ViewModel != null && ViewModel.Source.Count > 0)
        {
            var videoItem = ViewModel.Source.ElementAt(index);
            txtSize.Text = (videoItem.VideoSize / 1000).ToString();
            txtResolution.Text = $"{videoItem.VideoWidth}x{videoItem.VideoHeight}";
        }

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

    private void Btn_Update_Click(object sender, RoutedEventArgs e)
    {
        UpdateThumbnails();
    }
}
