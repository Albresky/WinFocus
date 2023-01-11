using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinFocus.Core.Models;
using WinFocus.ViewModels;

namespace WinFocus.Views;

public sealed partial class FocusGalleryPage : Page
{
    private WallpaperStyle style = WallpaperStyle.Fill;
    private int image_index = 0;
    private int rbs_index = 0;

    public FocusGalleryViewModel ViewModel
    {
        get;
    }

    public FocusGalleryPage()
    {
        Trace.WriteLine("FocusGalleryPage() called.");
        ViewModel = App.GetService<FocusGalleryViewModel>();
        ViewModel.SetCurrentPage(this);
        InitializeComponent();
        Init();
    }

    private void Init()
    {
        if (myListView.Items.Count > 0)
        {
            myListView.SelectedIndex = 0;
            txtResolution.Text = ViewModel.Source.ElementAt(0).ImageResolution;
            txtSize.Text = (ViewModel.Source.ElementAt(0).ImageSize / 1000).ToString() + "KB";
            txtDate.Text = ViewModel.Source.ElementAt(0).ImageDate.ToString();
        }
    }

    private void RadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var RBs = sender as RadioButtons;

        if (RBs != null && sender is RadioButtons rb)
        {
            rbs_index = RBs.SelectedIndex;
            var str_style = rb.SelectedItem as string;
            if (Enum.TryParse<WallpaperStyle>(str_style, out style))
            {

                Console.WriteLine("selected style:{0}", str_style);
            }
        }
    }

    private void Button_SetAsWallpaper_Click(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("Button<SetAsWallpaper> Clicked.");
        var imagePath = ViewModel.Source.ElementAt(image_index).ImagePath;
        WallpaperService.Set(imagePath, style);
    }

    private void ListsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var lv = sender as ListView;
        if (lv != null)
        {
            image_index = lv.SelectedIndex;
            myFlipView.SelectedIndex = image_index;
            txtResolution.Text = ViewModel.Source.ElementAt(image_index).ImageResolution;
            txtSize.Text = (ViewModel.Source.ElementAt(image_index).ImageSize / 1000).ToString() + "KB";
            txtDate.Text = ViewModel.Source.ElementAt(image_index).ImageDate.ToString();
            Trace.WriteLine("Current Image Index:{0}", image_index.ToString());
        }
    }


}
