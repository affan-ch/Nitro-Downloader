# Nitro Downloader

Nitro Downloader is a modern Universal Windows Platform (UWP) application built using C# and XAML with WINUI3 for the UI components. It provides a powerful download manager with the ability to download videos, video thumbnails and playlists, using [yt-dlp](https://github.com/yt-dlp/yt-dlp) – an enhanced version of the popular youtube-dl utility. It supports downloading torrents and every other file types from HTTP/HTTPS links, leveraging the robust capabilities of [aria2c](https://github.com/aria2/aria2).

Nitro Downloader is designed to offer an experience similar to popular download managers like Internet Download Manager (IDM), making it easy for users to access and manage their downloads with a feature-rich, modern UWP application.


## Features

- **Download Manager**: Easily download files, videos, video thumbnails, playlists, and more by pasting HTTP/HTTPS links.
- **Torrent Downloads**: Download torrents using the aria2c backend.
- **Resume and Pause Downloads**: Pause and resume downloads at any time.
- **Download Speed Control**: Adjust the download speed to optimize your network usage.
- **System Tray Integration**: Provides a system tray icon for easy access and management.
- **Customizable Themes**: Choose from a variety of themes to personalize your experience.
- **Automatic Updates**: Nitro Downloader can check for and apply updates automatically.

## System Requirements

- Windows 10 or later
- .NET 5.0 or later

## Installation

You can download and install Nitro Downloader from the Microsoft Store (coming soon) or by building the application from the source code.

### Building from Source

1. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/affan-ch/Nitro-Downloader.git
   ```

2. Open the solution in Visual Studio (make sure you have the necessary tools installed for UWP and WinUI3 development).

3. Build the solution.

4. Run the application on your local machine.

## Usage

1. Launch Nitro Downloader on your Windows machine.

2. Paste the HTTP/HTTPS link of the file, video, or playlist you want to download.

3. Adjust download settings, such as download location, speed, and more.

4. Click the "Start Download" button to initiate the download.

5. Monitor the progress of your downloads in the application window.

6. Use the system tray icon to access additional features, pause, resume, or cancel downloads.

## Contributing

Contributions to Nitro Downloader are welcome. You can help by reporting issues, suggesting features, or submitting pull requests. Please follow our [contribution guidelines](CONTRIBUTING.md) for more details.

## License

Nitro Downloader is licensed under the [Mozilla Public License Version 2.0](LICENSE).

## Disclaimer

This application is for personal use only. Please ensure that you have the necessary permissions and adhere to copyright and content distribution laws when using Nitro Downloader to download content from the internet.

