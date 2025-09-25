using CommunityToolkit.Mvvm.ComponentModel;
using SettingsSample.Common;

namespace SettingsSample.ViewModels;

public class MainViewModel : ObservableObject
{
    public CommonSettings Settings { get; }
    public MainViewModel(CommonSettings settings)
    {
        Settings = settings;
    }
}