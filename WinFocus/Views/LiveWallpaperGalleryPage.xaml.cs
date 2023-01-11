using System.Diagnostics;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;
using WinFocus.Core.Helpers;
using WinFocus.Core.Models;
using WinFocus.Core.Services;
using WinFocus.Helpers;

namespace WinFocus.Views;

public sealed partial class LiveWallpaperGalleryPage : Page
{
    private LiveWallpaperPage? liveWallpaperPage = null;
    private IntPtr _windowHandle = IntPtr.Zero;

    private VideoItem videoItem;

    public bool IsNotTSOn => !ts_apply.IsOn;

    public LiveWallpaperGalleryPage()
    {
        TraceHelper.TraceClass(this);
        InitializeComponent();
        Init();
        //NavigationCacheMode = NavigationCacheMode.Enabled;
    }

    private void Init()
    {
        //UpdateThumbnails();
        InitLiveWallpaperPage();
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

    public void SelectedVideoItemChanged(VideoItem v)
    {
        videoItem = v;
        UpdateInfo();
    }

    private void Btn_SetAsBg_Click(object sender, RoutedEventArgs e)
    {

        Trace.WriteLine("Button Clicked.");
    }


    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var slider = sender as Slider;
        if (slider != null && liveWallpaperPage != null)
        {
            liveWallpaperPage.SetVolume(slider.Value / 100);
        }
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Trace.WriteLine($"OnNavigatedTo:{GetType().Name}");
        base.OnNavigatedTo(e);
        FrameVideoGridView.Navigate(typeof(VideoGridViewPage), this);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        Trace.WriteLine($"OnNavigatedFrom:{GetType().Name}");
        base.OnNavigatingFrom(e);
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        var ts = sender as ToggleSwitch;
        if (ts != null && liveWallpaperPage != null)
        {
            if (ts.IsOn)
            {
                liveWallpaperPage.Show();
                liveWallpaperPage.videoFile = videoItem.VideoPath;
                liveWallpaperPage.Play();
                LiveWallpaperService.SetLiveWallpaper(_windowHandle);
            }
            else
            {
                liveWallpaperPage.Stop();
            }
        }
    }

    private void UpdateInfo()
    {
        txtResolution.Text = $"{videoItem.VideoWidth}x{videoItem.VideoHeight}";
        txtSize.Text = Math.Round((videoItem.VideoSize * 1.0 / 1048576), 2).ToString();
    }
}
