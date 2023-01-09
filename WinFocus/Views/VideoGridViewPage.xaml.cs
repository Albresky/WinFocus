using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;
using WinFocus.Core.Models;
using WinFocus.ViewModels;

namespace WinFocus.Views;
public sealed partial class VideoGridViewPage : Page
{
    private int gridview_index = 0;
    private VideoItem _storeditem;
    private LiveWallpaperGalleryPage pageFrom;


    public LiveWallpaperGalleryViewModel ViewModel
    {
        get; set;
    }

    public VideoGridViewPage()
    {
        Trace.WriteLine(GetType().Name);
        InitializeComponent();
    }

    private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var gv = sender as GridView;
        if (gv != null)
        {
            gridview_index = gv.SelectedIndex;
            pageFrom.SelectedVideoItemChanged(ViewModel.Source.ElementAt(gridview_index));
        }
    }

    private void VideoGridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (VideoGridView.ContainerFromItem(e.ClickedItem) is GridViewItem container)
        {
            _storeditem = container.Content as VideoItem;
            var animation =  VideoGridView.PrepareConnectedAnimation("ForwardConnectedAnimation", _storeditem, "connectedElement");
        }

        Frame.Navigate(typeof(MediaPlayerPage), _storeditem, new SuppressNavigationTransitionInfo());
    }

    private async void VideoGridView_Loaded(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("VideoGridView_Loaded()");
        if (ViewModel?.Source?.Count >= 0)
        {
            StopProcessRing();
        }
        if (_storeditem != null)
        {
            VideoGridView.ScrollIntoView(_storeditem, ScrollIntoViewAlignment.Default);
            VideoGridView.UpdateLayout();

            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null)
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                {
                    animation.Configuration = new DirectConnectedAnimationConfiguration();
                }

                await VideoGridView.TryStartConnectedAnimationAsync(animation, _storeditem, "connectedElement");
            }

            VideoGridView.Focus(FocusState.Programmatic);
        }
    }

    private void VideoGridView_Loading(FrameworkElement sender, object args)
    {
        //ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
        //if (animation != null)
        //{
        //    // Setup the "back" configuration if the API is present. 
        //    if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
        //    {
        //        animation.Configuration = new DirectConnectedAnimationConfiguration();
        //    }
        //    await VideoGridView.TryStartConnectedAnimationAsync(animation,)
        //    await collection.TryStartConnectedAnimationAsync(animation, _storeditem, "connectedElement");
        //}
    }

    public void StartProcessRing()
    {
        Trace.WriteLine("StartProcessRing called.");
        processRing.IsActive = true;
        Trace.WriteLine($"ProcessRing:{processRing.IsActive}");
    }

    public void StopProcessRing()
    {
        Trace.WriteLine("StopProcessRing called.");
        processRing.IsActive = false;
        Trace.WriteLine($"ProcessRing:{processRing.IsActive}");
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Trace.WriteLine($"OnNavigatedTo:{GetType().Name}");
        StartProcessRing();
        pageFrom = e?.Parameter as LiveWallpaperGalleryPage;
        ViewModel = App.GetService<LiveWallpaperGalleryViewModel>();
        ViewModel.SetCurrentPage(this);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        Trace.WriteLine($"OnNavigatedFrom:{GetType().Name}");
        base.OnNavigatingFrom(e);
    }
}
