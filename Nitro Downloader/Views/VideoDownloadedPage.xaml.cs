using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;

using Nitro_Downloader.ViewModels;
using Nitro_Downloader.DBM;

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

        var list = DatabaseHelper.GetVideoDownloads();
        var downloadedList = list.Where(item => item.Status == "Downloaded").ToList();
        dataGrid.ItemsSource = downloadedList;

        list.OnChanged += List_OnChanged;


        //var timer = new DispatcherTimer();
        //timer.Tick += RefreshGrid;
        //timer.Interval = TimeSpan.FromSeconds(3);
        //timer.Start();
    }

    private void List_OnChanged(object? sender, EventArgs e)
    {
        Debug.WriteLine("Refreshing downloaded grid");
        var list = DatabaseHelper.GetVideoDownloads();
        var downloadedList = list.Where(item => item.Status == "Downloaded").ToList();
        dataGrid.ItemsSource = downloadedList;
    }

    //private void RefreshGrid(object? sender, object e)
    //{
    //    var list = DatabaseHelper.GetVideoDownloads();
    //    var downloadedList = list.Where(item => item.Status == "Downloaded").ToList();
    //    dataGrid.ItemsSource = downloadedList;
    //}

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
