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
using Windows.Storage;
using Nitro_Downloader.DBM;
using WinRT.Interop;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Provider;
using System.Xml.Linq;
using Nitro_Downloader.Services;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
//using H.NotifyIcon.EfficiencyMode;
//using H.NotifyIcon.Interop;
//using H.NotifyIcon.Core;

using Microsoft.Toolkit.Uwp.Notifications;
//using H.NotifyIcon;

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
            var sizeInKB = sizeInBytes / 1024.0;
            return $"{sizeInKB:F1}KB";
        }
        else if (sizeInBytes < 1024 * 1024 * 1024)
        {
            var sizeInMB = sizeInBytes / (1024.0 * 1024.0);
            return $"{sizeInMB:F1}MB";
        }
        else
        {
            var sizeInGB = sizeInBytes / (1024.0 * 1024.0 * 1024.0);
            return $"{sizeInGB:F1}GB";
        }
    }

    public static void ShowDownloadCompleteNotification(string fileName, string filePath)
    {
        var content = new ToastContentBuilder()
        .AddText("Download Completed")
        .AddText($"{fileName}")
        .AddButton(new ToastButton("Open File", $"action=openFile&filePath={filePath}")
        {
            ActivationType = ToastActivationType.Background,
        })
        .AddButton(new ToastButton("Show in Folder", $"action=showInFolder&filePath={filePath}")
        {
            ActivationType = ToastActivationType.Background,
        });

        var toast = new ToastNotification(content.GetXml());
        ToastNotificationManagerCompat.CreateToastNotifier().Show(toast);

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
        LocationTextBox.Text = Settings.GetDownloadLocation();
        //EfficiencyModeUtilities.SetEfficiencyMode(true);


        //H.NotifyIcon.Interop.IconUtilities.GetRequiredCustomIconSize(true);
        //H.NotifyIcon.Core.TrayIconWithContextMenu trayIcon = new H.NotifyIcon.Core.TrayIconWithContextMenu();
        //trayIcon.Icon ;
        //trayIcon.Show();

        //var trayIcon = new TaskbarIcon();
        //trayIcon.Icon = new System.Drawing.Icon("Assets/Red.ico");
        //trayIcon.ToolTipText = "Nitro Downloader";
        //trayIcon.ShowNotification("Nitro Downloader", "Nitro Downloader is running in the background.");
        //trayIcon.Visible = true;
        //trayIcon.ShowBalloonTip("Nitro Downloader", "Nitro Downloader is running in the background.", BalloonIcon.Info);
        //trayIcon.Icon = new System.Drawing.Icon("Assets/Red.ico");
        //trayIcon.ToolTip = "Nitro Downloader";
        //trayIcon.Create();
        //trayIcon.Show();
        //using var iconStream = H.Resources.Red_ico.AsStream();
        //using var icon = new H.NotifyIcon.;
        //trayIcon.UpdateIcon(icon.Handle);



    }



  

    private async void GetInfoButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var link = Link_TextBox.Text;

        //var fileName = "Zara Khan Dance In Dubai. Filmfare Award Dubai";
        //var filePath = "C:\\Users\\affan\\Downloads\\Video\\Zara Khan Dance In Dubai. Filmfare Award Dubai.mp4";
       

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


public static class YtDlpHelper
{

    public static async Task<string> DownloadVideoAsync(string link)
    {
        var fullPath = AppDomain.CurrentDomain.BaseDirectory.ToString();

        var endIndex = fullPath.IndexOf("Nitro Downloader\\");
        var path = fullPath[..(endIndex + "Nitro Downloader\\".Length)];
        path += "Tools\\yt-dlp.exe";

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("yt-dlp.exe not found at the specified path.");
        }

        var downloadLocation = Settings.GetDownloadLocation();
        var OutputFileNameTemplate = "%(title)s.%(ext)s";

        var arguments = $"{link} -P {downloadLocation} -o {OutputFileNameTemplate}";

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
                Debug.WriteLine(e.Data);
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
