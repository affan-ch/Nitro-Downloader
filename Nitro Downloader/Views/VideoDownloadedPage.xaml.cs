using Microsoft.UI.Xaml.Controls;

using Nitro_Downloader.ViewModels;

namespace Nitro_Downloader.Views;

public sealed partial class VideoDownloadedPage : Page
{
    public VideoDownloadedViewModel ViewModel
    {
        get;
    }

    public VideoDownloadedPage()
    {
        ViewModel = App.GetService<VideoDownloadedViewModel>();
        InitializeComponent();
    }
}
