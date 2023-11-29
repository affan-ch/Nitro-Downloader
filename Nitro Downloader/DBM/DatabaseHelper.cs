using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using Nitro_Downloader.BL;

namespace Nitro_Downloader.DBM;

internal class DatabaseHelper
{
    private static readonly string DatabaseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Nitro Downloader");

    private static readonly string DatabaseFilePath = Path.Combine(DatabaseFolderPath, "NitroDownloader.db");

    public static readonly string ConnectionString = $"Data Source={DatabaseFilePath};Version=3;";

    private static readonly string CreateVideoDownloadsTableQuery = @"CREATE TABLE IF NOT EXISTS [VideoDownloads] (Id INTEGER PRIMARY KEY AUTOINCREMENT, FileName TEXT, Status TEXT, Size TEXT, Duration TEXT, Resolution TEXT, Downloaded TEXT, TimeLeft TEXT, TransferRate TEXT, VideoURL TEXT, ChannelURL TEXT, ThumbnailURL TEXT, VideoDescription TEXT, DateAdded TEXT, DateModified TEXT)";
    

    public static void CreateDatabase()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        using var command = new SQLiteCommand(CreateVideoDownloadsTableQuery, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }
    
}






