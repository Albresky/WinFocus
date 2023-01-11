using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Media.Core;
using WinFocus.Core.Models;
using WinFocus.Helpers;

namespace WinFocus.Views;

public sealed partial class MediaPlayerPage : Page
{
    public VideoItem MediaObject
    {
        get; set;
    }

    public MediaSource mSource;
    public MediaPlayerPage()
    {
        TraceHelper.TraceClass(this);
        InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Disabled;
        Btn_GoBack.Loaded += GoBackButton_Loaded;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Trace.WriteLine($"OnNavigatedTo:{GetType().Name}");
        base.OnNavigatedTo(e);

        // Store the item to be used in binding to UI
        MediaObject = e.Parameter as VideoItem;

        mSource = MediaSource.CreateFromUri(new Uri($"file:///{MediaObject.VideoPath}"));

        var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
        animation.TryStart(mediaPlayerElement);
    }

    private void GoBackButton_Loaded(object sender, RoutedEventArgs e)
    {
        // When we land in page, put focus on the back button
        Btn_GoBack.Focus(FocusState.Programmatic);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackConnectedAnimation", mediaPlayerElement);
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}
