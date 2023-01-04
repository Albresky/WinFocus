using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;
using WinFocus.Core.Helpers;
using WinFocus.Core.Services;
using WinFocus.ViewModels;
using WinUIEx;

namespace WinFocus.Views;

public sealed partial class FocusGalleryPage : Page
{
    private WallpaperStyle style = WallpaperStyle.Fill;

    public FocusGalleryViewModel ViewModel
    {
        get;
    }

    public FocusGalleryPage()
    {
        ViewModel = App.GetService<FocusGalleryViewModel>();
        ViewModel.SetCurrentPage(this);
        InitializeComponent();
    //     LiveWallpaperPage liveWallpaperPage = new();
    //     liveWallpaperPage.SetWindowSize(3840, 2160);
    //     liveWallpaperPage.Activate();
    //     liveWallpaperPage.CenterOnScreen(); liveWallpaperPage.Maximize();
    //     LiveWallpaperService.SetLiveWallpaper(liveWallpaperPage.GetWindowHandle());
    //     LiveWallpaperWindow.RemoveTitleBar(liveWallpaperPage.GetWindowHandle());
    }

    private void DropDownItemClicked(object sender, RoutedEventArgs e)
    {
        if (Enum.TryParse<WallpaperStyle>((sender as MenuFlyoutItem).Text.ToString(), out style))
        {
            var curViewIndex = myFlipView.SelectedIndex;
            var imagePath = ViewModel.Source.ElementAt(curViewIndex).ImagePath;
            WallpaperService.Set(imagePath, style);
            Console.WriteLine("selected style:{0}", (sender as MenuFlyoutItem).Text.ToString());
        }
    }

    private void ListsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView lv = sender as ListView;
        myFlipView.SelectedIndex = lv.SelectedIndex;
    }
}
