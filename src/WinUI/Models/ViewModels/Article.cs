namespace Praecon.WinUI.Models.ViewModels;

public sealed record Article
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTime Date { get; set; } = new DateTime(2000, 1, 1);
    public Guid? MediaId { get; set; } = default;
    public string Payload { get; set; } = string.Empty;
    public Guid? ThumbnailId { get; set; } = default;
    public bool Published { get; set; } = false;
    public string Title { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
}
