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
        ArticleEntity entity = new(request.Id, request.Title, request.Date, request.Payload, request.Published, request.ThumbnailId, request.MediaId, request.Tags);

        await this.repository.CreateAsync(entity, cancellationToken);
    }
}
