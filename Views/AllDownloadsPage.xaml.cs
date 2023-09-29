using Microsoft.UI.Xaml.Controls;

using Nitro_Downloader.ViewModels;

namespace Nitro_Downloader.Views;

public sealed partial class AllDownloadsPage : Page
{
    public AllDownloadsViewModel ViewModel
    {
        get;
    }

    public AllDownloadsPage()
    {
        ViewModel = App.GetService<AllDownloadsViewModel>();
        InitializeComponent();
    }
}
