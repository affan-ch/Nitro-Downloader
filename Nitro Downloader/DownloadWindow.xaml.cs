using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Nitro_Downloader.BL;
using Nitro_Downloader.DBM;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI;
using WinRT.Interop;

namespace Nitro_Downloader;

public sealed partial class DownloadWindow : WindowEx
{
    private readonly string URL;
    private readonly string DownloadType;

    public DownloadWindow(string URL, string DownloadType)
    {
        InitializeComponent();

        IsTitleBarVisible = true;
        ExtendsContentIntoTitleBar = true;
        IsResizable = true;
        MinHeight = 455;
        MinWidth = 790;
        Height = 455;
        Width = 790;
        MaxHeight = 505;
        MaxWidth = 900;
        
        Title = "Nitro Downloader";
        this.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        SetTitleBar(titleBar);

        this.URL = URL;
        this.DownloadType = DownloadType;


        URL_TextBox.Text = URL;
        Location_TextBox.Text = Settings.GetDownloadLocation();

        Get_URL_Info(URL);

    }


    private void MinimizeToTrayButton_Click(object _, RoutedEventArgs __)
    {
        this.Hide();
    }

    private void MinimizeToTrayButton_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        //var hexColor = "#191919";
        //var color = Color.FromArgb(
        //    255, // Alpha value (255 for fully opaque)
        //    Convert.ToByte(hexColor.Substring(1, 2), 16), // Red component
        //    Convert.ToByte(hexColor.Substring(3, 2), 16), // Green component
        //    Convert.ToByte(hexColor.Substring(5, 2), 16)  // Blue component
        //);



        //MinimizeToTrayButton.Background = new SolidColorBrush(Colors.Red);

        Debug.WriteLine("Entered");

    }

    private void MinimizeToTrayButton_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        MinimizeToTrayButton.Background = new SolidColorBrush(Colors.Transparent);
        Debug.WriteLine("Leave");
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
            Location_TextBox.Text = folder.Path;

            Settings.SetDownloadLocation(folder.Path);
        }
    }

    private void OpenURL_Button_Click(object sender, RoutedEventArgs e)
    {
        var link = URL_TextBox.Text;

        Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });

    }

    private async void Get_URL_Info(string link)
    {
        try
        {
            var jsonOutput = await YtDlpHelper.GetVideoInfoJsonAsync(link);
            var videoInfo = JsonSerializer.Deserialize<VideoInfo>(jsonOutput);
            Debug.WriteLine(videoInfo?.title);

            if (videoInfo?.thumbnail != null)
            {
                if (videoInfo?.thumbnail.Length > 10)
                {
                    ImageSource imageSource = new BitmapImage(new Uri(videoInfo?.thumbnail ?? "https://www.youtube.com/"));

                    ThumbnailImage.Source = imageSource;
                }
            }

            TitleTextBlock.Text = videoInfo?.title;

            DurationTextBlock.Text = videoInfo?.duration_string;



            var likes = HelperFunctions.FormatYouTubeCounts((long)(videoInfo?.like_count ?? 0));

            LikesTextBlock.Text = likes;

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



            FileSizeTextBlock.Text = HelperFunctions.FormatFileSize(videoInfo?.filesize_approx ?? 0);

            ProgressRingStackPanel.Visibility = Visibility.Collapsed;
            BodyContainer.Visibility = Visibility.Visible;


        }
        catch (Exception ex)
        {
            //Console.WriteLine("An error occurred: " + ex.Message);
            Debug.WriteLine("An Error Occured: " + ex.Message);
        }
    }

    private void StartDownload_Button_Click(object sender, RoutedEventArgs e)
    {
        BodyContainer.Visibility = Visibility.Collapsed;

        ProgressRingStackPanel.Visibility = Visibility.Collapsed;


    }
}
