
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using Nitro_Downloader.BL;

namespace Nitro_Downloader.DL;

public class VideoDownloadDL
{
    private static readonly ObservableList<VideoDownload> VideoDownloadsList = new();


    // Load VideoDownloads from database into list
    public static void LoadVideoDownloadsIntoList(string ConnectionString)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        using var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM VideoDownloads";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            VideoDownloadsList.Add(new VideoDownload
            (
                Id: reader.IsDBNull(0) ? "" : reader.GetInt32(0).ToString(),
                FileName: reader.IsDBNull(1) ? "" : reader.GetString(1),
                Status: reader.IsDBNull(2) ? "" : reader.GetString(2),
                Size: reader.IsDBNull(3) ? "" : reader.GetString(3),
                Duration: reader.IsDBNull(4) ? "" : reader.GetString(4),
                Resolution: reader.IsDBNull(5) ? "" : reader.GetString(5),
                Downloaded: reader.IsDBNull(6) ? "" : reader.GetString(6),
                TimeLeft: reader.IsDBNull(7) ? "" : reader.GetString(7),
                TransferRate: reader.IsDBNull(8) ? "" : reader.GetString(8),
                VideoURL: reader.IsDBNull(9) ? "" : reader.GetString(9),
                ChannelURL: reader.IsDBNull(10) ? "" : reader.GetString(10),
                ThumbnailURL: reader.IsDBNull(11) ? "" : reader.GetString(11),
                VideoDescription: reader.IsDBNull(12) ? "" : reader.GetString(12),
                DateAdded: reader.IsDBNull(13) ? "" : reader.GetString(13),
                DateModified: reader.IsDBNull(14) ? "" : reader.GetString(14)
            ));
        }
        connection.Close();
    }


    // Get VideoDownloads list
    public static ObservableList<VideoDownload> GetVideoDownloadsList()
    {
        return VideoDownloadsList;
    }


    // Insert VideoDownload into list
    public static void InsertVideoDownloadIntoList(VideoDownload videoDownload)
    {
        VideoDownloadsList.Add(videoDownload);
    }


    // Update VideoDownload in list
    public static void UpdateVideoDownloadInList(VideoDownload videoDownload)
    {
        var index = VideoDownloadsList.IndexOf(videoDownload);
        if (index != -1)
        {
            VideoDownloadsList.RemoveAt(index);
            VideoDownloadsList.Insert(index, videoDownload);
        }

    }

}



public class ObservableList<VideoDownload> : ObservableCollection<VideoDownload>
{
    private readonly SynchronizationContext _syncContext = SynchronizationContext.Current!;

    protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (_syncContext != null)
        {
            _syncContext.Post(_ => base.OnCollectionChanged(e), null);
        }
        else
        {
            base.OnCollectionChanged(e);
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (_syncContext != null)
        {
            _syncContext.Post(_ => base.OnPropertyChanged(e), null);
        }
        else
        {
            base.OnPropertyChanged(e);
        }
    }
}