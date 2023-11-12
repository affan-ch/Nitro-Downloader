using H.NotifyIcon.EfficiencyMode;
using H.NotifyIcon;
using Microsoft.UI.Xaml;
using Newtonsoft.Json.Linq;
using Nitro_Downloader.Helpers;

using Windows.UI.ViewManagement;
using H.NotifyIcon.Core;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using WinRT.Interop;
using CommunityToolkit.Mvvm.Input;

namespace Nitro_Downloader;

public sealed partial class MainWindow : WindowEx
{
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private UISettings settings;

    //private bool IsContextMenuVisible;

    public MainWindow()
    {
        InitializeComponent();



        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event

        EfficiencyModeUtilities.SetEfficiencyMode(true);

        TrayIcon.ForceCreate(true); // Show System Tray Icon

        TrayIcon.LeftClickCommand = new RelayCommand(ShowWindow);

        //H.NotifyIcon.WindowExtensions.HideInTaskbar(this);

    }

    [RelayCommand]
    public void ShowHideWindow()
    {
        var window = App.MainWindow;
        if (window == null)
        {
            return;
        }

        if (window.Visible)
        {
            window.Hide();
        }
        else
        {
            window.Show();
        }
    }

    [RelayCommand]
    public void ShowWindow()
    {
        var window = App.MainWindow;
        if (window == null)
        {
            return;
        }

        if (!window.Visible)
        {
            window.Show();
        }
        else
        {
            window.BringToFront();
        }
    }



    [RelayCommand]
    public void ExitApplication()
    {
        App.HandleClosedEvents = false;
        TrayIcon.Dispose();
        App.MainWindow?.Close();
    }

    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }


}
