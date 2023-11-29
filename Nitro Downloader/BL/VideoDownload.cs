
namespace Nitro_Downloader.BL;

public class VideoDownload
{
    public string? Id
    {
        get; set;
    }

    public string? FileName
    {
        get; set;
    }

    public string? Status
    {
        get; set;
    }

    public string? Size
    {
        get; set;
    }

    public string? Duration
    {
        get; set;
    }

    public string? Resolution
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

    public string? VideoURL
    {
        get; set;
    }

    public string? ChannelURL
    {
        get; set;
    }

    public string? ThumbnailURL
    {
        get; set;
    }

    public string? VideoDescription
    {
        get; set;
    }

    public string? DateAdded
    {
        get; set;
    }

    public string? DateModified
    {
        get; set;
    }

    public VideoDownload(string Id, string FileName, string Status, string Size, string Duration, string Resolution, string Downloaded, string TimeLeft, string TransferRate, string VideoURL, string ChannelURL, string ThumbnailURL, string VideoDescription, string DateAdded, string DateModified)
    {
        this.Id = Id;
        this.FileName = FileName;
        this.Status = Status;
        this.Size = Size;
        this.Duration = Duration;
        this.Resolution = Resolution;
        this.Downloaded = Downloaded;
        this.TimeLeft = TimeLeft;
        this.TransferRate = TransferRate;
        this.VideoURL = VideoURL;
        this.ChannelURL = ChannelURL;
        this.ThumbnailURL = ThumbnailURL;
        this.VideoDescription = VideoDescription;
        this.DateAdded = DateAdded;
        this.DateModified = DateModified;
    }

}

