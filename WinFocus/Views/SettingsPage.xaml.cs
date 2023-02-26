using System.Diagnostics.Eventing.Reader;
using Microsoft.UI.Xaml.Controls;
using WinFocus.Core.Services;
using WinFocus.ViewModels;
using WinFocus.Core.Models;

namespace WinFocus.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }


    private async void SetFolderAndSave(int type)
    {
        var p = await SystemService.GetFolderPickerAsync(new string[] { "*" });
        if (p != null)
        {
            var path = p.Path;
            SettingsTypes.PathType t;
            switch (type)
            {
                case 0:
                    txtFwDir.Text = path;
                    t = SettingsTypes.PathType.FocusImagePath;
                    break;
                case 1:
                    txtLwDir.Text = path;
                    t = SettingsTypes.PathType.LiveWallpaperVideoPath;
                    break;
                case 2:
                    txtBwDir.Text = path;
                    t = SettingsTypes.PathType.BingWallpaperImagePath;
                    break;
                default:
                    t = SettingsTypes.PathType.FocusImagePath;
                    break;
            }
            Core.CoreEngine.Current.AppSetting.SetAssetsPath(path, t);
        }
    }
    private async void Button_FwDir_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        SetFolderAndSave(0);
    }
    private async void Button_LwDir_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        SetFolderAndSave(1);

    }
    private async void Button_BwDir_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        SetFolderAndSave(2);
    }
}
