﻿#pragma checksum "D:\Code\UET\Semester 5\SE Lab\Project\Nitro Downloader\DownloadWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E09F553342EE3BD3CF763F95231F75782612D0831DE30E144E7F2563BBB63D33"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nitro_Downloader
{
    partial class DownloadWindow : 
        global::WinUIEx.WindowEx, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2310")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // DownloadWindow.xaml line 16
                {
                    this.AppTitleBar = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 3: // DownloadWindow.xaml line 75
                {
                    this.ProgressRingStackPanel = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 4: // DownloadWindow.xaml line 89
                {
                    this.BodyContainer = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            case 5: // DownloadWindow.xaml line 362
                {
                    this.StartDownload_Button = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.StartDownload_Button).Click += this.StartDownload_Button_Click;
                }
                break;
            case 6: // DownloadWindow.xaml line 324
                {
                    this.Location_TextBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 7: // DownloadWindow.xaml line 335
                {
                    this.ChangeLocationButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.ChangeLocationButton).Click += this.ChangeLocationButton_ClickAsync;
                }
                break;
            case 8: // DownloadWindow.xaml line 291
                {
                    this.URL_TextBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 9: // DownloadWindow.xaml line 302
                {
                    this.OpenURL_Button = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.OpenURL_Button).Click += this.OpenURL_Button_Click;
                }
                break;
            case 10: // DownloadWindow.xaml line 97
                {
                    this.Expander = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Expander>(target);
                }
                break;
            case 11: // DownloadWindow.xaml line 109
                {
                    this.TitleTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 12: // DownloadWindow.xaml line 113
                {
                    this.ChannelNameTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 13: // DownloadWindow.xaml line 232
                {
                    this.DateUploadedTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 14: // DownloadWindow.xaml line 221
                {
                    this.FileSizeTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 15: // DownloadWindow.xaml line 210
                {
                    this.ExtensionTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 16: // DownloadWindow.xaml line 199
                {
                    this.WebsiteTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 17: // DownloadWindow.xaml line 177
                {
                    this.ViewsTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 18: // DownloadWindow.xaml line 166
                {
                    this.CommentsTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 19: // DownloadWindow.xaml line 155
                {
                    this.LikesTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 20: // DownloadWindow.xaml line 145
                {
                    this.ResolutionTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 21: // DownloadWindow.xaml line 135
                {
                    this.DurationTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 22: // DownloadWindow.xaml line 103
                {
                    this.ThumbnailImage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Image>(target);
                }
                break;
            case 23: // DownloadWindow.xaml line 27
                {
                    this.titleBar = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            case 24: // DownloadWindow.xaml line 49
                {
                    this.MinimizeToTrayButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.MinimizeToTrayButton).PointerEntered += this.MinimizeToTrayButton_PointerEntered;
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.MinimizeToTrayButton).PointerExited += this.MinimizeToTrayButton_PointerExited;
                }
                break;
            case 25: // DownloadWindow.xaml line 40
                {
                    this.AppTitleBarText = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2310")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

