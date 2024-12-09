using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Vectron.UI.Utilities;
using Vectron.UI.ViewModels;
using Vectron.UI.Views;

namespace Vectron.UI;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public override void Initialize()
    {
        Resources[typeof(IServiceProvider)] = _serviceProvider;
        AvaloniaXamlLoader.Load(this);
        DataTemplates.Add(_serviceProvider.GetRequiredService<ViewLocator>());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = this.CreateInstance<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}