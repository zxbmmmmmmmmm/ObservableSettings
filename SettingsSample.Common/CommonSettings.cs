using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SettingsSample.Common;

public partial class CommonSettings : ObservableObject
{
    [ObservableProperty]
    public string Name { get; set; }

    [ObservableProperty]
    public ObservableCollection<string> Names { get; set; }
}