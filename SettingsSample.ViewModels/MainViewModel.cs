using CommunityToolkit.Mvvm.ComponentModel;
using SettingsSample.Common;

namespace SettingsSample.ViewModels;

public class MainViewModel : ObservableObject
{
    private CommonSettings _settings;
    public MainViewModel(CommonSettings settings)
    {
        _settings = settings;
    }
}