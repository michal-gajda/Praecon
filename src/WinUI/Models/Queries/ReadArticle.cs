namespace Praecon.WinUI.Models.Queries;

using Praecon.WinUI.Models.ViewModels;

internal sealed record ReadArticle : IRequest<Article>
{
    public required Guid Id { get; init; }
}
