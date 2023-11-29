namespace Praecon.WinUI.Models.ViewModels;

public sealed record Article
{
    public DateTime Date { get; set; } = new(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0, DateTimeKind.Utc);
    public Guid Id { get; set; } = Guid.Empty;
    public Guid? MediaId { get; set; } = default;
    public string Payload { get; set; } = string.Empty;
    public bool Published { get; set; } = false;
    public string Tags { get; set; } = string.Empty;
    public Guid? ThumbnailId { get; set; } = default;
    public IReadOnlyList<ThumbnailItem> Thumbnails { get; set; } = new List<ThumbnailItem>();
    public string Title { get; set; } = string.Empty;
}