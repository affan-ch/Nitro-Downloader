using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Text.Json;
using Nitro_Downloader.ViewModels;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Globalization;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Nitro_Downloader.DBM;
using WinRT.Interop;
using Nitro_Downloader.BL;
using System.Text;
using System.Text.RegularExpressions;
using Nitro_Downloader.DL;

namespace Nitro_Downloader.Views;

public sealed partial class AddVideoDownloadPage : Page
{
    public AddVideoDownloadViewModel ViewModel
    {
        get;
    }

    private VideoInfo? VideoInfo
    {
        get; set; 
    }


    public AddVideoDownloadPage()
    {
        ViewModel = App.GetService<AddVideoDownloadViewModel>();
        InitializeComponent();
        LocationTextBox.Text = Settings.GetDownloadLocation();
        FooterContainer.Visibility = Visibility.Collapsed;
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
            
            if(jsonOutput == null)
            {
                HelperFunctions.ShowToastNotification("Error while processing link!", "Please check the download link and try again.");

                GetInfoButton.Content = "Get Info";
                ProgressRingStackPanel.Visibility = Visibility.Collapsed;
                Link_TextBox.Text = "";

                return;
            }
            VideoInfo = JsonSerializer.Deserialize<VideoInfo>(jsonOutput);
            Debug.WriteLine(VideoInfo?.title);

            FooterContainer.Visibility = Visibility.Visible;

            TitleTextBlock.Text = VideoInfo?.title;

            if (VideoInfo?.thumbnail != null)
            {
                if (VideoInfo?.thumbnail.Length > 10)
                {
                    Microsoft.UI.Xaml.Media.ImageSource imageSource = new BitmapImage(new Uri(VideoInfo?.thumbnail ?? "https://www.youtube.com/"));

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


            DurationTextBlock.Text = VideoInfo?.duration_string;

            var likes = HelperFunctions.FormatYouTubeCounts((long)(VideoInfo?.like_count ?? 0));

            LikesTextBlock.Text = likes;

            var comments = HelperFunctions.FormatYouTubeCounts((long)(VideoInfo?.comment_count ?? 0));

            CommentsTextBlock.Text = comments;


            var views = HelperFunctions.FormatYouTubeCounts((long)(VideoInfo?.view_count ?? 0));
            ViewsTextBlock.Text = views;

            DateTime.TryParseExact(VideoInfo?.upload_date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate);

            var formattedDate = parsedDate.ToString("MMM dd, yyyy");

            ResolutionTextBlock.Text = VideoInfo?.resolution;

            DateUploadedTextBlock.Text = formattedDate;

            ChannelNameTextBlock.Text = VideoInfo?.channel;

            WebsiteTextBlock.Text = VideoInfo?.extractor_key;

            var extension = VideoInfo?.ext ?? "?";

            ExtensionTextBlock.Text = extension.ToUpper();

            Expander.Width = Link_TextBox.ActualWidth;

            ProgressRingStackPanel.Visibility = Visibility.Collapsed;



            GetInfoButton.Content = "Get Info";

            if(VideoInfo?.webpage_url != null && VideoInfo?.webpage_url != string.Empty)
            {
                VideoLink.Visibility = Visibility.Visible;
                VideoLinkTextBlock.Visibility = Visibility.Visible;

                VideoLink.NavigateUri = new Uri(VideoInfo?.webpage_url!);
                //VideoLink.Content = videoInfo?.webpage_url!;
            }
            else
            {
                VideoLink.Visibility = Visibility.Collapsed;
                VideoLinkTextBlock.Visibility = Visibility.Collapsed;
            }


            
            if(VideoInfo?.channel_url != null && VideoInfo?.channel_url != string.Empty)
            {
                ChannelLink.Visibility = Visibility.Visible;
                ChannelLinkTextBlock.Visibility = Visibility.Visible;

                ChannelLink.NavigateUri = new Uri(VideoInfo?.channel_url!);
                //ChannelLink.Content = videoInfo?.channel_url!;
            }
            else
            {
                ChannelLink.Visibility = Visibility.Collapsed;
                ChannelLinkTextBlock.Visibility = Visibility.Collapsed;
            }


            if(VideoInfo?.thumbnail != null && VideoInfo?.thumbnail != string.Empty)
            {
                ThumbnailLink.Visibility = Visibility.Visible;
                ThumbnailTextBlock.Visibility = Visibility.Visible;

                ThumbnailLink.NavigateUri = new Uri(VideoInfo?.thumbnail!);
                //ThumbnailLink.Content = videoInfo?.thumbnail!;
            }
            else
            {
                ThumbnailLink.Visibility = Visibility.Collapsed;
                ThumbnailTextBlock.Visibility = Visibility.Collapsed;
            }
            




            FileSizeTextBlock.Text = HelperFunctions.FormatFileSize(VideoInfo?.filesize_approx ?? 0);


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

        var list = VideoDownloadDL.GetVideoDownloadsList();
        var maxId = list.Max(x => x.Id);
        if (maxId == null || maxId == string.Empty)
        {
           maxId = "0";
        }
        var newMaxId = (int.Parse(maxId) + 1).ToString();
        Debug.WriteLine("Max Id:" + newMaxId);

        var videoDownload = new VideoDownload(
            Id: newMaxId,
            FileName: TitleTextBlock.Text,
            Status: "Downloading",
            VideoURL: link,
            ThumbnailURL: ThumbnailLink.NavigateUri.ToString() ?? "",
            ChannelURL: ChannelLink.NavigateUri.ToString() ?? "",
            Size: FileSizeTextBlock.Text,
            Resolution: ResolutionTextBlock.Text,
            Duration: DurationTextBlock.Text,
            Downloaded: "Starting...",
            TimeLeft: "Calculating...",
            TransferRate: "Calculating...",
            VideoDescription: VideoInfo?.description ?? "",
            DateAdded: DateTime.Now.ToString(),
            DateModified: DateTime.Now.ToString()
        );


        Link_TextBox.Text = string.Empty;
        Expander.Visibility = Visibility.Collapsed;

        VideoDownloadDL.InsertVideoDownloadIntoList(videoDownload);

        //var output = await YtDlpHelper.DownloadVideoAsync(link);

        var fullPath = AppDomain.CurrentDomain.BaseDirectory.ToString();

        //var endIndex = fullPath.IndexOf("Nitro Downloader\\");
        //var path = fullPath[..(endIndex + "Nitro Downloader\\".Length)];
        //path += "Tools\\yt-dlp.exe";
        var path = "C:\\tools\\yt-dlp.exe";

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

        // Show Notification
        HelperFunctions.ShowToastNotification("Download Added!", "Your download has been started. Check downloading page for progress.");


        var outputBuilder = new StringBuilder();

        process.OutputDataReceived +=  (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                outputBuilder.AppendLine(e.Data);

                var pattern = @"\[download\]\s.*\sof\s.*\sin\s.*\sat\s.*";
                var pattern2 = @"\[download\]\s.*\sof\s.*\sat\s.*\sETA\s.*";

                if (Regex.IsMatch(e.Data, pattern2))
                {
                    var startIndex = e.Data.IndexOf("[download]") + "[download]".Length;
                    var endIndex = e.Data.IndexOf("%", startIndex);
                    if (startIndex >= 0 && endIndex >= 0)
                    {

                       
                        var percentageString = e.Data.Substring(startIndex, endIndex - startIndex).Trim();
                        if (float.TryParse(percentageString, out var percentage))
                        {
                            Debug.WriteLine("Download Percentage: " + percentage + "%");
                            videoDownload.Downloaded = percentage.ToString() + "%";

                            // Extracting transfer rate
                            var rateIndex = e.Data.LastIndexOf("at") + "at".Length;
                            if (rateIndex >= 0)
                            {
                                var rateString = e.Data[rateIndex..].Trim();
                                var rate = rateString.Split(" ")[0];
                                Debug.WriteLine("Transfer Rate: " + rate);
                                videoDownload.TransferRate = rate;
                            }

                            // Extracting time left
                            var timeIndex = e.Data.LastIndexOf("ETA") + "ETA".Length;
                            var timeLeftString = e.Data[timeIndex..].Trim();
                            Debug.WriteLine("Time Left: " + timeLeftString);

                            videoDownload.TimeLeft = timeLeftString;
                            
                            VideoDownloadDL.UpdateVideoDownloadInList(videoDownload);
                        }
                    }
                }
                else if(Regex.IsMatch(e.Data, pattern))
                {
                    
                }
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
            
        }
        else
        {
            videoDownload.Status = "Error";
            VideoDownloadDL.UpdateVideoDownloadInList(videoDownload);
            return;
        }


        videoDownload.Status = "Downloaded";
        videoDownload.Downloaded = "100%";
        videoDownload.TimeLeft = "--";
        videoDownload.TransferRate = "--";
        VideoDownloadDL.UpdateVideoDownloadInList(videoDownload);

        var title = TitleTextBlock.Text;
        HelperFunctions.ShowDownloadCompleteNotification(title, downloadLocation);

    }

}
