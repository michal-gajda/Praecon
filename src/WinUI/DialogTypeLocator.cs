namespace Praecon.WinUI;

using System.ComponentModel;
using MvvmDialogs.DialogTypeLocators;
using Praecon.WinUI.ViewModels;
using Praecon.WinUI.Views;

internal sealed class DialogTypeLocator: IDialogTypeLocator
{
    public Type Locate(INotifyPropertyChanged viewModel)
        => viewModel switch
        {
            null => throw new ArgumentNullException(nameof(viewModel)),
            UpdateArticleViewModel => typeof(UpdateArticleView),
            _ => throw new ArgumentException($"No dialog view type found for view model type {viewModel.GetType()}"),
        };
}
