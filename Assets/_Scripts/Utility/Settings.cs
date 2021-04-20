using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Singleton Settings class containing all of the settings for the application
/// </summary>
public sealed class Settings
{
    private static readonly Settings instance = new Settings();
    public static Settings Instance => instance;

    private static string SettingsFilePath => Application.persistentDataPath + "/" + "config.ini";

    private SettingsData data;
    
    /// <summary>
    /// Explicity static constructor to inform C# compiler not to mark type as beforefieldinit
    /// </summary>
    static Settings() { }

    /// <summary>
    /// Initializes an instance of <see cref="Settings"/>
    /// </summary>
    private Settings()
    {
        // Read Settings from saved file if it exists
        if (File.Exists(SettingsFilePath))
        {
            ReadFromFile(File.OpenRead(SettingsFilePath));
        }
        else
        {
            data = new SettingsData();
        }
        
        Debug.Log("Setting Graphics Quality to Level: " + data.GraphicsQualityLevel);
        QualitySettings.SetQualityLevel(data.GraphicsQualityLevel);
    }

    /// <summary>
    /// Saves the current settings to settings file.
    /// </summary>
    public void Save()
    {
        WriteToFile(File.OpenWrite(SettingsFilePath));
    }

    /// <summary>
    /// Gets or sets the Mouse Sensitivity property
    /// </summary>
    public int MouseSensitivity
    {
        get => data.MouseSensitivity;
        set => data.MouseSensitivity = value;
    }

    /// <summary>
    /// Gets or sets the Scroll Sensitivity property
    /// </summary>
    public int ScrollSensitivity
    {
        get => data.ScrollSensitivity;
        set => data.ScrollSensitivity = value;
    }

    /// <summary>
    /// Gets or sets the Inverted X Axis property
    /// </summary>
    public bool InvertedXAxis
    {
        get => data.InvertedXAxis;
        set => data.InvertedXAxis = value;
    }

    /// <summary>
    /// Gets or sets the Inverted Y Axis property
    /// </summary>
    public bool InvertedYAxis
    {
        get => data.InvertedYAxis;
        set => data.InvertedYAxis = value;
    }

    public int MasterVolume
    {
        get => data.MasterVolume;
        set => data.MasterVolume = value;
    }

    public int MusicVolume
    {
        get => data.MusicVolume;
        set => data.MusicVolume = value;
    }

    public int DialogueVolume
    {
        get => data.DialogueVolume;
        set => data.DialogueVolume = value;
    }

    public int SFXVolume
    {
        get => data.SFXVolume;
        set => data.SFXVolume = value;
    }

    public int GraphicsQualityLevel
    {
        get => data.GraphicsQualityLevel;
        set => data.GraphicsQualityLevel = value;
    }

    /// <summary>
    /// Private class representing setting data.
    /// </summary>
    [Serializable]
    private class SettingsData
    {
        public int MouseSensitivity { get; set; } = 5;
        public int ScrollSensitivity { get; set; } = 2;
        public bool InvertedYAxis { get; set; } = true;
        public bool InvertedXAxis { get; set; } = false;
        public int MasterVolume { get; set; } = 10;
        public int MusicVolume { get; set; } = 8;
        public int DialogueVolume { get; set; } = 10;
        public int SFXVolume { get; set; } = 10;

        public int GraphicsQualityLevel { get; set; } = 1;
    }

    /// <summary>
    /// Reads the setting data from the passed filestream
    /// </summary>
    /// <param name="stream">The filestream containing the settings data</param>
    private void ReadFromFile(FileStream stream)
    {
        var formatter = new BinaryFormatter();
        data = (SettingsData)formatter.Deserialize(stream);
        stream.Close();
    }

    /// <summary>
    /// Writes the current setting data to the passed filestream
    /// </summary>
    /// <param name="stream">The filestream to write to</param>
    private void WriteToFile(FileStream stream)
    {
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
        stream.Close();
    }
}
