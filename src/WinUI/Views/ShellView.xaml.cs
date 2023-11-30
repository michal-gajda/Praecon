namespace Praecon.WinUI.Views;

using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Praecon.WinUI.ViewModels;

public partial class ShellView : Window
{
    public ShellViewModel ViewModel => (ShellViewModel)this.DataContext;

    public ShellView()
    {
        this.InitializeComponent();
        this.DataContext = Ioc.Default.GetService<ShellViewModel>();
    }
}
