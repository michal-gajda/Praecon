namespace Praecon.WinUI.Models.CommandHandlers;

using Praecon.WinUI.Models.Commands;
using Praecon.WinUI.Models.Entities;
using Praecon.WinUI.Models.Interfaces;

internal sealed class CreateArticleHandler : IRequestHandler<CreateArticle>
{
    private readonly ILogger<CreateArticleHandler> logger;
    private readonly IArticleRepository repository;

    public CreateArticleHandler(ILogger<CreateArticleHandler> logger, IArticleRepository repository)
        => (this.logger, this.repository) = (logger, repository);

    public async Task Handle(CreateArticle request, CancellationToken cancellationToken)
    {
        var entity = new ArticleEntity(request.Id, request.Title, request.Date, request.Payload, false, request.ThumbnailId, request.MediaId, request.Tags);

        await this.repository.CreateAsync(entity, cancellationToken);
    }
}
