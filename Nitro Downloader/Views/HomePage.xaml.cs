using System.Diagnostics;
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
using Nitro_Downloader.DBM;
using WinRT.Interop;

using Nitro_Downloader.BL;

namespace Nitro_Downloader.Views;


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
        LocationTextBox.Text = Settings.GetDownloadLocation();
    }



  

    private async void GetInfoButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var link = Link_TextBox.Text;       

        if (link.Length < 10)
        {
            var dialog = new ContentDialog
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


        ProgressRingStackPanel.Visibility = Visibility.Visible;
        Expander.Visibility = Visibility.Collapsed;
        Link_TextBox.SizeChanged += (sender, e) =>
        {
            Expander.Width = Link_TextBox.ActualWidth;
        };
        GetInfoButton.Content = "Loading...";


        

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

            DateTime.TryParseExact(videoInfo?.upload_date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate);

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




            FileSizeTextBlock.Text = HelperFunctions.FormatFileSize(videoInfo?.filesize_approx??0);


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

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        var link = Link_TextBox.Text;
        var output = await YtDlpHelper.DownloadVideoAsync(link);

        Debug.WriteLine("\n\n\nOutput Started");
        Debug.WriteLine(output);
        Debug.WriteLine("\n\n\nOutput Ended");

        var title = TitleTextBlock.Text;

        var downloadLocation = Settings.GetDownloadLocation();

        HelperFunctions.ShowDownloadCompleteNotification(title, downloadLocation);

    }
}

