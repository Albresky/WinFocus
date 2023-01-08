using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics.Display;
using Windows.Media.Core;
using Windows.Storage;
using WinUIEx;
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
        InitializeComponent();
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

    public void Stop()
    {
        if (MediaPlayerElement != null )
        {
            var player = MediaPlayerElement.MediaPlayer;
            if (player != null)
            {
                player.Pause();
                this.Hide();
            }
        }
    }

    public static ScreenSize GetSysScreenSize()
    {
        var displayInformation = DisplayInformation.GetForCurrentView();
        return new ScreenSize
        {
            Width = displayInformation.ScreenWidthInRawPixels,
            Height = displayInformation.ScreenHeightInRawPixels
        };
    }

    public void SetMute()
    {
        MediaPlayerElement.MediaPlayer.IsMuted = true;
    }

    public void SetVolume(double v)
    {
        MediaPlayerElement.MediaPlayer.Volume = v;
        Trace.WriteLine($"Current Volume of Video:{v}");
    }
}
