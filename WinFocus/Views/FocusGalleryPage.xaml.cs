using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WinFocus.Core.Models;
using WinFocus.ViewModels;

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
    }

    private void DropDownItemClicked(object sender, RoutedEventArgs e)
    {
        if(Enum.TryParse<WallpaperStyle>((sender as MenuFlyoutItem).Text.ToString(), out style))
        {
            Console.WriteLine("selected style:{0}", (sender as MenuFlyoutItem).Text.ToString());
        }
    }

    private void btn_setAsBg_clicked(object sender, RoutedEventArgs e)
    {
        var curViewIndex = myFlipView.SelectedIndex;
        var imagePath = ViewModel.Source.ElementAt(curViewIndex).ImagePath;
        WallpaperService.Set(imagePath, WallpaperStyle.Fill);
    }


}
