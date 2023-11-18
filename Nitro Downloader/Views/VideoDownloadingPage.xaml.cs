using Microsoft.UI.Xaml.Controls;
using Nitro_Downloader.ViewModels;
using Nitro_Downloader.DBM;
using Microsoft.UI.Xaml;

namespace Nitro_Downloader.Views;

public sealed partial class VideoDownloadingPage : Page
{
    public VideoDownloadingViewModel ViewModel
    {
        get;
    }

    public VideoDownloadingPage()
    {
        ViewModel = App.GetService<VideoDownloadingViewModel>();
        InitializeComponent();

        dataGrid.ItemsSource = DatabaseHelper.GetVideoDownloads();

    }

    private async void DescriptionHyperlinkButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is HyperlinkButton button && button.Tag != null)
        {
            var description = button.Tag.ToString();

            var dialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Video Description",
                PrimaryButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary,
                Content = description
            };

            await dialog.ShowAsync();
        }
    }
}
