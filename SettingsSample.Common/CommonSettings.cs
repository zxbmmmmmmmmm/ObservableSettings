using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SettingsSample.Common;

public partial class CommonSettings : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<string> Names { get; set; }
}