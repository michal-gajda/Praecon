﻿namespace Praecon.WinUI.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MvvmDialogs;

using Praecon.WinUI.Models.ViewModels;

public partial class UpdateArticleViewModel : ObservableObject, IModalDialogViewModel
{
    private readonly ILogger<UpdateArticleViewModel> logger;
    private readonly ISender mediator;

    [ObservableProperty] private bool isClosed = default;
    [ObservableProperty] private Article item = new();
    [ObservableProperty] private string windowTitle = "Article";

    public IAsyncRelayCommand CancelCommand { get; }
    public bool? DialogResult { get; private set; } = default;
    public IAsyncRelayCommand OkCommand { get; }

    public UpdateArticleViewModel(ILogger<UpdateArticleViewModel> logger, ISender mediator)
    {
        (this.logger, this.mediator) = (logger, mediator);

        this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);
        this.OkCommand = new AsyncRelayCommand(this.OkAsync);
    }

    private async Task CancelAsync(CancellationToken cancellationToken = default)
    {
        this.IsClosed = true;
        this.DialogResult = false;
        await Task.CompletedTask;
    }

    private async Task OkAsync(CancellationToken cancellationToken = default)
    {
        this.IsClosed = true;
        this.DialogResult = true;
        await Task.CompletedTask;
    }
}
