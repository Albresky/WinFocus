using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Vanara.Extensions.Reflection;
using Windows.Graphics.Display;
using Windows.Media.Core;
using Windows.Storage;
using static WinFocus.Core.Services.SystemService;

namespace WinFocus.Views;

public sealed partial class LiveWallpaperPage : Window
{
    public string VideoFile
    {
       set;
       private get;
    }

    public LiveWallpaperPage()
    {
        this.InitializeComponent();
    }

    public async void Play()
    {
        var file = await StorageFile.GetFileFromPathAsync(VideoFile);
        var source = MediaSource.CreateFromStorageFile(file);
        MediaPlayerElement.AutoPlay = true;
        MediaPlayerElement.IsFullWindow = true;
        MediaPlayerElement.MediaPlayer.IsLoopingEnabled = true;
        MediaPlayerElement.Source = source;
        MediaPlayerElement.MediaPlayer.AutoPlay = true;
    }

    public ScreenSize GetSysScreenSize()
    {
        var displayInformation = DisplayInformation.GetForCurrentView();
        return new ScreenSize
        {
            Width = displayInformation.ScreenWidthInRawPixels,
            Height = displayInformation.ScreenHeightInRawPixels
        };
    }

    //private OverlappedPresenter GetAppWindowAndPresenter()
    //{
    //    var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
    //    WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
    //    var _apw = AppWindow.GetFromWindowId(myWndId);
    //    return _apw.Presenter as OverlappedPresenter;
    //}

    //public  void Maximize(this Window window)
    //{
    //var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
    //Vanara.PInvoke.User32.ShowWindow(windowHandle, Vanara.PInvoke.ShowWindowCommand.SW_MAXIMIZE);
    //}
}
