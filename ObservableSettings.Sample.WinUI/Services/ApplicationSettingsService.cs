using CommunityToolkit.Mvvm.ComponentModel;
using ObservableSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using Windows.Storage;

namespace ObservableSettings.Sample.WinUI.Services;

public class ApplicationSettingsService : ISettingsService
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
            prop.SetValue(setting, container.GetSettings(prop.Name, prop.PropertyType, prop.GetValue(setting)));
        }
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
        var container = ApplicationData.Current.LocalSettings.Containers[containerName];
        var propInfo = setting.GetType().GetProperty(e.PropertyName);
        container.SetSettings(e.PropertyName, propInfo?.GetValue(setting), propInfo?.PropertyType);
    }
}
public static class ApplicationDataContainerExtensions
{
    public static T? GetSettings<T>(this ApplicationDataContainer container, string propertyName, T? defaultValue)
    {
        try
        {
            if (container.Values.TryGetValue(propertyName, out var value))
            {

                if (typeof(T).IsEnum)
                {
                    var tempValue = value.ToString();
                    if(Enum.TryParse(typeof(T), tempValue, out var result))
                        return (T)result;
                }

                if (typeof(T) == typeof(bool) ||
                    typeof(T) == typeof(byte) ||
                    typeof(T) == typeof(char) ||
                    typeof(T) == typeof(DateTimeOffset) ||
                    typeof(T) == typeof(double) ||
                    typeof(T) == typeof(Guid) ||
                    typeof(T) == typeof(short) ||
                    typeof(T) == typeof(int) ||
                    typeof(T) == typeof(long) ||
                    typeof(T) == typeof(float) ||
                    typeof(T) == typeof(string) ||
                    typeof(T) == typeof(TimeSpan) ||
                    typeof(T) == typeof(ushort) ||
                    typeof(T) == typeof(uint) ||
                    typeof(T) == typeof(ulong) ||
                    typeof(T) == typeof(Uri)
                    )
                {
                    return (T)container.Values[propertyName];
                }
                return JsonSerializer.Deserialize<T>((string)value);
            }
            container.Values[propertyName] = defaultValue;
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static object? GetSettings(this ApplicationDataContainer container, string propertyName, Type valueType, object? defaultValue)
    {
        try
        {
            if (container.Values.TryGetValue(propertyName, out var value))
            {
                if (valueType.IsEnum)
                {
                    var tempValue = value.ToString();
                    if (Enum.TryParse(valueType, tempValue, out var result))
                        return result;
                }

                if (valueType == typeof(bool) || 
                    valueType == typeof(byte) ||
                    valueType == typeof(char) ||
                    valueType == typeof(DateTimeOffset) ||
                    valueType == typeof(double) ||
                    valueType == typeof(Guid) ||
                    valueType == typeof(short) ||
                    valueType == typeof(int) ||
                    valueType == typeof(long) ||
                    valueType == typeof(float) ||
                    valueType == typeof(string) ||
                    valueType == typeof(TimeSpan) ||
                    valueType == typeof(ushort) ||
                    valueType == typeof(uint) ||
                    valueType == typeof(ulong) ||
                    valueType == typeof(Uri)
                   )
                {
                    return container.Values[propertyName];
                }
                return JsonSerializer.Deserialize((string)value, valueType);
            }
            container.Values[propertyName] = defaultValue;
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static void SetSettings(this ApplicationDataContainer container, string propertyName, object? value, Type? valueType)
    {
        if (value == null)
        {
            container.Values.Remove(propertyName);
            return;
        }
        if (valueType == typeof(bool) ||
            valueType == typeof(byte) ||
            valueType == typeof(char) ||
            valueType == typeof(DateTimeOffset) ||
            valueType == typeof(double) ||
            valueType == typeof(Guid) ||
            valueType == typeof(short) ||
            valueType == typeof(int) ||
            valueType == typeof(long) ||
            valueType == typeof(float) ||
            valueType == typeof(string) ||
            valueType == typeof(TimeSpan) ||
            valueType == typeof(ushort) ||
            valueType == typeof(uint) ||
            valueType == typeof(ulong) ||
            valueType == typeof(Uri)
           )
        {
            container.Values[propertyName] = value;
        }
        else
        {
            container.Values[propertyName] = JsonSerializer.Serialize(value);
        }
    }

}