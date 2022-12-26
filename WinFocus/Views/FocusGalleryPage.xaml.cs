using Microsoft.UI.Xaml.Controls;

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
}
