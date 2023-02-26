using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

using WinFocus.Contracts.Services;
using WinFocus.Core.Models;
using WinFocus.Helpers;

namespace WinFocus.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;
    private string _versionDescription;
    private string _focusWallpaperDir;
    private string _liveWallpaperDir;
    private string _bingWallpaperDir;

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public string FocusWallpaperDir
    {
        get => _focusWallpaperDir;
        set => SetProperty(ref _focusWallpaperDir, value);
    }

    public string LiveWallpaperDir
    {
        get => _liveWallpaperDir;
        set => SetProperty(ref _liveWallpaperDir, value);
    }

    public string BingWallpaperDir
    {
        get => _bingWallpaperDir;
        set => SetProperty(ref _bingWallpaperDir, value);
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        _focusWallpaperDir = Core.CoreEngine.Current.AppSetting.GetAssetsPath(SettingsTypes.PathType.FocusImagePath);
        _liveWallpaperDir = Core.CoreEngine.Current.AppSetting.GetAssetsPath(SettingsTypes.PathType.LiveWallpaperVideoPath);
        _bingWallpaperDir = Core.CoreEngine.Current.AppSetting.GetAssetsPath(SettingsTypes.PathType.BingWallpaperImagePath);

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

}
