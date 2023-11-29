namespace Praecon.WinUI.Models.Commands;

internal sealed record UpdateArticle : IRequest
{
    public DateOnly Date { get; set; } = new(year: 2000, month: 1, day: 1);
    public Guid Id { get; set; } = Guid.Empty;
    public Guid? MediaId { get; set; } = default;
    public string Payload { get; set; } = string.Empty;
    public bool Published { get; set; } = false;
    public string Tags { get; set; } = string.Empty;
    public Guid? ThumbnailId { get; set; } = default;
    public string Title { get; set; } = string.Empty;
}
