namespace Praecon.WinUI.Models.Queries;

using Praecon.WinUI.Models.ViewModels;

internal sealed record ListArticles : IRequest<IEnumerable<Article>>
{
}
