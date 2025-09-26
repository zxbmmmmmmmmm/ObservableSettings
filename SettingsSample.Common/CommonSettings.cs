using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial BindingList<Student> Students { get; set; } = [new("a", 15), new("b", 12), new("c", 18)];

    partial void OnStudentsChanged(BindingList<Student> oldValue, BindingList<Student> newValue)
    {
        oldValue.ListChanged -= OnListChanged;
        newValue.ListChanged += OnListChanged;
    }

    private void OnListChanged(object? sender, ListChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Students));
    }


}


public partial class Student(string name, int age) : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = name;

    [ObservableProperty]
    public partial int Age { get; set; } = age;
}