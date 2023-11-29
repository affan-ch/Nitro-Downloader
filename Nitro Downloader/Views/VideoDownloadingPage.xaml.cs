using Microsoft.UI.Xaml.Controls;
using Nitro_Downloader.ViewModels;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Data;
using Nitro_Downloader.DL;

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

        var list = VideoDownloadDL.GetVideoDownloadsList();
        var downloadingList = list.Where(item => item.Status == "Downloading").ToList();
        dataGrid.ItemsSource = downloadingList;

        list.CollectionChanged += (sender, e) =>
        {  
            Debug.WriteLine("Refreshing downloading grid");

            dataGrid.ItemsSource = null;
            var downloadingList = list.Where(item => item.Status == "Downloading").ToList();
            dataGrid.ItemsSource = downloadingList;
        };

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


public class VisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string _)
    {
        return value == null || string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : (object)Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string _)
    {
        throw new NotImplementedException();
    }
}

public class StringToUriConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string stringValue && stringValue != "")
        {
            return new Uri(stringValue);
        }
        else
        {
            return new Uri("https://codehub.pk/");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}