using Avalonia.ReactiveUI;
using Vectron.UI.ViewModels;

namespace Vectron.UI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}