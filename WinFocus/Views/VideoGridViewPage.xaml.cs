using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;
using WinFocus.Core.Models;
using WinFocus.Core.Services;
using WinFocus.ViewModels;

namespace WinFocus.Views;
public sealed partial class VideoGridViewPage : Page
{
    private int gridview_index = -1;
    private VideoItem _storeditem;
    private LiveWallpaperGalleryPage pageFrom;
    private int prevCnt_VideoItems = 0;
    private bool IsAddItemCreated;

    public LiveWallpaperGalleryViewModel ViewModel
    {
        get; set;
    }

    public VideoGridViewPage()
    {
        Trace.WriteLine(GetType().Name);
        InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Enabled;
    }

    private void VideoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var gv = sender as GridView;
        if (gv != null && (gv.SelectedIndex < gv.Items.Count - 1 || gridview_index < 0))
        {
            gridview_index = gv.SelectedIndex;
            pageFrom.SelectedVideoItemChanged(ViewModel.Source.ElementAt(gridview_index));
        }
        else { gv.SelectedIndex = gridview_index; }
    }

    private async void VideoGridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is VideoItem videoItem && videoItem.IsButton)
        {
            if (!processRing.IsActive)
            {
                StartProcessRing();
            }
            var newVideoItemsPath = await SystemService.GetFilesInFilePickerAsync(new string[] { ".mp4", ".mkv" });
            if (newVideoItemsPath.Count > 0)
            {
                await ViewModel.AddVideoItemAsync(newVideoItemsPath);
            }
            else if (processRing.IsActive)
            {
                StopProcessRing();
            }
            return;
        }
        if (VideoGridView.ContainerFromItem(e.ClickedItem) is GridViewItem container)
        {
            _storeditem = container.Content as VideoItem;
            var animation = VideoGridView.PrepareConnectedAnimation("ForwardConnectedAnimation", _storeditem, "connectedElement");
        }

        Frame.Navigate(typeof(MediaPlayerPage), _storeditem, new SuppressNavigationTransitionInfo());
    }

    private async void VideoGridView_Loaded(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("VideoGridView_Loaded()");
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

    private void CreateVideoImportItem()
    {
        var item = new VideoItem { ThumbnailPath = "D:\\Code\\C# Workspace\\WinFocus\\WinFocus\\Assets\\add.png", IsButton = true };
        ViewModel.Source.Add(item);
    }

    public void StartProcessRing()
    {
        Trace.WriteLine("StartProcessRing()");
        processRing.IsActive = true;
    }

    public void StopProcessRing()
    {
        Trace.WriteLine("StopProcessRing()");
        processRing.IsActive = false;
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

    private void connectedElement_Loaded(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("connectedElement_Loaded()");
        if (0 == (--prevCnt_VideoItems))
        {
            if (!IsAddItemCreated)
            {
                IsAddItemCreated = true;
                CreateVideoImportItem();
            }
            StopProcessRing();
        }
    }

    private void connectedElement_Loading(FrameworkElement sender, object args)
    {
        Trace.WriteLine("connectedElement_Loading()");
        prevCnt_VideoItems++;
    }
}
