using System.Collections.ObjectModel;

using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MvvmDialogs;

using Praecon.WinUI.Models.Commands;
using Praecon.WinUI.Models.Queries;
using Praecon.WinUI.Models.ViewModels;

namespace Praecon.WinUI.ViewModels;

public partial class ShellViewModel : ObservableObject, IModalDialogViewModel
{
    private readonly IDialogService dialogService;
    private readonly ILogger<ShellViewModel> logger;
    private readonly ILoggerFactory loggerFactory;
    private readonly IMapper mapper;
    private readonly ISender mediator;
    private readonly IReadOnlyList<ThumbnailItem> thumbnails;
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
        (this.dialogService, logger, this.loggerFactory, this.mapper, this.mediator, this.timeProvider) = (dialogService, loggerFactory.CreateLogger<ShellViewModel>(), loggerFactory, mapper, mediator, timeProvider);

        thumbnails = new List<ThumbnailItem>
        {
            new()
            {
                Id = Guid.Empty,
                Code = "None",
                Name = "Nic",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Code = "Random",
                Name = "Logowy",
            },
        };

        CreateArticleCommand = new AsyncRelayCommand(CreateArticleAsync);
        UpdateArticleCommand = new AsyncRelayCommand<Article?>(UpdateArticleAsync, CanUpdateArticle);

        LoadArticlesCommand = new AsyncRelayCommand(LoadArticlesAsync);
        RefreshArticlesCommand = new AsyncRelayCommand(RefreshArticlesAsync);

        ExitCommand = new AsyncRelayCommand(ExitAsync);
    }

    private bool CanUpdateArticle(Article? parameter) => parameter is not null;

    private async Task CreateArticleAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Call: {MethodName}", nameof(CreateArticleCommand));

        DateTimeOffset dateTimeOffset = timeProvider.GetUtcNow();

        Article? item = new()
        {
            Id = Guid.NewGuid(),
            Date = dateTimeOffset.DateTime,
            Thumbnails = thumbnails,
        };

        ILogger<UpdateArticleViewModel>? updateViewLogger = loggerFactory.CreateLogger<UpdateArticleViewModel>();

        UpdateArticleViewModel? viewModel = new(updateViewLogger, mediator)
        {
            Item = item,
        };

        bool? result = dialogService.ShowDialog(this, viewModel);

        if (result is true)
        {
            CreateArticle? command = mapper.Map<CreateArticle>(viewModel.Item);
            await mediator.Send(command, cancellationToken);
        }

        await Task.CompletedTask;
    }

    private async Task ExitAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Call: {MethodName}", nameof(ExitAsync));

        System.Windows.Application.Current.Shutdown();
        await Task.CompletedTask;
    }

    private async Task LoadArticlesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Call: {MethodName}", nameof(LoadArticlesCommand));

        ListArticles? query = new();
        IEnumerable<Article>? list = await mediator.Send(query, cancellationToken);

        foreach (Article? listItem in list)
        {
            Items.Add(listItem);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshArticlesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Call: {MethodName}", nameof(RefreshArticlesAsync));

        ListArticles? query = new();
        IEnumerable<Article>? list = await mediator.Send(query, cancellationToken);

        foreach (Article? listItem in list)
        {
            if (Items.Contains(listItem))
            {
                continue;
            }

            Guid id = listItem.Id;
            Article? item = Items.FirstOrDefault(article => article.Id == id);

            if (item is not null)
            {
                Items.Remove(item);
            }

            Items.Add(listItem);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateArticleAsync(Article? parameter, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Call: {MethodName}", nameof(UpdateArticleAsync));

        ArgumentNullException.ThrowIfNull(parameter);

        ILogger<UpdateArticleViewModel>? updateViewLogger = loggerFactory.CreateLogger<UpdateArticleViewModel>();

        Article item = parameter with { Thumbnails = thumbnails };

        UpdateArticleViewModel? viewModel = new(updateViewLogger, mediator)
        {
            Item = item,
        };

        bool? result = dialogService.ShowDialog(this, viewModel);

        if (result is true)
        {
            UpdateArticle? command = mapper.Map<UpdateArticle>(viewModel.Item);
            await mediator.Send(command, cancellationToken);
        }

        await Task.CompletedTask;
    }
}