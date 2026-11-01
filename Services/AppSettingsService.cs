using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace SmartCalculator.Services;

public sealed class AppSettingsService
{
    private sealed class SettingsDocument
    {
        public int SchemaVersion { get; set; } = 2;
        public string ThemeId { get; set; } = "charcoal-black";
        public string SkinId { get; set; } = "classic-scientific";
        public bool IsCompactMode { get; set; } = false;
        public bool IsAlwaysOnTop { get; set; } = false;
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly string _legacyThemePath;

    public AppSettingsService(string? settingsDirectory = null, string? legacyThemePath = null)
    {
        SettingsDirectory = settingsDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SmartCalculator");
        SettingsPath = Path.Combine(SettingsDirectory, "settings.json");
        _legacyThemePath = legacyThemePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme_setting.txt");
    }

    public string SettingsDirectory { get; }
    public string SettingsPath { get; }

    public string? LoadThemeId()
    {
        var settings = LoadDocument();
        return string.IsNullOrWhiteSpace(settings?.ThemeId) ? null : settings.ThemeId;
    }

    public string? LoadSkinId()
    {
        var settings = LoadDocument();
        return string.IsNullOrWhiteSpace(settings?.SkinId) ? null : settings.SkinId;
    }

    public bool LoadCompactMode()
    {
        var settings = LoadDocument();
        return settings?.IsCompactMode ?? false;
    }

    public bool SaveCompactMode(bool isCompact)
    {
        var settings = LoadDocument() ?? new SettingsDocument();
        settings.IsCompactMode = isCompact;
        return SaveDocument(settings);
    }

    public bool LoadAlwaysOnTop()
    {
        var settings = LoadDocument();
        return settings?.IsAlwaysOnTop ?? false;
    }

    public bool SaveAlwaysOnTop(bool isAlwaysOnTop)
    {
        var settings = LoadDocument() ?? new SettingsDocument();
        settings.IsAlwaysOnTop = isAlwaysOnTop;
        return SaveDocument(settings);
    }

    public int? LoadLegacyThemeIndex()
    {
        try
        {
            if (File.Exists(_legacyThemePath) && int.TryParse(File.ReadAllText(_legacyThemePath), out int index))
                return index;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Trace.TraceWarning($"Unable to read legacy theme setting: {ex.Message}");
        }

        return null;
    }

    public bool SaveThemeId(string themeId)
    {
        var settings = LoadDocument() ?? new SettingsDocument();
        settings.ThemeId = themeId;
        return SaveDocument(settings);
    }

    public bool SaveSkinId(string skinId)
    {
        var settings = LoadDocument() ?? new SettingsDocument();
        settings.SkinId = skinId;
        return SaveDocument(settings);
    }

    private SettingsDocument? LoadDocument()
    {
        try
        {
            if (!File.Exists(SettingsPath))
                return null;

            return JsonSerializer.Deserialize<SettingsDocument>(File.ReadAllText(SettingsPath), JsonOptions);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            Trace.TraceWarning($"Unable to read SmartCalculator settings: {ex.Message}");
            return null;
        }
    }

    private bool SaveDocument(SettingsDocument settings)
    {
        string temporaryPath = SettingsPath + ".tmp";

        try
        {
            Directory.CreateDirectory(SettingsDirectory);
            settings.SchemaVersion = 2;
            var json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, SettingsPath, true);
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Trace.TraceWarning($"Unable to save SmartCalculator settings: {ex.Message}");
            try
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
            catch (Exception cleanupEx) when (cleanupEx is IOException or UnauthorizedAccessException)
            {
                Trace.TraceWarning($"Unable to remove temporary settings file: {cleanupEx.Message}");
            }

            return false;
        }
    }
}
