namespace Praecon.WinUI.Models.Interfaces;

using Praecon.WinUI.Models.Entities;

internal interface IArticleRepository
{
    Task CreateAsync(ArticleEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<ArticleEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<ArticleEntity?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(ArticleEntity entity, CancellationToken cancellationToken = default);
}
