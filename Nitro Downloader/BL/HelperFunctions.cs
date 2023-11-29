using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Nitro_Downloader.BL;


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

    public static string FormatFileSize(long sizeInBytes)
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


    public static void ShowToastNotification(string title, string subtitle)
    {
        var builder = new ToastContentBuilder()
                .AddText(title)
                .AddText(subtitle);

        var toast = new ToastNotification(builder.GetToastContent().GetXml());
        ToastNotificationManager.CreateToastNotifier().Show(toast);
    }

}


