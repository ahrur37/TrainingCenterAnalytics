using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using TCA.Desktop.Services;
using TCA.Desktop.ViewModels;
using TCA.Desktop.Views;

namespace TCA.Desktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();

        collection.AddSingleton<SessionService>();
        collection.AddSingleton<ApiService>();
        collection.AddSingleton<SignalRService>();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<INavigator>(p => p.GetRequiredService<MainWindowViewModel>());

        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<RegistrationViewModel>();
        collection.AddTransient<LobbyViewModel>();
        collection.AddTransient<CreateRequestViewModel>();
        collection.AddTransient<EditRequestViewModel>();
        collection.AddTransient<DictionariesViewModel>();
        collection.AddTransient<DashboardViewModel>();

        Services = collection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainVm = Services.GetRequiredService<MainWindowViewModel>();
            mainVm.CurrentView = Services.GetRequiredService<LoginViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainVm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}