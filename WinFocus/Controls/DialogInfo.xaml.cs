using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace WinFocus.Controls;

public sealed partial class DialogInfo : Page
{
    public DialogInfo()
    {
        this.InitializeComponent();
    }

    public async void DisplayToDoDialog()
    {
        ContentDialog todoDialog = new ContentDialog
        {
            Title = "ToDo",
            Content = "The detail page of these images is not avaliable right now.",
            CloseButtonText = "I'll be waiting!",
            XamlRoot = Window.Current.Content.XamlRoot
        };
        await todoDialog.ShowAsync();
    }
}
