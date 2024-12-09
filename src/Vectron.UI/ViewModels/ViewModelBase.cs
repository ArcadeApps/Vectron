using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Vectron.Core;

namespace Vectron.UI.ViewModels;

public class ViewModelBase : ReactiveObject, IActivatableViewModel, IRoutableViewModel
{
    protected IObservable<bool> IsBusyChanged { get; init; }

    public bool IsBusy
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    [ObservableAsProperty]
    public bool HasError { get; }

    public ObservableCollection<Error> Errors { get; } = [];
    
    
    public string? UrlPathSegment { get; protected init; }
    public IScreen HostScreen { get; }
    
    public ViewModelActivator Activator { get; } = new();
    
    
    protected ViewModelBase(IScreen hostScreen) : this() => HostScreen = hostScreen;
    
    protected ViewModelBase()
    {
        IsBusyChanged = this.WhenAnyValue(x => x.IsBusy);
        Errors
            .ToObservableChangeSet(x => x)
            .ToCollection()
            .Select(x => x.Count != 0)
            .ToPropertyEx(this, x => x.HasError);
    }
}