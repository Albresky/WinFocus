using Microsoft.UI.Xaml;

using WinFocus.Contracts.Services;
using WinFocus.Helpers;

namespace WinFocus.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    public async Task InitializeAsync()
    {
        Theme = LoadThemeFromSettings();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        SaveThemeInSettings(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    private ElementTheme LoadThemeFromSettings()
    {
        var themeName = Core.CoreEngine.Current.AppSetting.GetAppTheme();
        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }
        return ElementTheme.Default;
    }

    private void SaveThemeInSettings(ElementTheme theme)
    {
       Core.CoreEngine.Current.AppSetting.SetAppTheme(theme.ToString());
    }
}
