namespace Praecon.WinUI.Models.Commands;

internal sealed record CreateArticle : IRequest
{
    public required Guid Id { get; init; }
    public required DateOnly Date { get; init; }
    public required string Title { get; init; } = string.Empty;
    public required string Payload { get; init; } = string.Empty;
    public Guid? ThumbnailId { get; set; } = default;
    public Guid? MediaId { get; set; } = default;
    public required string Tags { get; init; }
}
