namespace Praecon.WinUI.Models.Commands;

internal sealed record CreateArticle : IRequest
{
    public required DateOnly Date { get; init; }
    public required Guid Id { get; init; }
    public Guid? MediaId { get; set; } = default;
    public required string Payload { get; init; } = string.Empty;
    public bool Published { get; init; } = default;
    public required string Tags { get; init; }
    public Guid? ThumbnailId { get; set; } = default;
    public required string Title { get; init; } = string.Empty;
}
