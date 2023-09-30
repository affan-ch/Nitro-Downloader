﻿using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Text.Json;
using Nitro_Downloader.ViewModels;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Shell;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Globalization;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;
using Nitro_Downloader.Helpers;
using WinRT.Interop;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Provider;
using System.Xml.Linq;
using Nitro_Downloader.Services;

namespace Nitro_Downloader.Views;

public class VideoInfo
{
    public string? title
    {
        get; set;
    }

    public string? description
    {
        get; set;
    }

    public string? thumbnail
    {
        get; set;
    }

    public string? extractor_key
    {
        get; set;
    }

    public string? channel_url
    {
        get; set;
    }


    public long? view_count
    {
        get; set;
    }

    public string? webpage_url
    {
        get; set;
    }

    public int? like_count
    {
        get; set;
    }

    public int? comment_count
    {
        get; set;
    }

    public string? channel
    {
        get; set;
    }

    public int? channel_follower_count
    {
        get; set;
    }

    public string? upload_date
    {
        get; set;
    }

    public string? duration_string
    {
        get; set;
    }

    public string? resolution
    {
        get; set;
    }

    public string? ext
    {
        get; set;
    }

    public long? filesize_approx
    {
        get; set;
    }

}


public class HelperFunctions
{
    public static string FormatYouTubeCounts(long views)
    {
        if (views < 1000)
        {
            return views.ToString(); // No formatting needed for less than 1,000 views
        }
        else if (views < 1000000)
        {
            var kViews = views / 1000.0;
            if (kViews < 10)
            {
                return kViews.ToString("0.#") + "K";
            }
            else
            {
                return kViews.ToString("0") + "K";
            }
        }
        else if (views < 1000000000)
        {
            var mViews = views / 1000000.0;
            return mViews.ToString("0.#") + "M";
        }
        else
        {
            var bViews = views / 1000000000.0;
            return bViews.ToString("0.#") + "B";
        }
    }

