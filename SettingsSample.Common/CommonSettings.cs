using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SettingsSample.Common;

public partial class CommonSettings : ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial int Count { get; set; }

    [ObservableProperty]
    public partial bool IsEnabled { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<Student> Students { get; set; } = [new("a", 15), new("b", 12), new("c", 18)];

    public CommonSettings()
    {
        Students.CollectionChanged += (s,e) => OnPropertyChanged(nameof(Students));
    }
}

public partial class Student(string name, int age) : ObservableObject
{
    [ObservableProperty]
    public string? Name { get; set; } = name;

    [ObservableProperty]
    public int Age { get; set; } = age;
}