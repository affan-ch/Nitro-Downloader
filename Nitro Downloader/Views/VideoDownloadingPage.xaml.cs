using Microsoft.UI.Xaml.Controls;

using Nitro_Downloader.ViewModels;

namespace Nitro_Downloader.Views;

public sealed partial class VideoDownloadingPage : Page
{
    public VideoDownloadingViewModel ViewModel
    {
        get;
    }

    public VideoDownloadingPage()
    {
        ViewModel = App.GetService<VideoDownloadingViewModel>();
        InitializeComponent();
    }
}
