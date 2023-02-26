using System.Diagnostics;
using Microsoft.UI.Xaml;
using Windows.Graphics.Display;
using Windows.Media.Core;
using Windows.Storage;
using static WinFocus.Core.Services.SystemService;

namespace WinFocus.Views;

public sealed partial class LiveWallpaperPage : Window
{
    public string? videoFile
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
        var file = await StorageFile.GetFileFromPathAsync(videoFile);
        var source = MediaSource.CreateFromStorageFile(file);
        mediaPlayerElement.AutoPlay = true;
        mediaPlayerElement.IsFullWindow = true;
        mediaPlayerElement.MediaPlayer.IsLoopingEnabled = true;
        mediaPlayerElement.Source = source;
        mediaPlayerElement.MediaPlayer.AutoPlay = true;
    }

    public void Stop()
    {
        var player = mediaPlayerElement?.MediaPlayer;
        if (player != null)
        {
            player.Pause();
            this.Hide();
        }
        WallpaperService.BackupState();
        WallpaperService.RestoreState();
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
        mediaPlayerElement.MediaPlayer.IsMuted = true;
    }

    public void SetVolume(double v)
    {
        if (mediaPlayerElement.MediaPlayer != null)
        {
            mediaPlayerElement.MediaPlayer.Volume = v;
        }
        Trace.WriteLine($"Current Volume of Video:{v}");
    }
}
