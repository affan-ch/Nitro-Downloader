using Microsoft.UI.Xaml.Controls;
using Nitro_Downloader.ViewModels;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Nitro_Downloader.DBM;

namespace Nitro_Downloader.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
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

        LocationTextBox.Text = Settings.GetDownloadLocation();
    }

    private async void ChangeLocationButton_ClickAsync(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var openPicker = new FolderPicker();

        var hWnd = WindowNative.GetWindowHandle(App.MainWindow);

        InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        var folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            LocationTextBox.Text = folder.Path;

            
            Settings.SetDownloadLocation(folder.Path);
        }

    }
}
