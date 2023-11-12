using System.Diagnostics;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Nitro_Downloader.Activation;
using Nitro_Downloader.Contracts.Services;
using Nitro_Downloader.Core.Contracts.Services;
using Nitro_Downloader.Core.Services;
using Nitro_Downloader.Helpers;
using Nitro_Downloader.Models;
using Nitro_Downloader.Notifications;
using Nitro_Downloader.Services;
using Nitro_Downloader.ViewModels;
using Nitro_Downloader.Views;

namespace Nitro_Downloader;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public static bool HandleClosedEvents { get; set; } = true;


    public App()
    {
        InitializeComponent();




        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<AllDownloadsViewModel>();
            services.AddTransient<AllDownloadsPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        AppActivationArguments arguments = AppInstance.GetCurrent().GetActivatedEventArgs();
        ExtendedActivationKind kind = arguments.Kind;

        if (arguments.Kind == ExtendedActivationKind.Protocol)
        {
            var protocolArgs = arguments.Data as Windows.ApplicationModel.Activation.ProtocolActivatedEventArgs;
            var uri = protocolArgs!.Uri;

            Debug.WriteLine("App.xaml.cs: uri");
            Debug.WriteLine(uri);

            if (uri.Scheme == "nitro-downloader")
            {
                if(uri.Host == "video")
                {
                    var query = HttpUtility.ParseQueryString(uri.Query);

                    var url = query["url"] ?? "";

                    var window = new DownloadWindow(url, uri.Host);

                    window.Activate();

                }
            }

            return;
        }

        base.OnLaunched(args);

        MainWindow.Closed += (sender, args) =>
        {
            if (HandleClosedEvents)
            {
                args.Handled = true;
                MainWindow.Hide();
            }
        };




        //App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);

        Debug.WriteLine(args);
        Debug.WriteLine(args.Arguments);
        Debug.WriteLine($"Launched with the arguments: {string.Join(", ", args.Arguments)}");
        
    }


}
