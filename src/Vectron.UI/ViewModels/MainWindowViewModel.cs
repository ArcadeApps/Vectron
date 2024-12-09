using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Vectron.Core.Authentication;

namespace Vectron.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; } = new();


    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        UrlPathSegment = "main";
        Router.Navigate.Execute(serviceProvider.GetRequiredService<LoginViewModel>());
    }

    // public MainWindowViewModel() : this(DesignTimeHelper.Services.GetRequiredService<IServerDiscoveryApi>())
    // {
    // }
}