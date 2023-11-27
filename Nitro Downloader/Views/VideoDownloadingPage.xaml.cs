using Microsoft.UI.Xaml.Controls;
using Nitro_Downloader.ViewModels;
using Nitro_Downloader.DBM;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;
using System.Windows;

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

        var list = DatabaseHelper.GetVideoDownloads();
        var downloadingList = list.Where(item => item.Status == "Downloading").ToList();
        dataGrid.ItemsSource = downloadingList;

        list.OnChanged += List_OnChanged;

        //var timer = new DispatcherTimer();
        //timer.Tick += RefreshGrid;
        //timer.Interval = TimeSpan.FromSeconds(1);
        //timer.Start();
    }

    private void List_OnChanged(object? sender, EventArgs e) 
    {
        Debug.WriteLine("Refreshing downloading grid");
        var list = DatabaseHelper.GetVideoDownloads();
        var downloadingList = list.Where(item => item.Status == "Downloading").ToList();
        dataGrid.ItemsSource = downloadingList;
    }

    //private void RefreshGrid(object? sender, object e)
    //{
    //    var list = DatabaseHelper.GetVideoDownloads();

    //    var isDownloading = list.Any(item => item.Status == "Downloading");

    //    if (isDownloading)
    //    {
    //        Debug.WriteLine("Refreshing grid");
    //        dataGrid.ItemsSource = null;
    //        var downloadingList = list.Where(item => item.Status == "Downloading").ToList();
    //        dataGrid.ItemsSource = downloadingList;
    //    }
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


public class VisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string _)
    {
        Debug.WriteLine(value as string);
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
            Debug.WriteLine("String Value:");
            Debug.WriteLine(stringValue);
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