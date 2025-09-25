using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Windows.Storage;

namespace SettingsSample.WinUI.Services;

public class ApplicationSettingsService
{
    private List<ObservableObject> _settingsToListen = [];
    public T LoadAndListenSettings<T>() where T : ObservableObject, new()
    {
        if (!ApplicationData.Current.LocalSettings.Containers.ContainsKey(typeof(T).Name))
            ApplicationData.Current.LocalSettings.CreateContainer(typeof(T).Name, ApplicationDataCreateDisposition.Always);
        var container = ApplicationData.Current.LocalSettings.Containers[typeof(T).Name];
        var setting = new T();
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            prop.SetValue(setting, container.GetSettings(prop.Name, prop.GetValue(setting)));
        }
        setting.PropertyChanged += OnSettingPropertyChanged;
        _settingsToListen.Add(setting);
        return setting;
    }

    private void OnSettingPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not ObservableObject setting || e.PropertyName is null) return;
        var containerName = setting.GetType().Name;
        var container = ApplicationData.Current.LocalSettings.Containers[containerName];
        container.Values[e.PropertyName] = setting.GetType().GetProperty(e.PropertyName)?.GetValue(setting);
    }
}

public static class ApplicationDataContainerExtensions
{
    public static T GetSettings<T>(this ApplicationDataContainer container, string propertyName, T defaultValue)
    {
        try
        {
            if (container.Values.TryGetValue(propertyName,out var value) &&
                container.Values[propertyName] is not null &&
                !string.IsNullOrEmpty(container.Values[propertyName].ToString()))
            {
                switch (defaultValue)
                {
                    case bool:
                        return (T)(object)bool.Parse(value.ToString());
                    case Enum:
                    {
                        var tempValue = value.ToString();
                        Enum.TryParse(typeof(T), tempValue, out var result);
                        return (T)result;
                    }
                    // WinRT base data types
                    case bool or 
                        byte or 
                        char or 
                        DateTimeOffset or
                        double or
                        Guid or 
                        short or
                        int or 
                        long or
                        float or
                        string or
                        TimeSpan or
                        ushort or
                        uint or
                        ulong or
                        Uri:
                        return (T)container.Values[propertyName];
                    case null:
                        try
                        {
                            return (T)container.Values[propertyName];
                        }
                        catch
                        {
                            return JsonSerializer.Deserialize<T>((string)value);
                        }
                    default:
                        return JsonSerializer.Deserialize<T>((string)value);
                }
            }
            container.Values[propertyName] = defaultValue;
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
}