    public static string FormatSize(long sizeInBytes)
    {
        if (sizeInBytes < 1024)
        {
            return $"{sizeInBytes} bytes";
        }
        else if (sizeInBytes < 1024 * 1024)
        {
            double sizeInKB = sizeInBytes / 1024.0;
            return $"{sizeInKB:F1}KB";
        }
        else if (sizeInBytes < 1024 * 1024 * 1024)
        {
            double sizeInMB = sizeInBytes / (1024.0 * 1024.0);
            return $"{sizeInMB:F1}MB";
        }
        else
        {
            double sizeInGB = sizeInBytes / (1024.0 * 1024.0 * 1024.0);
            return $"{sizeInGB:F1}GB";
        }
    }

}

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }



  

    private async void GetInfoButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ProgressRingStackPanel.Visibility = Visibility.Visible;
        Expander.Visibility = Visibility.Collapsed;
        Link_TextBox.SizeChanged += (sender, e) =>
        {
            Expander.Width = Link_TextBox.ActualWidth;
        };
        GetInfoButton.Content = "Loading...";
        var link = Link_TextBox.Text;

        if (link.Length < 10)
        {
            ContentDialog dialog = new ContentDialog
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "URL Error",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = "Enter a Valid Video Link"
            };

            await dialog.ShowAsync();

            return;

        }

        try
        {
            var jsonOutput = await YtDlpHelper.GetVideoInfoJsonAsync(link);
            var videoInfo = JsonSerializer.Deserialize<VideoInfo>(jsonOutput);
            Debug.WriteLine(videoInfo?.title);

            TitleTextBlock.Text = videoInfo?.title;

            if (videoInfo?.thumbnail != null )
            {
                if (videoInfo?.thumbnail.Length > 10)
                {
                    Microsoft.UI.Xaml.Media.ImageSource imageSource = new BitmapImage(new Uri(videoInfo?.thumbnail ?? "https://www.youtube.com/"));

                    ThumbnailImage.Source = imageSource;
                }
                else
                {
                    ThumbnailTextBlock.Visibility = Visibility.Collapsed;
                    ThumbnailLink.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ThumbnailTextBlock.Visibility = Visibility.Collapsed;
                ThumbnailLink.Visibility = Visibility.Collapsed;
            }

            
            DurationTextBlock.Text = videoInfo?.duration_string;

            var likes = HelperFunctions.FormatYouTubeCounts((long)(videoInfo?.like_count ?? 0));

            LikesTextBlock.Text = likes ;

            var comments = HelperFunctions.FormatYouTubeCounts((long)(videoInfo?.comment_count ?? 0));

            CommentsTextBlock.Text = comments;


            var views = HelperFunctions.FormatYouTubeCounts((long)(videoInfo?.view_count ?? 0));
            ViewsTextBlock.Text = views;

            DateTime.TryParseExact(videoInfo?.upload_date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);

            var formattedDate = parsedDate.ToString("MMM dd, yyyy");

            ResolutionTextBlock.Text = videoInfo?.resolution;

            DateUploadedTextBlock.Text = formattedDate;

            ChannelNameTextBlock.Text = videoInfo?.channel;

            WebsiteTextBlock.Text = videoInfo?.extractor_key;

            var extension = videoInfo?.ext ?? "?";

            ExtensionTextBlock.Text = extension.ToUpper();

            Expander.Width = Link_TextBox.ActualWidth;

            ProgressRingStackPanel.Visibility = Visibility.Collapsed;

            

            GetInfoButton.Content = "Get Info";

            VideoLink.NavigateUri = new Uri(videoInfo?.webpage_url ?? "");
            VideoLink.Content = videoInfo?.webpage_url ?? "";

            ChannelLink.NavigateUri = new Uri(videoInfo?.channel_url ?? "");
            ChannelLink.Content = videoInfo?.channel_url ?? "";

            ThumbnailLink.NavigateUri = new Uri(videoInfo?.thumbnail ?? "");
            ThumbnailLink.Content = videoInfo?.thumbnail ?? "";




            FileSizeTextBlock.Text = HelperFunctions.FormatSize(videoInfo?.filesize_approx??0);


            Expander.Visibility = Visibility.Visible;


 
            //Console.WriteLine(jsonOutput);
        }
        catch (Exception ex)
        {
            //Console.WriteLine("An error occurred: " + ex.Message);
            Debug.WriteLine("An Error Occured: " + ex.Message);
        }



    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        // This method is called when new data is received on the standard output
        if (!string.IsNullOrEmpty(e.Data))
        {
            // Assuming the download percentage appears in the output, you can parse it here
            // Example: Check if the line contains "[download]" and extract the percentage
            if (e.Data.Contains("[download]"))
            {
                var startIndex = e.Data.IndexOf("[download]") + "[download]".Length;
                var endIndex = e.Data.IndexOf("%", startIndex);
                if (startIndex >= 0 && endIndex >= 0)
                {
                    var percentageString = e.Data.Substring(startIndex, endIndex - startIndex).Trim();
                    if (int.TryParse(percentageString, out var percentage))
                    {
                        // Update your UI with the download percentage
                        // You can use Dispatcher to update UI elements if needed
                        Debug.WriteLine("Download Percentage: " + percentage + "%");
                    }
                }
            }
        }
    }

    private async void ChangeLocationButton_ClickAsync(object sender, RoutedEventArgs e)
    {
        // Clear previous returned file name, if it exists, between iterations of this scenario
        LocationTextBox.Text = "";

        // Create a folder picker
        var openPicker = new FolderPicker();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        //var window = WindowHelper.GetWindowForElement(this);
        //var windows = NavigationHelper

        //var window = Window.Current;

        var hWnd = WindowNative.GetWindowHandle(App.MainWindow);

        // Initialize the folder picker with the window handle (HWND).
        InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your folder picker
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        // Open the picker for the user to pick a folder
        var folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            LocationTextBox.Text = "Picked folder: " + folder.Name;
        }
        else
        {
            LocationTextBox.Text = "Operation cancelled.";
        }
    }

}


public static class YtDlpHelper
{
    public static async Task<string> GetVideoInfoJsonAsync(string link)
    {
        var fullPath = AppDomain.CurrentDomain.BaseDirectory.ToString();

        var endIndex = fullPath.IndexOf("Nitro Downloader\\");
        var path = fullPath[..(endIndex + "Nitro Downloader\\".Length)];
        path += "Tools\\yt-dlp.exe";

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("yt-dlp.exe not found at the specified path.");
        }

        var arguments = $"{link} --skip-download --dump-json";

        using var process = new Process();
        process.StartInfo.FileName = path;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;

        var outputBuilder = new StringBuilder();

        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                outputBuilder.AppendLine(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();

        await Task.Run(() =>
        {
            process.WaitForExit();
        });

        if (process.ExitCode == 0)
        {
            var jsonOutput = outputBuilder.ToString();
            return jsonOutput;
        }
        else
        {
            throw new Exception("yt-dlp command failed.");
        }
    }
}
