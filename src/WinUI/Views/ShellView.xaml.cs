namespace Praecon.WinUI.Views;

using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Praecon.WinUI.ViewModels;

public partial class ShellView : Window
{
    public ShellViewModel ViewModel => (ShellViewModel)DataContext;

    public ShellView()
    {
        this.InitializeComponent();
        DataContext = Ioc.Default.GetService<ShellViewModel>();
    }
}
