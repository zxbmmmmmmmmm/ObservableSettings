using CommunityToolkit.Mvvm.ComponentModel;
using ObservableSettings.Sample.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.ApplicationModel;

namespace ObservableSettings.Sample.WinUI.Services;

public class JsonFileSettingsService : ISettingsService
{
    private List<ObservableObject> _settingsToListen = [];

    private string BaseDirectory { get; init; } = Package.Current.InstalledPath;

    public T LoadAndListen<T>() where T : ObservableObject, new()
    {
        var fileName = typeof(T).Name + ".json";
        var dir = Path.Combine(BaseDirectory, fileName);
        T? setting = null;
        if (!File.Exists(dir))
        {
            setting = CreateDefaultSetting<T>(dir);
        }
        else
        {
            string jsonString = File.ReadAllText(dir);
            try
            {
                setting = JsonSerializer.Deserialize<T>(jsonString);
            }
            catch(JsonException)
            {
                setting = CreateDefaultSetting<T>(dir);
            }
        }
        setting.PropertyChanged += OnSettingPropertyChanged;
        _settingsToListen.Add(setting);
        return setting;
    }

    public void SaveAll()
    {
        foreach (var setting in _settingsToListen)
        {
            Save(setting);
        }
    }

    public void StopListening()
    {
        foreach (var setting in _settingsToListen)
        {
            setting.PropertyChanged -= OnSettingPropertyChanged;
        }
        _settingsToListen.Clear();
    }

    private void Save(ObservableObject setting)
    {
        var fileName = setting.GetType().Name + ".json";
        var dir = Path.Combine(BaseDirectory, fileName);
        // TODO: Fix JsonSerializer
        File.WriteAllText(dir, JsonSerializer.Serialize(setting, SourceGenerationContext.Default.CommonSettings));
    }

    private T CreateDefaultSetting<T>(string dir) where T : ObservableObject, new()
    {
        var stream = File.Create(dir);
        stream.Close();
        var defaultSetting = new T();
        File.WriteAllText(dir, JsonSerializer.Serialize(defaultSetting));
        return defaultSetting;
    }

    private void OnSettingPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not ObservableObject setting || e.PropertyName is null) return;
        Save(setting);
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(CommonSettings))]
internal partial class SourceGenerationContext : JsonSerializerContext { }