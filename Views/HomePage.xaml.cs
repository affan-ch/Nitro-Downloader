using System.Diagnostics;
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

    private void DownloadButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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
            process.Start();

            Debug.WriteLine("Download Started");

            var output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();


            Debug.WriteLine(output);

        }
    }
}
