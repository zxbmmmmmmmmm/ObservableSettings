using CommunityToolkit.Mvvm.ComponentModel;

namespace ObservableSettings.Sample.WinUI.Services;

public interface ISettingsService
{
    public T LoadAndListenSettings<T>() where T : ObservableObject, new();
    public void StopListening();
}