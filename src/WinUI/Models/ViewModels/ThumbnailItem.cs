namespace Praecon.WinUI.Models.ViewModels;

using System.Windows.Media.Imaging;

public sealed record ThumbnailItem
{
    public required string Code { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required BitmapImage? Preview { get; init; }
}
