using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WinFocus.Core.Models;
using WinFocus.ViewModels;

namespace WinFocus.Views;

public sealed partial class FocusGalleryPage : Page
{
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

    public void btn_setAsBg_clicked(object sender, RoutedEventArgs e)
    {
        var curViewIndex = myFlipView.SelectedIndex;
        var imagePath = ViewModel.Source.ElementAt(curViewIndex).ImagePath;
        WallpaperService.Set(imagePath, WallpaperStyle.Fill);
    }


}
