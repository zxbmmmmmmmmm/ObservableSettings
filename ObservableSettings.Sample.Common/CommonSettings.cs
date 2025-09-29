using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace ObservableSettings.Sample.Common;

public partial class CommonSettings : ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial int Count { get; set; }

    [ObservableProperty]
    public partial bool IsEnabled { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedWhen(nameof(Students.ListChanged))]
    public partial ObservableBindingList<Student> Students { get; set; } = [new("a", 15), new("b", 12), new("c", 18)];

    [ObservableProperty]
    [NotifyPropertyChangedWhen(nameof(Tags.CollectionChanged))]
    public partial ObservableCollection<string> Tags { get; set; } = ["Tag1", "Tag2", "Tag3"];
}

public partial class Student(string name, int age) : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = name;

    [ObservableProperty]
    public partial int Age { get; set; } = age;
}