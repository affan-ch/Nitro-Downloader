using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Nitro_Downloader.ViewModels;
using Nitro_Downloader.DL;

namespace Nitro_Downloader.Views;

public sealed partial class VideoDownloadedPage : Page
{
    public VideoDownloadedViewModel ViewModel
    {
        get;
    }

    public VideoDownloadedPage()
    {
        ViewModel = App.GetService<VideoDownloadedViewModel>();
        InitializeComponent();

        var list = VideoDownloadDL.GetVideoDownloadsList();
        var downloadedList = list.Where(item => item.Status == "Downloaded").ToList();
        dataGrid.ItemsSource = downloadedList;

 
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
