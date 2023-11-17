using Microsoft.UI.Xaml.Controls;

using Nitro_Downloader.ViewModels;

namespace Nitro_Downloader.Views;

public sealed partial class VideoQueuedPage : Page
{
    public VideoQueuedViewModel ViewModel
    {
        get;
    }

    public VideoQueuedPage()
    {
        ViewModel = App.GetService<VideoQueuedViewModel>();
        InitializeComponent();
    }
}
