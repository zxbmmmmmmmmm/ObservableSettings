using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SettingsSample.Common;

namespace SettingsSample.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public CommonSettings Settings { get; }

    [ObservableProperty]
    public Student Student { get; set; } = new Student("New Student", 20);

    [RelayCommand]
    public void AddStudent()
    {
        Settings.Students.Add(Student);
    }

    [RelayCommand]
    public void RemoveStudent(Student student)
    {
        Settings.Students.Remove(student);
    }

    public MainViewModel(CommonSettings settings)
    {
        Settings = settings;
    }
}