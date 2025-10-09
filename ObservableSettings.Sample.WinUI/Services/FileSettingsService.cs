using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace ObservableSettings.Sample.WinUI.Services;

public class JsonFileSettingsService : ISettingsService
{
    private List<ObservableObject> _settingsToListen = [];
    public T LoadAndListenSettings<T>() where T : ObservableObject, new()
    {
        if (!File.Exists(typeof(T).Name))
            File.Create((typeof(T).Name));
        string jsonString = File.ReadAllText(typeof(T).Name);
        var setting = JsonSerializer.Deserialize<T>(jsonString);
        setting.PropertyChanged += OnSettingPropertyChanged;
        _settingsToListen.Add(setting);
        return setting;
    }

    public void StopListening()
    {
        foreach (var setting in _settingsToListen)
        {
            setting.PropertyChanged -= OnSettingPropertyChanged;
        }
        _settingsToListen.Clear();
    }

    private static void OnSettingPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not ObservableObject setting || e.PropertyName is null) return;
        var containerName = setting.GetType().Name;
        JsonSerializer.Deserialize(containerName);
    }
}