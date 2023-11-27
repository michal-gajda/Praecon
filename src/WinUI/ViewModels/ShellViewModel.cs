namespace Praecon.WinUI.ViewModels;

using System.Collections.ObjectModel;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using Praecon.WinUI.Models.Commands;
using Praecon.WinUI.Models.Queries;
using Praecon.WinUI.Models.ViewModels;
using Praecon.WinUI.Views;

public partial class ShellViewModel : ObservableObject, IModalDialogViewModel
{
    private readonly IDialogService dialogService;
    private readonly ILogger<ShellViewModel> logger;
    private readonly ILoggerFactory loggerFactory;
    private readonly IMapper mapper;
    private readonly ISender mediator;
    private readonly TimeProvider timeProvider;

    [ObservableProperty] private Article? selectedItem = default;
    [ObservableProperty] private ObservableCollection<Article> items = new();
    [ObservableProperty] private string windowTitle = "Praecon";

    public ShellViewModel(IDialogService dialogService, ILoggerFactory loggerFactory, IMapper mapper, ISender mediator, TimeProvider timeProvider)
    {
        (this.dialogService, this.logger, this.loggerFactory, this.mapper, this.mediator, this.timeProvider) = (dialogService, loggerFactory.CreateLogger<ShellViewModel>(), loggerFactory, mapper, mediator, timeProvider);

        this.CreateArticleCommand = new AsyncRelayCommand(this.CreateArticleAsync);
        this.UpdateArticleCommand = new AsyncRelayCommand<Article?>(this.UpdateArticleAsync, this.CanUpdateArticle);

        this.LoadArticlesCommand = new AsyncRelayCommand(this.LoadArticlesAsync);
        this.RefreshArticlesCommand = new AsyncRelayCommand(this.RefreshArticlesAsync);

        this.ExitCommand = new AsyncRelayCommand(this.ExitAsync);
    }

    public bool? DialogResult { get; } = true;

    public IAsyncRelayCommand CreateArticleCommand { get; }
    public IAsyncRelayCommand<Article> UpdateArticleCommand { get; }
    public IAsyncRelayCommand LoadArticlesCommand { get; }
    public IAsyncRelayCommand RefreshArticlesCommand { get; }

    public IAsyncRelayCommand ExitCommand { get; }

    private async Task CreateArticleAsync(CancellationToken cancellationToken = default)
    {
        var dateTimeOffset = this.timeProvider.GetUtcNow();

        var viewModel = new UpdateArticleViewModel(this.loggerFactory, mediator)
        {
            Item = new Article
            {
                Id = Guid.NewGuid(),
                Date = dateTimeOffset.DateTime,
            }
        };

        var result = this.dialogService.ShowDialog<UpdateArticleView>(this, viewModel);

        if (result is true)
        {
            var command = this.mapper.Map<CreateArticle>(viewModel.Item);
            await this.mediator.Send(command, cancellationToken);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateArticleAsync(Article? parameter, CancellationToken cancellationToken = default)
    {
        if (parameter is null)
        {
            throw new NullReferenceException(nameof(parameter));
        }

        var viewModel = new UpdateArticleViewModel(this.loggerFactory, mediator)
        {
            Item = parameter with { },
        };

        var result = this.dialogService.ShowDialog<UpdateArticleView>(this, viewModel);

        if (result is true)
        {
            var command = this.mapper.Map<UpdateArticle>(viewModel.Item);
            await this.mediator.Send(command, cancellationToken);
        }

        await Task.CompletedTask;
    }

    private bool CanUpdateArticle(Article? parameter) => parameter is not null;

    private async Task LoadArticlesAsync(CancellationToken cancellationToken = default)
    {
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

    private async Task ExitAsync(CancellationToken cancellationToken = default)
    {
        System.Windows.Application.Current.Shutdown();
        await Task.CompletedTask;
    }
}
