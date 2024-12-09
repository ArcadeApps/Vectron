using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Vectron.UI.ViewModels;

namespace Vectron.UI;

/*

   public class AppViewLocator : ReactiveUI.IViewLocator
   {
       public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
       {
           FirstViewModel context => new FirstView { DataContext = context },
           _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
       };
   }
 */
public class ViewLocator : IDataTemplate
{
    private readonly IServiceProvider _serviceProvider;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type == null) return new TextBlock { Text = "Not Found: " + name };
        var scope = _serviceProvider.CreateScope();
        return (Control)scope.ServiceProvider.GetRequiredService(type);
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}