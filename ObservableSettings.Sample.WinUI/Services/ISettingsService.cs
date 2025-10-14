using CommunityToolkit.Mvvm.ComponentModel;

namespace ObservableSettings.Sample.WinUI.Services;

public interface ISettingsService
{
    public T LoadAndListen<T>() where T : ObservableObject, new();

    public void StopListening();

    public void SaveAll();
}