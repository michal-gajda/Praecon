namespace Praecon.WinUI.Views;

using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Praecon.WinUI.ViewModels;

public partial class UpdateArticleView : Window
{
    public UpdateArticleView()
    {
        this.InitializeComponent();
        this.DataContext = Ioc.Default.GetService<UpdateArticleViewModel>();
    }

    public UpdateArticleViewModel ViewModel => (UpdateArticleViewModel)this.DataContext;
}
