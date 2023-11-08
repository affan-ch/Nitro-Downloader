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

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event

        EfficiencyModeUtilities.SetEfficiencyMode(true);
        //WindowExtensions.Hide(this Window window, enableEfficiencyMode: true) // default value
        //WindowExtensions.Show(this Window window, disableEfficiencyMode: true) // default value
        TrayIcon.ForceCreate(true); // default value

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
    public void ExitApplication()
    {
        App.HandleClosedEvents = false;
        TrayIcon.Dispose();
        App.MainWindow?.Close();
    }

    //private void PrepareContextMenuWindow()
    //{
    //    if (TrayIcon.ContextFlyout == null ||
    //        TrayIcon.ContextMenuMode != ContextMenuMode.SecondWindow)
    //    {
    //        return;
    //    }

    //    var frame = new Frame
    //    {
    //        Background = new SolidColorBrush(Colors.Transparent),
    //    };
    //    var window = new Window()
    //    {
    //        Content = frame,
    //    };

    //    TrayIcon.ActualThemeChanged += (_, _) => frame.RequestedTheme = ActualTheme;

    //    var handle = WindowNative.GetWindowHandle(window);
    //    WindowUtilities.MakeTransparent(handle);

    //    var id = Win32Interop.GetWindowIdFromWindow(handle);
    //    var appWindow = AppWindow.GetFromWindowId(id);
    //    appWindow.IsShownInSwitchers = false;

    //    var presenter = (OverlappedPresenter)appWindow.Presenter;
    //    presenter.IsMaximizable = false;
    //    presenter.IsMinimizable = false;
    //    presenter.IsResizable = false;
    //    presenter.IsAlwaysOnTop = true;
    //    presenter.SetBorderAndTitleBar(false, false);

    //    var flyout = new MenuFlyout
    //    {
    //        AreOpenCloseAnimationsEnabled = TrayIcon.ContextFlyout.AreOpenCloseAnimationsEnabled,
    //        Placement = FlyoutPlacementMode.Full,
    //    };
    //    flyout.Closed += async (_, _) =>
    //    {
    //        if (!flyout.AreOpenCloseAnimationsEnabled ||
    //            !IsContextMenuVisible)
    //        {
    //            _ = WindowUtilities.HideWindow(handle);
    //            return;
    //        }

    //        await Task.Delay(1).ConfigureAwait(true);
    //        flyout.ShowAt(window.Content, new FlyoutShowOptions
    //        {
    //            ShowMode = FlyoutShowMode.Transient,
    //        });
    //    };
    //    foreach (var flyoutItemBase in ((MenuFlyout)TrayIcon.ContextFlyout).Items)
    //    {
    //        flyout.Items.Add(flyoutItemBase);
    //        flyoutItemBase.Tapped += (_, _) =>
    //        {
    //            IsContextMenuVisible = false;
    //            flyout.Hide();
    //            _ = WindowUtilities.HideWindow(handle);
    //        };
    //        flyoutItemBase.PointerMoved += (_, _) => PointerActionInContextMenuWindow = true;
    //        flyoutItemBase.PointerPressed += (_, _) => PointerActionInContextMenuWindow = true;
    //    }

    //    frame.Loaded += (_, _) =>
    //    {
    //        flyout.ShowAt(window.Content, new FlyoutShowOptions
    //        {
    //            ShowMode = FlyoutShowMode.Transient,
    //        });
    //        flyout.Hide();
    //    };
    //    window.Activated += (sender, args) =>
    //    {
    //        if (args.WindowActivationState == WindowActivationState.Deactivated)
    //        {
    //            IsContextMenuVisible = false;
    //            flyout.Hide();
    //            _ = WindowUtilities.HideWindow(handle);
    //            return;
    //        }

    //        if (ContextMenuWindow == null)
    //        {
    //            return;
    //        }

    //        IsContextMenuVisible = true;
    //        flyout.ShowAt(window.Content, new FlyoutShowOptions
    //        {
    //            ShowMode = FlyoutShowMode.Transient,
    //        });
    //    };

    //    ContextMenuWindow = window;
    //    ContextMenuWindowHandle = handle;
    //    ContextMenuAppWindow = appWindow;
    //    ContextMenuFlyout = flyout;
    //}




    // this handles updating the caption button colors correctly when indows system theme is changed
    // while the app is open
    
    
    
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }
}
