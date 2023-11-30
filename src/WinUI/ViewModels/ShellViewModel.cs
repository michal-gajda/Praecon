namespace Praecon.WinUI.ViewModels;

using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MvvmDialogs;

using Praecon.WinUI.Models.Commands;
using Praecon.WinUI.Models.Interfaces;
using Praecon.WinUI.Models.Queries;
using Praecon.WinUI.Models.ViewModels;

public partial class ShellViewModel : ObservableObject, IModalDialogViewModel
{
    private readonly IDialogService dialogService;
    private readonly ILogger<ShellViewModel> logger;
    private readonly ILoggerFactory loggerFactory;
    private readonly IMapper mapper;
    private readonly ISender mediator;
    private readonly IMediaRepository repository;
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

    public ShellViewModel(IDialogService dialogService, ILoggerFactory loggerFactory, IMapper mapper, ISender mediator, IMediaRepository repository, TimeProvider timeProvider)
    {
        (this.dialogService, this.logger, this.loggerFactory, this.mapper, this.mediator, this.repository, this.timeProvider) = (dialogService, loggerFactory.CreateLogger<ShellViewModel>(), loggerFactory, mapper, mediator, repository, timeProvider);

        var image = GetBitmapImage(this.repository.Read(Guid.Empty));

        this.thumbnails = new List<ThumbnailItem>
        {
            new()
            {
                Id = Guid.Empty,
                Code = "None",
                Name = "Nic",
                Preview = image,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Code = "Random",
                Name = "Logowy",
                Preview = image,
            },
        };

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

        DateTimeOffset dateTimeOffset = this.timeProvider.GetUtcNow();

        Article? item = new()
        {
            Id = Guid.NewGuid(),
            Date = dateTimeOffset.DateTime,
            Thumbnails = this.thumbnails,
        };

        ILogger<UpdateArticleViewModel>? updateViewLogger = this.loggerFactory.CreateLogger<UpdateArticleViewModel>();

        UpdateArticleViewModel? viewModel = new(updateViewLogger, this.mediator)
        {
            Item = item,
        };

        bool? result = this.dialogService.ShowDialog(this, viewModel);

        if (result is true)
        {
            CreateArticle? command = this.mapper.Map<CreateArticle>(viewModel.Item);
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

        ListArticles? query = new();
        IEnumerable<Article>? list = await this.mediator.Send(query, cancellationToken);

        foreach (Article? listItem in list)
        {
            this.Items.Add(listItem);
        }

        await Task.CompletedTask;
    }

    private async Task RefreshArticlesAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Call: {MethodName}", nameof(this.RefreshArticlesAsync));

        ListArticles? query = new();
        IEnumerable<Article>? list = await this.mediator.Send(query, cancellationToken);

        foreach (Article? listItem in list)
        {
            if (this.Items.Contains(listItem))
            {
                continue;
            }

            Guid id = listItem.Id;
            Article? item = this.Items.FirstOrDefault(article => article.Id == id);

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

        ILogger<UpdateArticleViewModel>? updateViewLogger = this.loggerFactory.CreateLogger<UpdateArticleViewModel>();

        Article item = parameter with
        {
            Thumbnails = this.thumbnails,
        };

        UpdateArticleViewModel? viewModel = new(updateViewLogger, this.mediator)
        {
            Item = item,
        };

        bool? result = this.dialogService.ShowDialog(this, viewModel);

        if (result is true)
        {
            UpdateArticle? command = this.mapper.Map<UpdateArticle>(viewModel.Item);
            await this.mediator.Send(command, cancellationToken);
        }

        await Task.CompletedTask;
    }

    private static BitmapImage GetBitmapImage(byte[] imageBytes)
    {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = new MemoryStream(imageBytes);
        bitmapImage.EndInit();
        return bitmapImage;
    }
}
