using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Reflection;

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
    public partial ObservableBindingList<Student> Students { get; set; } = [new("a", 15), new("b", 12), new("c", 18)];

    [ObservableProperty]
    public partial ObservableCollection<string> Tags { get; set; } = ["Tag1", "Tag2", "Tag3"];

    public CommonSettings()
    {
        Tags.CollectionChanged += OnCollectionChanged;
        Students.ListChanged += OnListChanged;
    }

    partial void OnTagsChanged(ObservableCollection<string> oldValue, ObservableCollection<string> newValue)
    {
        oldValue.CollectionChanged -= OnCollectionChanged;
        newValue.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Tags));
    }

    partial void OnStudentsChanged(ObservableBindingList<Student> oldValue, ObservableBindingList<Student> newValue)
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