﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Nitro_Downloader.Contracts.Services;
using Nitro_Downloader.DL;
using Nitro_Downloader.Helpers;
using Nitro_Downloader.ViewModels;

using Windows.System;

namespace Nitro_Downloader.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();

        RefreshNumbers(null, null);

        var timer = new DispatcherTimer();
        timer.Tick += RefreshNumbers;
        timer.Interval = TimeSpan.FromSeconds(2);
        timer.Start();
    }

    private void RefreshNumbers(object? sender, object? e)
    {
        var list = VideoDownloadDL.GetVideoDownloadsList();
        var downloadingListCount = list.Where(item => item.Status == "Downloading").Count();
        var downloadedListCount = list.Where(item => item.Status == "Downloaded").Count();

        if (downloadingListCount > 0)
        {
            downloadingCount_InfoBadge.Value = downloadingListCount;
            downloadingCount_InfoBadge.Visibility = Visibility.Visible;
        }
        else
        {
            downloadingCount_InfoBadge.Visibility = Visibility.Collapsed;
            downloadingCount_InfoBadge.Value = 0;
        }

        if (downloadedListCount > 0)
        {
            downloadedCount_InfoBadge.Value = downloadedListCount;
            downloadedCount_InfoBadge.Visibility = Visibility.Visible;
        }
        else
        {
            downloadedCount_InfoBadge.Visibility = Visibility.Collapsed;
            downloadedCount_InfoBadge.Value = 0;
        }
    }

    private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush)App.Current.Resources[resource];
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }
}
