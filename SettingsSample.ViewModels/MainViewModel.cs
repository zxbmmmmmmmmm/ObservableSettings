using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SettingsSample.Common;

namespace SettingsSample.ViewModels;

public partial class MainViewModel(CommonSettings settings) : ObservableObject
{
    public CommonSettings Settings { get; } = settings;

    [ObservableProperty]
    public partial string Name { get; set; } = "New Student";

    [ObservableProperty] 
    public partial int Age { get; set; } = 10;

    [RelayCommand]
    public void AddStudent()
    {
        Settings.Students.Add(new Student(Name,Age));
    }

    [RelayCommand]
    public void RemoveStudent(Student student)
    {
        Settings.Students.Remove(student);
    }

    [RelayCommand]
    public void AddTag(string tag)
    {
        Settings.Tags.Add(tag);
    }

    [RelayCommand]
    public void RemoveTag(string tag)
    {
        Settings.Tags.Remove(tag);
    }
}