using ReactiveUI;

namespace Vectron.UI;

public class AppViewLocator : IViewLocator
{
    private readonly ViewLocator _locator;
    public AppViewLocator(){}
    public AppViewLocator(ViewLocator viewLocator)
    {
        _locator = viewLocator;
    }
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        throw new System.NotImplementedException();
    }
}