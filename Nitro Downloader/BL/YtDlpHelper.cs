using System.Diagnostics;
using System.Text;
using Nitro_Downloader.DBM;

namespace Nitro_Downloader.BL;


public static class YtDlpHelper
{

    public static async Task<string?> DownloadVideoAsync(string link)
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
            return null;
        }
    }

    public static async Task<string?> GetVideoInfoJsonAsync(string link)
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

        try 
        { 
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
                try
                {
                    var jsonOutput = outputBuilder.ToString();
                    return jsonOutput;
                }
                catch (Exception)
                {
                    return null;
                }
            
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }


}
