namespace Praecon.WinUI.Models.CommandHandlers;

using Praecon.WinUI.Models.Commands;
using Praecon.WinUI.Models.Entities;
using Praecon.WinUI.Models.Interfaces;

internal sealed class UpdateArticleHandler : IRequestHandler<UpdateArticle>
{
    private readonly ILogger<UpdateArticleHandler> logger;
    private readonly IArticleRepository repository;

    public UpdateArticleHandler(ILogger<UpdateArticleHandler> logger, IArticleRepository repository)
        => (this.logger, this.repository) = (logger, repository);

    public async Task Handle(UpdateArticle request, CancellationToken cancellationToken)
    {
        ArticleEntity entity = new(request.Id, request.Title, request.Date, request.Payload, request.Published, request.ThumbnailId, request.MediaId, request.Tags);

        await this.repository.UpdateAsync(entity, cancellationToken);
    }
}
