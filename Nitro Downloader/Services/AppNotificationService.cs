using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;

using Microsoft.Windows.AppNotifications;

using Nitro_Downloader.Contracts.Services;
using Nitro_Downloader.ViewModels;
using Windows.System;


namespace Nitro_Downloader.Notifications;

public class AppNotificationService : IAppNotificationService
{
    private readonly INavigationService _navigationService;

    public AppNotificationService(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    ~AppNotificationService()
    {
        Unregister();
    }

    public void Initialize()
    {
        AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;

        AppNotificationManager.Default.Register();
    }

    public async void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        // TODO: Handle notification invocations when your app is already running.

        Debug.WriteLine($"Notification invoked: {args.Argument}");

        if (ParseArguments(args.Argument)["action"] == "openFile")
        {
            //args.Argument.ToString().StartsWith("action=openFile&filePath=")

            var filePath = ParseArguments(args.Argument)["filePath"];
             
            var fileUri = new Uri("file:///" + filePath!.Replace("\\", "/"));

            var options = new LauncherOptions
            {
                DisplayApplicationPicker = false // Set to true to show the app picker
            };

            var success = await Launcher.LaunchUriAsync(fileUri, options);

            Debug.WriteLine(success);

        }

        if (ParseArguments(args.Argument)["action"] == "showInFolder")
        {
            var filePath = ParseArguments(args.Argument)["filePath"];

            var explorerPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\explorer.exe";
            var arguments = $"/select,\"{filePath}\"";

            Process.Start(explorerPath, arguments);

        }

        //// // Navigate to a specific page based on the notification arguments.
        //// if (ParseArguments(args.Argument)["action"] == "Settings")
        //// {
        ////    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        ////    {
        ////        _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        ////    });
        //// }

        //App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        //{
        //    App.MainWindow.ShowMessageDialogAsync("TODO: Handle notification invocations when your app is already running.", "Notification Invoked");

        //    App.MainWindow.BringToFront();
        //});
    }

    public bool Show(string payload)
    {
        var appNotification = new AppNotification(payload);

        AppNotificationManager.Default.Show(appNotification);

        return appNotification.Id != 0;
    }

    public NameValueCollection ParseArguments(string arguments)
    {
        return HttpUtility.ParseQueryString(arguments);
    }

    public void Unregister()
    {
        AppNotificationManager.Default.Unregister();
    }
}
