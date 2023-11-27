namespace Praecon.WinUI.Models.QueryHandlers;

using AutoMapper;
using Praecon.WinUI.Models.Entities;
using Praecon.WinUI.Models.Interfaces;
using Praecon.WinUI.Models.Queries;
using Praecon.WinUI.Models.ViewModels;

internal sealed class ListArticlesHandler : IRequestHandler<ListArticles, IEnumerable<Article>>
{
    private readonly ILogger<ListArticlesHandler> logger;
    private readonly IMapper mapper;
    private readonly IArticleRepository repository;

    public ListArticlesHandler(ILogger<ListArticlesHandler> logger, IMapper mapper, IArticleRepository repository)
        => (this.logger, this.mapper, this.repository) = (logger, mapper, repository);

    public async Task<IEnumerable<Article>> Handle(ListArticles request, CancellationToken cancellationToken)
    {
        var source = await this.repository.ListAsync(cancellationToken);

        var result = this.mapper.Map<IEnumerable<ArticleEntity>, IEnumerable<Article>>(source);

        return await Task.FromResult(result);
    }
}
