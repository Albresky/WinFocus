using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Vanara.Extensions.Reflection;
using Windows.Media.Core;
using Windows.Storage;
namespace WinFocus.Views;

public sealed partial class LiveWallpaperPage : Window
{
    public LiveWallpaperPage()
    {
        this.InitializeComponent();
        //Play();
    }

    async void Play()
    {
        var file = await StorageFile.GetFileFromPathAsync("C:\\Users\\Albre\\Downloads\\Video\\Ink.mp4");
        var source = MediaSource.CreateFromStorageFile(file);
        MediaPlayerElement.AutoPlay = true;
        MediaPlayerElement.IsFullWindow = true;
        MediaPlayerElement.MediaPlayer.IsLoopingEnabled = true;
        MediaPlayerElement.Source = source;
        MediaPlayerElement.MediaPlayer.AutoPlay = true;
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
