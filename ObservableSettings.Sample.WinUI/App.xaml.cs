using System;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ObservableSettings.Sample.Common;
using ObservableSettings.Sample.WinUI.Services;
using ObservableSettings.Sample.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ObservableSettings.Sample.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window? _window;

    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            var settingsService = new ApplicationSettingsService();
            var commonSettings = settingsService.LoadAndListenSettings<CommonSettings>();
            services.AddSingleton(settingsService);
            services.AddSingleton(commonSettings);
            services.AddSingleton<MainViewModel>();
            // Register your services here
        })
        .Build();

    public static T GetService<T>() where T : class
    {
        return _host.Services.GetService(typeof(T)) as T ?? throw new InvalidOperationException($"Service of type {typeof(T)} not registered.");
    }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }
}