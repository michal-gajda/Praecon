namespace Praecon.WinUI.ViewModels;

using System.Collections.ObjectModel;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using Praecon.WinUI.Models.Commands;
using Praecon.WinUI.Models.Queries;
using Praecon.WinUI.Models.ViewModels;

public partial class ShellViewModel : ObservableObject, IModalDialogViewModel
{
    private readonly IDialogService dialogService;
    private readonly ILogger<ShellViewModel> logger;
    private readonly ILoggerFactory loggerFactory;
    private readonly IMapper mapper;
    private readonly ISender mediator;
    private readonly TimeProvider timeProvider;

    [ObservableProperty] private ObservableCollection<Article> items = new();
    [ObservableProperty] private Article? selectedItem = default;
    [ObservableProperty] private string windowTitle = "Praecon";

    public IAsyncRelayCommand CreateArticleCommand { get; }

    public bool? DialogResult { get; } = true;

    public IAsyncRelayCommand ExitCommand { get; }
    public IAsyncRelayCommand LoadArticlesCommand { get; }
    public IAsyncRelayCommand RefreshArticlesCommand { get; }
    public IAsyncRelayCommand<Article> UpdateArticleCommand { get; }

    public ShellViewModel(IDialogService dialogService, ILoggerFactory loggerFactory, IMapper mapper, ISender mediator, TimeProvider timeProvider)
    {
        (this.dialogService, this.logger, this.loggerFactory, this.mapper, this.mediator, this.timeProvider) = (dialogService, loggerFactory.CreateLogger<ShellViewModel>(), loggerFactory, mapper, mediator, timeProvider);

        this.CreateArticleCommand = new AsyncRelayCommand(this.CreateArticleAsync);
        this.UpdateArticleCommand = new AsyncRelayCommand<Article?>(this.UpdateArticleAsync, this.CanUpdateArticle);

        this.LoadArticlesCommand = new AsyncRelayCommand(this.LoadArticlesAsync);
        this.RefreshArticlesCommand = new AsyncRelayCommand(this.RefreshArticlesAsync);

        this.ExitCommand = new AsyncRelayCommand(this.ExitAsync);
    }

    private bool CanUpdateArticle(Article? parameter) => parameter is not null;

    private async Task CreateArticleAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Call: {MethodName}", nameof(this.CreateArticleCommand));

        var dateTimeOffset = this.timeProvider.GetUtcNow();

        var item = new Article
        {
            Id = Guid.NewGuid(),
            Date = dateTimeOffset.DateTime,
        };

        var updateViewLogger = this.loggerFactory.CreateLogger<UpdateArticleViewModel>();

        var viewModel = new UpdateArticleViewModel(updateViewLogger, this.mediator)
        {
            Item = item,
        };

        var result = this.dialogService.ShowDialog(this, viewModel);

        if (result is true)
        {
            var command = this.mapper.Map<CreateArticle>(viewModel.Item);
            await this.mediator.Send(command, cancellationToken);
        }

        await Task.CompletedTask;
    }

    private async Task ExitAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Call: {MethodName}", nameof(this.ExitAsync));

        System.Windows.Application.Current.Shutdown();
        await Task.CompletedTask;
    }

    private async Task LoadArticlesAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Call: {MethodName}", nameof(this.LoadArticlesCommand));

        var query = new ListArticles();
        var list = await this.mediator.Send(query, cancellationToken);

        foreach (var listItem in list)
        {
            this.Items.Add(listItem);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshArticlesAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Call: {MethodName}", nameof(this.RefreshArticlesAsync));

        var query = new ListArticles();
        var list = await this.mediator.Send(query, cancellationToken);

        foreach (var listItem in list)
        {
            if (this.Items.Contains(listItem))
            {
                continue;
            }

            var id = listItem.Id;
            var item = this.Items.FirstOrDefault(article => article.Id == id);

            if (item is not null)
            {
                this.Items.Remove(item);
            }

            this.Items.Add(listItem);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateArticleAsync(Article? parameter, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Call: {MethodName}", nameof(this.UpdateArticleAsync));

        ArgumentNullException.ThrowIfNull(parameter);

        var updateViewLogger = this.loggerFactory.CreateLogger<UpdateArticleViewModel>();

        var viewModel = new UpdateArticleViewModel(updateViewLogger, this.mediator)
        {
            Item = parameter with { },
        };

        var result = this.dialogService.ShowDialog(this, viewModel);

        if (result is true)
        {
            var command = this.mapper.Map<UpdateArticle>(viewModel.Item);
            await this.mediator.Send(command, cancellationToken);
        }

        await Task.CompletedTask;
    }
}
