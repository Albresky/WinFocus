using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media;
using WinFocus.Core.Contracts.Services;
using WinFocus.Core.Models;
using WinFocus.Core.Services;
using WinFocus.ViewModels;

namespace WinFocus.Views;

public sealed partial class LiveWallpaperGalleryPage : Page
{
    public void setThumbnail(BitmapImage bitmapImage)
    {
        thumbnail.Source = bitmapImage;
    }

    public void SetGridViewSource(IList<VideoItem> source) => VideoGridView.ItemsSource = source;

    public LiveWallpaperGalleryViewModel ViewModel
    {
        get;
    }

    
    public LiveWallpaperGalleryPage()
    {
        ViewModel = App.GetService<LiveWallpaperGalleryViewModel>();
        ViewModel.SetCurrentPage(this);
        this.InitializeComponent();

    }
}
