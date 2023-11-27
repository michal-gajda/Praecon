namespace Praecon.WinUI.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using Praecon.WinUI.Models.ViewModels;

public partial class UpdateArticleViewModel : ObservableObject, IModalDialogViewModel
{
    private readonly ILogger<UpdateArticleViewModel> logger;
    private readonly ISender mediator;

    [ObservableProperty] private string windowTitle = "Article";
    [ObservableProperty] private Article item = new();
    [ObservableProperty] private bool isClose = default;

    public UpdateArticleViewModel(ILoggerFactory loggerFactory, ISender mediator)
    {
        (this.logger, this.mediator) = (loggerFactory.CreateLogger<UpdateArticleViewModel>(), mediator);

        this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);
        this.OkCommand = new AsyncRelayCommand(this.OkAsync);
    }

    public bool? DialogResult { get; private set; } = default;

    public IAsyncRelayCommand CancelCommand { get; }
    public IAsyncRelayCommand OkCommand { get; }

    private async Task CancelAsync(CancellationToken cancellationToken = default)
    {
        this.IsClose = true;
        this.DialogResult = false;
        await Task.CompletedTask;
    }

    private async Task OkAsync(CancellationToken cancellationToken = default)
    {
        this.IsClose = true;
        this.DialogResult = true;
        await Task.CompletedTask;
    }
}
