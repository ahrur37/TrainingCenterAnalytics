using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using TCA.Desktop.Services;
using TCA.Desktop.ViewModels;
using TCA.Desktop.Views;

namespace TCA.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var sessionservice = new SessionService();
            var apiservice = new ApiService(sessionservice);
            var mainvm = new MainWindowViewModel(sessionservice, apiservice);
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainvm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}