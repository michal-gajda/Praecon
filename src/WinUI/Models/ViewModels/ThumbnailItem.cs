namespace Praecon.WinUI.Models.ViewModels;

public sealed record ThumbnailItem
{
    public required string Code { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
