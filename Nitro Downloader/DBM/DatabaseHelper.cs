using System.Data.SQLite;
using System.Diagnostics;

namespace Nitro_Downloader.DBM;

internal class DatabaseHelper
{
    private static readonly string DatabaseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Nitro Downloader");

    private static readonly string DatabaseFilePath = Path.Combine(DatabaseFolderPath, "NitroDownloader.db");

    private static readonly string ConnectionString = $"Data Source={DatabaseFilePath};Version=3;";

    private static readonly string CreateVideoDownloadsTableQuery = @"CREATE TABLE IF NOT EXISTS [VideoDownloads] (Id INTEGER PRIMARY KEY AUTOINCREMENT, FileName TEXT, Status TEXT, Size TEXT, Duration TEXT, Resolution TEXT, Downloaded TEXT, TimeLeft TEXT, TransferRate TEXT, VideoURL TEXT, ChannelURL TEXT, ThumbnailURL TEXT, VideoDescription TEXT)";

    //private static readonly List<VideoDownload> VideoDownloadsList = new();
    private static readonly ObservableList<VideoDownload> VideoDownloadsList = new();

    

    public static void CreateDatabase()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        using var command = new SQLiteCommand(CreateVideoDownloadsTableQuery, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }


    public static void LoadVideoDownloadsIntoList()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        using var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM VideoDownloads";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            VideoDownloadsList.Add(new VideoDownload
            {
                Id = reader.IsDBNull(0) ? "" : reader.GetInt32(0).ToString(),
                FileName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                Status = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Size = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Duration = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Resolution = reader.IsDBNull(5) ? "" : reader.GetString(5),
                Downloaded = reader.IsDBNull(6) ? "" : reader.GetString(6),
                TimeLeft = reader.IsDBNull(7) ? "" : reader.GetString(7),
                TransferRate = reader.IsDBNull(8) ? "" : reader.GetString(8),
                VideoURL = reader.IsDBNull(9) ? "" : reader.GetString(9),
                ChannelURL = reader.IsDBNull(10) ? "" : reader.GetString(10),
                ThumbnailURL = reader.IsDBNull(11) ? "" : reader.GetString(11),
                VideoDescription = reader.IsDBNull(12) ? "" : reader.GetString(12)
            });
        }
        connection.Close();
    }
  


    public static void InsertVideoDownloadIntoList(VideoDownload videoDownload)
    {
        VideoDownloadsList.Add(videoDownload);
    }

    public static ObservableList<VideoDownload> GetVideoDownloads()
    {
        return VideoDownloadsList;
    }


    public static void StoreListInDatabase()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        using var command = new SQLiteCommand(connection);
        command.CommandText = "DELETE FROM VideoDownloads";
        command.ExecuteNonQuery();
        foreach (var videoDownload in VideoDownloadsList)
        {
            command.CommandText = "INSERT INTO VideoDownloads (FileName, Status, Size, Duration, Resolution, Downloaded, TimeLeft, TransferRate, VideoURL, ChannelURL, ThumbnailURL, VideoDescription) VALUES (@FileName, @Status, @Size, @Duration, @Resolution, @Downloaded, @TimeLeft, @TransferRate, @VideoURL, @ChannelURL, @ThumbnailURL, @VideoDescription)";
            command.Parameters.AddWithValue("@FileName", videoDownload.FileName);
            command.Parameters.AddWithValue("@Status", videoDownload.Status);
            command.Parameters.AddWithValue("@Size", videoDownload.Size);
            command.Parameters.AddWithValue("@Duration", videoDownload.Duration);
            command.Parameters.AddWithValue("@Resolution", videoDownload.Resolution);
            command.Parameters.AddWithValue("@Downloaded", videoDownload.Downloaded);
            command.Parameters.AddWithValue("@TimeLeft", videoDownload.TimeLeft);
            command.Parameters.AddWithValue("@TransferRate", videoDownload.TransferRate);
            command.Parameters.AddWithValue("@VideoURL", videoDownload.VideoURL);
            command.Parameters.AddWithValue("@ChannelURL", videoDownload.ChannelURL);
            command.Parameters.AddWithValue("@ThumbnailURL", videoDownload.ThumbnailURL);
            command.Parameters.AddWithValue("@VideoDescription", videoDownload.VideoDescription);
            command.ExecuteNonQuery();
        }
        connection.Close();
    }   



    public static void UpdateVideoDownloadInList(VideoDownload videoDownload)
    {
        var index = VideoDownloadsList.FindIndex(v => v.Id == videoDownload.Id);
        if (index != -1)
        {
            VideoDownloadsList.Update(index, videoDownload);
        }
        
    }
}

public class ObservableList<VideoDownload> : List<VideoDownload>
{
    public event EventHandler? OnChanged;

    public new void Add(VideoDownload item)
    {
        base.Add(item);
        OnListChanged();
    }

    public new void Remove(VideoDownload item)
    {
        base.Remove(item);
        OnListChanged();
    }

    public new void Insert(int index, VideoDownload item)
    {
        base.Insert(index, item);
        OnListChanged();
    }

    public void Update(int index, VideoDownload item)
    {
        base[index] = item;
        OnListChanged();
    }


    public virtual void OnListChanged()
    {
        OnChanged?.Invoke(this, EventArgs.Empty);
        Debug.WriteLine("List changed");
    }
}



public class VideoDownload
{
    public string? Id
    {
        get; set;
    }

    public required string FileName
    {
        get; set;
    }

    public required string Status
    {
        get; set; 
    }

    public required string Size
    {
        get; set; 
    }

    public required string Duration
    {
        get; set; 
    }

    public required string Resolution
    {
        get; set; 
    }

    public string? Downloaded
    {
        get; set;
    }

    public string? TimeLeft
    {
        get; set; 
    }

    public string? TransferRate
    {
        get; set; 
    }

    public required string VideoURL
    {
        get; set; 
    }

    public string? ChannelURL
    {
        get; set; 
    }

    public required string ThumbnailURL
    {
        get; set; 
    }

    public string? VideoDescription
    {
        get; set; 
    }
}

