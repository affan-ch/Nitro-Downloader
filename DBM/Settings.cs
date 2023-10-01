using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nitro_Downloader.DBM;

internal class Settings
{
    public static readonly string SettingsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Nitro Downloader");
    public static readonly string SettingsFilePath = Path.Combine(SettingsFolderPath, "settings.json");

    public static string? DownloadLocation
    {
        get; set;
    }
    public static int MaxConnections
    {
        get; set;
    }



    public static void CheckOrCreateSettingsFile()
    {
        try
        {
            if (!Directory.Exists(SettingsFolderPath))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(SettingsFolderPath);
                directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden | FileAttributes.System;
                Debug.WriteLine("Settings folder created");
            }

            if (!File.Exists(SettingsFilePath))
            {
                File.Create(SettingsFilePath).Close();
                Debug.WriteLine("Settings file created");
                
                SetDefaultSettings();
                Debug.WriteLine("Default settings set");
            }
            else
            {
                ReadSettings();
                Debug.WriteLine("Settings read");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error creating settings file");
            Debug.WriteLine(ex.Message);
        }
    }

    public static void SetDefaultSettings()
    {
        try
        {
            DownloadLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Nitro Downloader");
            MaxConnections = 1;

            Dictionary<string, string> defaultSettings = new Dictionary<string, string>
            {
                { "DownloadLocation", DownloadLocation.ToString() },
                { "MaxConnections", MaxConnections.ToString() }
            };

            var jsonSettings = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);

            File.WriteAllText(SettingsFilePath, jsonSettings);

            Console.WriteLine("Default Settings Inserted successfully.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error setting default settings");
            Debug.WriteLine(ex.Message);
        }
    }

    public static void ReadSettings()
    {
        try
        {        
            var jsonSettings = File.ReadAllText(SettingsFilePath);
            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings);
            
            DownloadLocation = settings?["DownloadLocation"];
            MaxConnections = Convert.ToInt32(settings?["MaxConnections"]);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error reading settings");
            Debug.WriteLine(ex.Message);
        }
    }

    public static void SetDownloadLocation(string downloadLocation)
    {
        CheckOrCreateSettingsFile();
        try
        {
            DownloadLocation = downloadLocation;
        
            Dictionary<string, string> defaultSettings = new Dictionary<string, string>
            {
                { "DownloadLocation", DownloadLocation.ToString() },
                { "MaxConnections", MaxConnections.ToString() }
            };
        
            var jsonSettings = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
        
            File.WriteAllText(SettingsFilePath, jsonSettings);
        
            Debug.WriteLine("Download Location updated successfully.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error updating download location");
            Debug.WriteLine(ex.Message);
        }
    }

    public static string GetDownloadLocation()
    {
        CheckOrCreateSettingsFile();
        try
        {
            var jsonSettings = File.ReadAllText(SettingsFilePath);
            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings);
               
            DownloadLocation = settings?["DownloadLocation"];
            return DownloadLocation ?? string.Empty;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error getting download location");
            Debug.WriteLine(ex.Message);
            return string.Empty;
        }
    }


}
