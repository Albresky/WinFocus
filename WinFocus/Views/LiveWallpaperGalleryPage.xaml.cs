using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using WinFocus.Core.Helpers;
using WinFocus.Core.Models;
using WinFocus.Core.Services;
using WinFocus.ViewModels;
using WinUIEx;

namespace WinFocus.Views;

public sealed partial class LiveWallpaperGalleryPage : Page
{
    private LiveWallpaperPage? liveWallpaperPage = null;
    private IntPtr _windowHandle = IntPtr.Zero;
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
        UpdateLayout();
    }

    private void Init()
    {
        UpdateThumbnails();
        InitLiveWallpaperPage();
    }

    private void UpdateThumbnails()
    {
        var addImage = new Image();
        addImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Add.png"));
        addImage.Width = 130;
        addImage.Height = 130;
        addImage.Stretch = Microsoft.UI.Xaml.Media.Stretch.UniformToFill;
        VideoGridView.Items.Add(addImage);
    }

    private void InitLiveWallpaperPage()
    {
        liveWallpaperPage = new();
        _windowHandle = liveWallpaperPage.GetWindowHandle();
        var screenSize = DisplayArea.Primary.OuterBounds;
        liveWallpaperPage.SetWindowSize(screenSize.Width, screenSize.Height);
        liveWallpaperPage.Activate();
        liveWallpaperPage.CenterOnScreen();
        LiveWallpaperWindow.RemoveTitleBar(liveWallpaperPage.GetWindowHandle());
        liveWallpaperPage.Hide();
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

    private void Btn_SetAsBg_Click(object sender, RoutedEventArgs e)
    {

        Trace.WriteLine("Button Clicked.");
    }

    private async void Btn_Add_Click(object sender, RoutedEventArgs e)
    {
        var result = await SystemService.GetFilesInFilePickerAsync(new string[] { ".mp4", ".mkv" });
        if (result.Count > 0)
        {
            foreach (var item in result)
            {
                await ViewModel.AddVideoItemAsync(item.Path);
            }
        }
    }

    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var slider = sender as Slider;
        if (slider != null && liveWallpaperPage != null)
        {
            liveWallpaperPage.SetVolume(slider.Value);
        }
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Trace.WriteLine("OnNavigatingTo()");
        base.OnNavigatedTo(e);

        VideoGridView.SelectedIndex = gridview_index = 0;
        UpdateVideoInfo(gridview_index);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        Trace.WriteLine("OnNavigatingFrom()");
        base.OnNavigatingFrom(e);

    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        var ts = sender as ToggleSwitch;
        if (ts != null)
        {
            if (ts.IsOn)
            {
                var index = VideoGridView.SelectedIndex;
                if (liveWallpaperPage != null)
                {
                    liveWallpaperPage.Show();
                    liveWallpaperPage.VideoFile = ViewModel.Source.ElementAt(index).VideoPath;
                    liveWallpaperPage.Play();
                    LiveWallpaperService.SetLiveWallpaper(_windowHandle);
                }
            }
            else
            {
                liveWallpaperPage.Stop();
            }
        }
    }
}
