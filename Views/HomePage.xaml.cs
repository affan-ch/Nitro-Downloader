using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Microsoft.UI.Xaml.Controls;

using Nitro_Downloader.ViewModels;


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
    }

    private async void DownloadButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var link = Link_TextBox.Text;

        //var command = "../Tools/aria2c.exe";

        //var toolsFolder = "Tools"; // Folder containing yt-dlp.exe
        //var ytDlpPath = Path.Combine(Environment.CurrentDirectory, toolsFolder, "yt-dlp.exe");

        //Debug.WriteLine(ytDlpPath);

        Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

        var fullPath = AppDomain.CurrentDomain.BaseDirectory.ToString();

        var endIndex = fullPath.IndexOf("Nitro Downloader\\");

        var path = fullPath.Substring(0, endIndex + "Nitro Downloader\\".Length);

        Debug.WriteLine(path);

        path += "Tools\\yt-dlp.exe";

        Debug.WriteLine(path);


        // Check if yt-dlp.exe exists at the specified path
        if (!File.Exists(path))
        {
            Debug.WriteLine("yt-dlp.exe not found at: " + path);
            return;
        }

        var arguments = "https://youtu.be/Mh-OtwRcTcw?si=0wfjyHGOR1J1oaFO -P \"C:\\Users\\affan\\Pictures\"";

        if (link != null)
        {


            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = false;
            //process.OutputDataReceived += Process_OutputDataReceived;

            StringBuilder outputBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Debug.WriteLine("Output: " + e.Data);

                    outputBuilder.AppendLine(e.Data);
                }
            };

            process.Start();

            process.BeginOutputReadLine();

            Debug.WriteLine("Download Started");

            await Task.Run(() =>
            {
                process.WaitForExit();
            });

            Debug.WriteLine("Download Completed");

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

}
